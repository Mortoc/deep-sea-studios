using UnityEngine;

using System;
using System.Collections;

public class Bomb : MonoBehaviour 
{
	public int _turnsToExplode = 3;
	private int _i;
	private int _j;
	private HexMap _map;
	private TurnManager _turnManager;

	public TextMesh _countdown;

	public void Init(TurnManager turnManager, HexMap map, int i, int j)
	{
		_i = i;
		_j = j;
		_map = map;
		_turnManager = turnManager;
	}

	public bool IsAt(int i, int j)
	{
		return _i == i && _j == j;
	}

	public bool CountDown()
	{
		_countdown.text = _turnsToExplode.ToString ();
		_turnsToExplode--;

		if( _turnsToExplode == 0 )
		{
			Explode();
			return false;
		}
		return true;
	}

	private bool _isExploding = false;
	public void Explode()
	{
		if( _isExploding ) 
			return;
		_isExploding = true;

		foreach(var hexMapDirection in Enum.GetValues(typeof(HexMap.Direction)))
		{
			var coords = _map.Move(_i, _j, (HexMap.Direction)hexMapDirection);
			_map.DestroyHexAt(coords[0], coords[1]);
		}
		
		_map.DestroyHexAt(_i, _j);
		_turnManager.BombExploded(_i, _j);
		Destroy (gameObject);
	}

}
