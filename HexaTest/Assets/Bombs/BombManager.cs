using UnityEngine;
using System.Collections;

public class BombManager : MonoBehaviour 
{
	public HexMap _map;
	public GameObject _bombPrefab;
	public GameController _controller;


	public Bomb PlaceBomb(int i, int j)
	{
		var bomb = (GameObject)Instantiate(_bombPrefab);
		bomb.transform.position = _map.HexIdxToPos(i, j);
		var bombComponent = bomb.GetComponent<Bomb>();
		bombComponent.Init(_controller.TurnManager, _controller.Map, i, j);
		_controller.TurnManager.PlaceBomb(i, j);
		return bombComponent;
	}

	public Bomb PlaceForeignBomb(int i, int j)
	{
		var bomb = (GameObject)Instantiate(_bombPrefab);
		bomb.transform.position = _map.HexIdxToPos(i, j);
		var bombComponent = bomb.GetComponent<Bomb>();
		bombComponent.Init(_controller.TurnManager, _controller.Map, i, j);
		return bombComponent;
	}
}
