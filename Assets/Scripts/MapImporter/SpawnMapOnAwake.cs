using System;
using UnityEngine;

namespace HDyar.MapImporter
{
	public class SpawnMapOnAwake : MonoBehaviour
	{
		public ImageToPrefabMap map;
		private void Awake()
		{
			map.Spawn(transform);
		}
	}
}