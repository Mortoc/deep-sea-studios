using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;


public class HexMap : MonoBehaviour 
{
	public enum Direction 
	{
		Left,
		UpLeft,
		UpRight,
		Right,
		DownRight,
		DownLeft
	}

	public int _width = 50;
	public int Width 
	{ 
		get { return _width; } 
	}
	public int _height = 50;
	public int Height 
	{ 
		get { return _height; } 
	}

	public Material _hexMaterial;

	private bool[] _existingHexes;
	private Hex[][] _hexes = null;

	public Hex GetRandomValidHex()
	{
		var hexes = GetComponentsInChildren<Hex>();
		return hexes[UnityEngine.Random.Range (0, hexes.Length)];
	}

	public Vector3 HexIdxToPos(int i, int j)
	{
		var xOffset = 0.86f;
		var yOffset = 0.75f;
		var result = Vector3.zero;
		result.x = (float)i * xOffset;
		if( j % 2 == 0 )
			result.x += xOffset * 0.5f;

		result.y = (float)j * yOffset;
		return result;
	}

	public bool IsValidIdx(int i, int j)
	{
		return i >= 0 && i < _width &&
				j >= 0 && j < _height &&
				_hexes[i][j] != null;
	}
	
	public bool IsValidIdx(int[] idx)
	{
		return IsValidIdx (idx[0], idx[1]);
	}

	public bool CanMove(int i, int j, Direction dir)
	{
		return IsValidIdx(Move (i, j, dir));
	}

	public int[] Move(int i, int j, Direction dir)
	{
		var isOffsetRow = j % 2 == 0;

		var result = new int[2];
		switch(dir)
		{
		case Direction.Left:
			result[0] = i - 1;
			result[1] = j;
			break;
			
		case Direction.UpLeft:
			result[0] = i - 1;
			result[1] = j + 1;
			if( isOffsetRow ) result[0]++;
			break;
			
		case Direction.UpRight:
			result[0] = i;
			result[1] = j + 1;
			if( isOffsetRow ) result[0]++;
			break;
			
		case Direction.Right:
			result[0] = i + 1;
			result[1] = j;
			break;
			
		case Direction.DownRight:
			result[0] = i;
			result[1] = j - 1;
			if( isOffsetRow ) result[0]++;
			break;
			
		case Direction.DownLeft:
			result[0] = i - 1;
			result[1] = j - 1;
			if( isOffsetRow ) result[0]++;
			break;
			
		default:
			throw new NotImplementedException();
		}

		return result;
	}

	public void DestroyHexAt(int i, int j)
	{
		_existingHexes[(i * j) + i] = false;
		Destroy (_hexes[i][j]);
		_hexes[i][j] = null;
	}

	private void Cleanup()
	{
		for(var i = 0; i < _width; ++i) 
		{
			for(var j = 0; j < _height; ++j)
			{
				Destroy (_hexes[i][j]);
			}
		}
	}

	private Hex CreateHexAt(int i, int j)
	{
		var hexObj = Hex.MakeHex();
		hexObj.name = string.Format ("Hex {0} {1}", i, j);
		hexObj.transform.parent = transform;
		hexObj.transform.localPosition = HexIdxToPos(i, j);
		hexObj.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 90.0f);
		hexObj.transform.localScale = Vector3.one;
		hexObj.i = i;
		hexObj.j = j;
		
		hexObj.GetComponent<Renderer>().sharedMaterial = _hexMaterial;
		return hexObj;
	}

	public void InitializeBoard()
	{
		if( _hexes != null ) 
		{
			Cleanup();
		}

		_existingHexes = new bool[_width * _height];
		_hexes = new Hex[_width][];
		for(var i = 0; i < _width; ++i) 
		{
			_hexes[i] = new Hex[_height];
			for(var j = 0; j < _height; ++j)
			{
				_existingHexes[(i * j) + i] = true;
				var hexObj = CreateHexAt(i, j);

				_hexes[i][j] = hexObj;
			}
		}
	}

	public void SetBoardByCustomProperties(Hashtable customProps)
	{
		if( _existingHexes == null ) 
			InitializeBoard();

		object remoteHexesRaw;
		if( customProps.TryGetValue("hexState", out remoteHexesRaw) && remoteHexesRaw != null ) 
		{
			var remoteHexes = new BitArray((bool[])remoteHexesRaw);
			var localHexes = new BitArray(_existingHexes);
			var difference = localHexes.Or(remoteHexes);

			var index = new int[] { 0, 0 };
			for(var i = 0; i < _width * _height; ++i)
			{
				if( difference[i] )
				{
					if( remoteHexes[i] )
					{
						CreateHexAt(index[0], index[1]);
					}
					else
					{
						DestroyHexAt(index[0], index[1]);
					}
				}
				
				index[0]++;
				if( index[0] == _width )
				{
					index[0] = 0;
					index[1]++;
				}
			}
		}
	}

	protected internal Hashtable GetBoardAsCustomProperties()
	{
		var customProps = new Hashtable();
		customProps["hexState"] = _existingHexes;
		return customProps;
	}
}
