using UnityEngine;
using UnityEditor;

using System.Text.RegularExpressions;
using System.Collections.Generic;

public static class PlanesToSprites
{
	private static Regex _sortingLayerRegex = new Regex("sort([0-9])");

	[MenuItem("Tools/Assign sorting order")]
	public static void ConvertPlanesToSprites()
	{
		if( Selection.activeGameObject ) 
		{
			foreach(var renderer in Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				var match = _sortingLayerRegex.Match(renderer.gameObject.name);
				int sortingOrder;
				if( int.TryParse(match.Groups[1].Value, out sortingOrder) )
				{
                    Debug.Log("Setting sort order " + sortingOrder + " on ", renderer.gameObject);
					renderer.sortingOrder = sortingOrder;
				}
			}
		}
	}
}
