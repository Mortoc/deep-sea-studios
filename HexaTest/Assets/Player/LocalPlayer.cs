using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayer : Player
{
	public GameObject _moveWidgetPrefab;
	public GameObject _bombWidgetPrefab;
	private BombWidget _bombWidget;
	private MoveWidget _moveWidget;
	private TurnManager _turnManager;

	void Awake()
	{
		var moveWidgetObj = (GameObject)Instantiate (_moveWidgetPrefab);
		_moveWidget = moveWidgetObj.GetComponent<MoveWidget>();
		_moveWidget.SetPlayer(this);
		_moveWidget.gameObject.SetActive(false);

		var bombWidgetObj = (GameObject)Instantiate (_bombWidgetPrefab);
		_bombWidget = bombWidgetObj.GetComponent<BombWidget>();
		_bombWidget.Init(this);
		_bombWidget.gameObject.SetActive(false);
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

		_turnManager.UpdateLocalPlayerPos(i, j);
		StartCoroutine (AnimateToHex(i, j, () => ShowBombWidget(i, j)));
	}

	private void ShowBombWidget(int i, int j)
	{
		Debug.Log ("Countdowns");
		var explodedBombs = new List<Bomb>();
		_myBombs.ForEach(b => {
			if( !b || !b.CountDown() ) explodedBombs.Add (b);
		});

		explodedBombs.ForEach (b => _myBombs.Remove(b));

		_bombWidget.gameObject.SetActive(true);
		_bombWidget.MoveToPosition(i, j);
	}

	private List<Bomb> _myBombs = new List<Bomb>();

	public void PlaceBomb(int i, int j)
	{
		_bombWidget.gameObject.SetActive(false);
		_myBombs.Add(FindObjectOfType<BombManager>().PlaceBomb(i, j));

		TurnComplete();
	}

	public void TakeTurn(TurnManager turnManager)
	{
		_turnManager = turnManager;
		_moveWidget.gameObject.SetActive(true);
	}

	private void TurnComplete()
	{
		_turnManager.HandoverTurnToNextPlayer();
	}

	
	public override void Murder()
	{
		FindObjectOfType<GameController>().ShowYouLose();
		base.Murder ();
	}
}
