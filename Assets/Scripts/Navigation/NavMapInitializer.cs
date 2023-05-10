using System;
using System.Collections;
using HDyar.RougeLevelGen;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace Tactics
{
	public class NavMapInitializer : MonoBehaviour
	{
		[SerializeField] private LevelGenerator _levelGenerator;
		[SerializeField] private NavMap _navMap;
		[SerializeField] private Grid _grid;

		private void Awake()
		{
			_levelGenerator.OnGenerationComplete += InitMap;
			_levelGenerator.Generate();
		}

		private void InitMap()
		{
			Debug.Log("generation complete. Initiating Grid.");
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