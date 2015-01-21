using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;

public class LocalPlayer : Player
{
	public GameObject _moveWidgetPrefab;
	private MoveWidget _moveWidget;
	private TurnManager _turnManager;

	void Awake()
	{
		var moveWidgetObj = (GameObject)Instantiate (_moveWidgetPrefab);
		_moveWidget = moveWidgetObj.GetComponent<MoveWidget>();
		_moveWidget.SetPlayer(this);
		_moveWidget.gameObject.SetActive(false);
	}

	public void SetInitialPosition(int i, int j)
	{
		_moveWidget.MoveToPosition(i, j);
		_moveWidget.gameObject.SetActive(false);
		transform.position = Map.HexIdxToPos(i, j);
	}

	public void MoveToHex(int i, int j)
	{
		_moveWidget.MoveToPosition(i, j);
		_moveWidget.gameObject.SetActive(false);

		StartCoroutine (AnimateToHex(i, j));
		
		TurnComplete(i, j);
	}

	public void TakeTurn(TurnManager turnManager)
	{
		_turnManager = turnManager;
		_moveWidget.gameObject.SetActive(true);
	}

	private void TurnComplete(int i, int j)
	{
		_turnManager.UpdateLocalPlayerPos(i, j);
		_turnManager.HandoverTurnToNextPlayer();
	}
}
