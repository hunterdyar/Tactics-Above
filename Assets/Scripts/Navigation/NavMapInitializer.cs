using System;
using System.Collections;
using HDyar.MapImporter;
using HDyar.RougeLevelGen;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace Tactics
{
	public class NavMapInitializer : MonoBehaviour
	{
		[SerializeField] private ImageToPrefabMap _map;
		[SerializeField] private NavMap _navMap;
		[SerializeField] private Grid _grid;
		[SerializeField] private Transform _worldParent;
		private void Awake()
		{
			// _levelGenerator.OnGenerationComplete += InitMap;
			// _levelGenerator.Generate();
			_map.Spawn(_grid, _worldParent);
			InitMap();
		}

		private void InitMap()
		{
			_navMap.SetWorldGrid(_grid);
			_navMap.Initiate();
		}
#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_grid == null)
			{
				_grid = GetComponent<Grid>();
			}
		}
#endif
	}
}