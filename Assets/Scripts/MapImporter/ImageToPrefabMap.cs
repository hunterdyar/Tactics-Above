﻿using System.Collections.Generic;
using UnityEngine;

namespace HDyar.MapImporter
{
	[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
	public class ImageToPrefabMap : ScriptableObject
	{
		public Texture2D _mapTexture;
		public int Width => _mapTexture.width;
		public int Height => _mapTexture.height;
		[HideInInspector] [SerializeField] private ColorToPrefab[] _colors;
		private Dictionary<Color, ColorToPrefab> _colorToPrefabMap = new Dictionary<Color, ColorToPrefab>();
		public void SetMapTexture(Texture2D texture)
		{
			_mapTexture = texture;
		}

		public void SetPrefabColors(ColorToPrefab[] newColors)
		{
			_colors = newColors;
			InitializeColorDictionary();
		}

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			Spawn(null);
		}
		public void Spawn(Transform parent)
		{
			InitializeColorDictionary();
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					//get color
					var color = _mapTexture.GetPixel(i, j);
					if (_colorToPrefabMap.TryGetValue(color, out var colorToPrefab))
					{
						var prefab = colorToPrefab.GetPrefab();
						Instantiate(prefab, new Vector3(i, 0, j), prefab.transform.rotation,parent);
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
				if (!_colorToPrefabMap.ContainsKey(cp.color))
				{
					_colorToPrefabMap.Add(cp.color,cp);
				}
				else
				{
					Debug.LogWarning("Can't have two of the same color.");
				}
			}
		}
	}
}