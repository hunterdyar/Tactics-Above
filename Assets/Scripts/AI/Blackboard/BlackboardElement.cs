using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Tactics.AI.Blackboard
{
	[System.Serializable]
	[AttributeUsage(validOn: System.AttributeTargets.Method | System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class BlackboardElement : Attribute
	{
		public string Name = "";

		public delegate object GetValueDelegate();

		public System.Type attribueType;

		public MethodInfo method;
		public object context;//method.invoke(context,null);
		public bool IsInitiated => method != null && context != null;
		public float GetValueAsFloat(float fallback = 0)
		{
			object o = null;
			o = method.Invoke(context,null);
			
			if (o == null)
			{
				Debug.LogError("blackboard delegate return value null");
				return 0;
			}
			if (o is float f)
			{
				return f;
			}

			if (o is int i)
			{
				return (float)i;
			}

			if (o is double d)
			{
				return (float)d;
			}
			
			if (o is bool b)
			{
				return b ? 1f : 0f;
			}
			
			//toString is slow. But is it as slow as doing the 5 above? Would be interesting to profile, maybe string-and-back isn't that slow.
			if(float.TryParse(o.ToString(), out var a))
			{
				return a;
			}

			return fallback;
		}

		public object GetValueObject()
		{
			return method.Invoke(context, null);
		}
	}
}
