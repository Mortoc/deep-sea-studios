using UnityEngine;
using System.Collections;

public class HexMap : MonoBehaviour 
{
	public int _width = 50;
	public int _height = 50;

	public Material _hexMaterial;

	private Hex[][] _hexes = null;

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

	public void Awake()
	{
		if( _hexes == null )
		{
			_hexes = new Hex[_width][];
			for(var i = 0; i < _width; ++i) 
			{
				_hexes[i] = new Hex[_height];
				for(var j = 0; j < _height; ++j)
				{
					var hexObj = Hex.MakeHex();
					hexObj.name = string.Format ("Hex {0} {1}", i, j);
					hexObj.transform.parent = transform;
					hexObj.transform.localPosition = HexIdxToPos(i, j);
					hexObj.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 90.0f);
					hexObj.transform.localScale = Vector3.one;

					hexObj.GetComponent<Renderer>().sharedMaterial = _hexMaterial;

					_hexes[i][j] = hexObj;
				}
			}
		}

	}
}
