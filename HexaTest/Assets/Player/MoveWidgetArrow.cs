using UnityEngine;
using System.Collections;

public class MoveWidgetArrow : MonoBehaviour 
{
	void OnMouseUpAsButton()
	{
		GetComponentInParent<MoveWidget>().DirectionSelected(gameObject);
	}
}
