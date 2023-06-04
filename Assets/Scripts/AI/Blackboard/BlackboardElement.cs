﻿using System;
using UnityEngine;

namespace Tactics.AI.Blackboard
{
	[System.Serializable]
	[AttributeUsage(validOn: System.AttributeTargets.Method | System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class BlackboardElement : Attribute
	{
		public string Name;

		public delegate object GetValueDelegate();

		public System.Type attribueType;
		public GetValueDelegate GetValue;
	}
}
