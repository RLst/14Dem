using System.Collections.Generic;

namespace LeMinhHuy.AI
{
	public static class GlobalBlackboard
	{
		//Global blackboard
		private static Dictionary<string, object> globalBlackboard = new Dictionary<string, object>();
		public static void SetData(string key, object value) => globalBlackboard[key] = value;
		public static object GetData(string key)
		{
			object value = null;
			if (globalBlackboard.TryGetValue(key, out value))
				return value;
			return value;
		}
		public static bool ClearData(string key)
		{
			if (globalBlackboard.ContainsKey(key))
			{
				globalBlackboard.Remove(key);
				return true;
			}
			return false;
		}
	}
}