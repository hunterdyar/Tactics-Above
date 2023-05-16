using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HDyar.MapImporter
{
	public class ImageToPrefabMap : ScriptableObject
	{
		public Texture2D _mapTexture;
		public int Width => _mapTexture.width;
		public int Height => _mapTexture.height;

		public Vector3Int TotalGridOffset = Vector3Int.zero;
		public int LowerYOffset = -1;
		//[HideInInspector]
		public Color[] AllColorsInTexture;
		
		[SerializeField] private ColorToPrefab[] _colors;
		private Dictionary<string, ColorToPrefab> _colorToPrefabMap = new Dictionary<string, ColorToPrefab>();
		public void SetMapTexture(Texture2D texture)
		{
			_mapTexture = texture;
		}

		//Asset needs to get saved here, or the editor needs to get updated directly.
		public void SetPrefabColors(ColorToPrefab[] newColors)
		{
			_colors = newColors;
			InitializeColorDictionary();
		}

		public void Spawn(Grid grid)
		{
			Spawn(grid, null);
		}
		public void Spawn(Grid grid,Transform parent)
		{
			InitializeColorDictionary();
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					//get color
					var color = ColorUtility.ToHtmlStringRGB(_mapTexture.GetPixel(i, j));
					if (_colorToPrefabMap.TryGetValue(color, out var colorToPrefab))
					{
						var prefab = colorToPrefab.GetUpperPrefab();
						if (prefab != null)
						{
							
							Instantiate(prefab, grid.CellToLocal(TotalGridOffset+new Vector3Int(i, 0, j)), prefab.transform.rotation, parent);
						}

						prefab = colorToPrefab.GetLowerPrefab();
						if (prefab != null)
						{
							Instantiate(prefab, grid.CellToWorld(TotalGridOffset +new Vector3Int(i, LowerYOffset, j)), prefab.transform.rotation, parent);
						}
					}
					//insantiate
				}
			}
		}

		private void InitializeColorDictionary()
		{
			_colorToPrefabMap.Clear();
			foreach (var cp in _colors)
			{
				var hexColor = ColorUtility.ToHtmlStringRGB(cp.color);
				if (!_colorToPrefabMap.ContainsKey(hexColor))
				{
					_colorToPrefabMap.Add(hexColor,cp);
				}
				else
				{
					Debug.LogWarning("Can't have two of the same color.");
				}
			}
		}
	}
}