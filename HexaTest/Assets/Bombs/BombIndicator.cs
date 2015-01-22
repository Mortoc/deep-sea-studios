using UnityEngine;
using System.Collections;

public class BombIndicator : MonoBehaviour 
{
	void OnMouseUpAsButton()
	{
		GetComponentInParent<BombWidget>().DirectionSelected(gameObject);
	}
}
