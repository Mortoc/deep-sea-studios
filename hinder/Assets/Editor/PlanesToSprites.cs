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
			foreach(var meshRenderer in Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>())
			{
				var match = _sortingLayerRegex.Match(meshRenderer.gameObject.name);
				int sortingOrder;
				if( int.TryParse(match.Groups[1].Value, out sortingOrder) )
				{
					meshRenderer.sortingOrder = sortingOrder;
				}
			}
		}
	}
}
