using UnityEngine;
using System.Collections.Generic;

public class MoveWidget : MonoBehaviour 
{
	public GameObject _right;
	public GameObject _upRight;
	public GameObject _downRight;
	public GameObject _left;
	public GameObject _upLeft;
	public GameObject _downLeft;

	private LocalPlayer _player = null;
	private int hexi;
	private int hexj;

	public void SetPlayer(LocalPlayer player)
	{
		_player = player;
	}

	public void MoveToPosition(int i, int j)
	{
		hexi = i;
		hexj = j;

		transform.position = _player.Map.HexIdxToPos(i, j);
		
		_right.SetActive(_player.Map.CanMove(i, j, HexMap.Direction.Right));
		_upRight.SetActive(_player.Map.CanMove(i, j, HexMap.Direction.UpRight));
		_downRight.SetActive(_player.Map.CanMove(i, j, HexMap.Direction.DownRight));
		_left.SetActive(_player.Map.CanMove(i, j, HexMap.Direction.Left));
		_upLeft.SetActive(_player.Map.CanMove(i, j, HexMap.Direction.UpLeft));
		_downLeft.SetActive(_player.Map.CanMove(i, j, HexMap.Direction.DownLeft));
	}

	public void DirectionSelected(GameObject directionObj)
	{
		var dir = HexMap.Direction.Left;
		if( directionObj == _right ) dir = HexMap.Direction.Right;
		else if( directionObj == _upRight ) dir = HexMap.Direction.UpRight;
		else if( directionObj == _downRight ) dir = HexMap.Direction.DownRight;
		else if( directionObj == _upLeft ) dir = HexMap.Direction.UpLeft;
		else if( directionObj == _downLeft ) dir = HexMap.Direction.DownLeft;

		var coords = _player.Map.Move(hexi, hexj, dir);
		_player.MoveToHex(coords[0], coords[1]);
	}
}
