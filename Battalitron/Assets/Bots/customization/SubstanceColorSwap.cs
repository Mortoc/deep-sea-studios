using UnityEngine;
using System.Collections;

public class SubstanceColorSwap : MonoBehaviour 
{
	[SerializeField]
	private ProceduralMaterial _substance;

	[SerializeField]
	private string _colorName;

	public void SetColorRed()
	{
		SetColor(Color.red);
	}
	
	public void SetColorGreen()
	{
		SetColor(Color.green);
	}
	
	public void SetColorBlue()
	{
		SetColor(Color.blue);
	}

	public void SetColor(Color c)
	{
		_substance.SetProceduralColor(_colorName, c);
		_substance.RebuildTextures();
	}
}
