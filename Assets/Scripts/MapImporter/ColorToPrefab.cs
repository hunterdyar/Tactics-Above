using System;
using UnityEngine;

namespace HDyar.MapImporter
{
	[Serializable]
	public class ColorToPrefab
	{
		//serialized
		public Color color;
		public GameObject Prefab;
		
		//not serialized
		[HideInInspector] [SerializeField] public Color[] ColorOptions;
		public GameObject GetPrefab()
		{
			return Prefab;
		}
	}
}