using System.Linq;
using Tactics.AI.Blackboard;
using Tactics.Utility;
using UnityEngine;

namespace Tactics.AI.InfluenceMaps
{
	public partial class InfluenceMap
	{
		//An influence map is a grid of floats.
		//It is used to represent the influence of a certain type of entity on the map.
		//We will use it for territory, distance to allies and enemies. We can combine them in clever ways to define things like "a point on the front lines that is near me and not near my allies", and have tankier enemies move there.
		//similar effects with scouts and knowledge/fog of war maps, and safe locations for support units.
		//we will also use it for threat maps. A collection and creation of influence maps will be the main input to the Utility AI system.
		
		//Factory to create and combine maps.
		//Cache system to avoid creating the same map twice, especially with distance functions. - give range and falloff type
		//preview system to show the map in the editor as Texture2D.
		
		
		
		//2D grid of floats or Texture2D, or dictionary of floats.
		
		private NavMap _navMap;
		private float _defaultValue;
		private float[,] _grid;
		public int Width;
		public int Height;
		private float _min => GetLowestPointValue();
		private float _max => GetHighestPointValue();
		public static InfluenceMap New(NavMap navMap, float defaultValue = 0f)
		{
			return new InfluenceMap(navMap, defaultValue);
		}
		
		public InfluenceMap(NavMap navMap, float defaultValue = 0f)
		{	
			_navMap = navMap;
			_defaultValue = defaultValue;
			var bounds = navMap.GridSpaceBounds;
			if (bounds.xMin != 0 || bounds.yMin != 0 || bounds.zMin != 0)
			{
				Debug.LogWarning("Influence map bounds are not zeroed at min. This is not supported yet.");
			}
			Width = bounds.xMax;
			Height = bounds.zMax;

			_grid = new float[bounds.xMax, bounds.zMax];
			for (int x = 0; x < bounds.xMax; x++)
			{
				for (int y = 0; y < bounds.zMax; y++)
				{
					_grid[x, y] = defaultValue;
				}
			}
		}

		public float GetValue(NavNode node)
		{
			return _grid[node.GridPosition.x, node.GridPosition.z];
		}
		public float GetValue(Vector2Int position)
		{
			return _grid[position.x, position.y];
		}

		public float GetValue(int x, int y)
		{
			if (x > 0 && x < Width && y > 0 && y < Height)
			{
				return _grid[x, y];
			}
			//Debug.LogWarning("Bad Value Selected");
			return 0;
		}
		
		public void SetValue(int x, int y, float value)
		{
			if (x > 0 && x < Width && y > 0 && y < Height)
			{
				_grid[x, y] = value;
			}
		}

		public void AddValue(int x, int y, float value)
		{
			if (x > 0 && x < Width && y > 0 && y < Height)
			{
				_grid[x, y] += value;
			}
		}

		public void MultiplyValue(int x, int y, float value)
		{
			if (x > 0 && x < Width && y > 0 && y < Height)
			{
				_grid[x, y] *= value;
			}
		}

		[BlackboardElement(Name = "Highest Point Value")]
		public float GetHighestPointValue()
		{
			float max = Mathf.NegativeInfinity;
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (max < _grid[x, y])
					{
						max = _grid[x, y];
					}
				}
			}

			return max;
		}

		[BlackboardElement(Name = "Lowest Point Value")]
		public float GetLowestPointValue()
		{
			float min = Mathf.Infinity;
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (min > _grid[x, y])
					{
						min = _grid[x, y];
					}
				}
			}

			return min;
		}

		public void AddInfluence(InfluenceMap otherMap, float modifier = 1f)
		{
			//add values of other map to this map.
			for (int ox = 0; ox < otherMap.Width; ox++)
			{
				for (int oy = 0; oy < otherMap.Height; oy++)
				{
					AddValue(ox,oy,modifier*otherMap.GetValue(ox,oy));
				}
			}
		}

		public void MultiplyInfluence(InfluenceMap otherMap)
		{
			//add values of other map to this map.
			for (int ox = 0; ox < otherMap.Width; ox++)
			{
				for (int oy = 0; oy < otherMap.Height; oy++)
				{
					MultiplyValue(ox, oy, otherMap.GetValue(ox, oy));
				}
			}
		}
		
		public void AddPropagation(Vector2Int center, float range, DistanceFalloff falloff = DistanceFalloff.Linear)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var pos = new Vector2Int(x, y);
					AddValue(x, y, InfluenceMap.GetDistanceValue(pos, center, range, falloff));
				}
			}
		}
		public void Normalize()
		{
			//get highest value, divide all values by that.
			float modifier = 1f / GetHighestPointValue();
			for (int x = 0; x <Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					_grid[x, y] *= modifier;
				}
			}
		}
		

		public static float GetDistanceValue(Vector2Int position, Vector2Int center, float range, DistanceFalloff falloff = DistanceFalloff.Linear)
		{
			switch (falloff)
			{
				case DistanceFalloff.Linear:
					return 1f - Mathf.Clamp01(Vector2Int.Distance(position, center) / range);
				case DistanceFalloff.Exponential:
					return 1f - Mathf.Clamp01(Mathf.Pow(Vector2Int.Distance(position, center) / range, 2));
				case DistanceFalloff.Quadratic:
					return 1f - Mathf.Clamp01(Mathf.Pow(Vector2Int.Distance(position, center) / range, 4));
				case DistanceFalloff.None:
					return center.ManhattanDistance(position) <= range ? 1 : 0;
			}
			return 0f;
		}

		public Texture GetMapAsTexture(Gradient g = null, float min = 0, float max = 1)
		{
			if (g == null)
			{
				g = new Gradient();
				g.SetKeys(new GradientColorKey[] {new GradientColorKey(Color.black, 0f), new GradientColorKey(Color.white, 1f)}, new GradientAlphaKey[] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f)});
			}
			var texture = new Texture2D(Width, Height); 
			texture.filterMode = FilterMode.Point;
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y <Height; y++)
				{
					texture.SetPixel(x, y, g.Evaluate(Mathf.InverseLerp(min,max,_grid[x, y])));
				}
			}
			texture.Apply();
			return texture;
		}

		public static InfluenceMap Clone(InfluenceMap otherMap)
		{
			var map = new InfluenceMap(otherMap._navMap, otherMap._defaultValue);
			map._grid = (float[,]) otherMap._grid.Clone();
			map.Width = otherMap.Width;
			map.Height = otherMap.Height;
			return map;
		}

		public Vector2Int GetHighestPosition()
		{
			Vector2Int maxSpace = Vector2Int.zero;
			float max = GetValue(0,0);
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (max < _grid[x, y])
					{
						max = _grid[x, y];
					}
					
				}
			}

			return maxSpace;
		}

		public Vector2Int GetLowestPosition()
		{
			Vector2Int minSpace = Vector2Int.zero;
			float min = GetValue(0,0);
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (min > _grid[x, y])
					{
						min = _grid[x, y];
						minSpace = new Vector2Int(x, y);
					}
				}
			}

			return minSpace;
		}
	}
}