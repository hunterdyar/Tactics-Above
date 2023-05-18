using System;
using Cinemachine;
using Tactics;
using Unity.VisualScripting;
using UnityEngine;

namespace Camera
{
	[RequireComponent(typeof(CinemachineTargetGroup))]
	public class WorldGroupTargetListener : MonoBehaviour
	{
		private CinemachineTargetGroup _cinemachineTargetGroup;
		private Transform[] targets = new Transform[4];
		private void OnEnable()
		{
			NavMap.OnMapSizeChanged+= OnMapSizeChanged;
		}

		private void OnDisable()
		{
			NavMap.OnMapSizeChanged += OnMapSizeChanged;
		}

		private void OnMapSizeChanged(NavMap map,BoundsInt bounds)
		{
			if (targets[0] == null)
			{
				targets[0] = new GameObject().transform;
				targets[0].gameObject.name = "worldMin";
				_cinemachineTargetGroup.AddMember(targets[0], 1, 1);

			}
			targets[0].position = map.Grid.CellToWorld(bounds.min);
			

			if (targets[1] == null)
			{
				targets[1] = new GameObject().transform;
				targets[1].gameObject.name = "worldMax";
				_cinemachineTargetGroup.AddMember(targets[1], 1, 1);

			}
			targets[1].position = map.Grid.CellToWorld(bounds.max);

			if (targets[2] == null)
			{
				targets[2] = new GameObject().transform;
				targets[2].gameObject.name = "worldMinMax";
				_cinemachineTargetGroup.AddMember(targets[2], 1, 1);

			}

			targets[2].position = map.Grid.CellToWorld(new Vector3Int(bounds.min.x, bounds.min.y, bounds.max.z));

			if (targets[3] == null)
			{
				targets[3] = new GameObject().transform;
				targets[3].gameObject.name = "worldMaxMin";
				_cinemachineTargetGroup.AddMember(targets[3], 1, 1);

			}

			targets[3].position = map.Grid.CellToWorld(new Vector3Int(bounds.max.x, bounds.min.y, bounds.min.z));
		}

		private void OnValidate()
		{
			if (_cinemachineTargetGroup == null)
			{
				_cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
			}
		}
	}
}