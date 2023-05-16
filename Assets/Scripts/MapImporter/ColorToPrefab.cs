using System;
using UnityEngine;

namespace HDyar.MapImporter
{
	[Serializable]
	public class ColorToPrefab
	{
		//serialized
		public Color color;
		public GameObject PrefabUpper;
		public GameObject PrefabLower;

		//not serialized
		[HideInInspector] [SerializeField] public Color[] ColorOptions;
		public GameObject GetUpperPrefab()
		{
			return PrefabUpper;
		}

		public GameObject GetLowerPrefab()
		{
			return PrefabLower;
		}
	}
}