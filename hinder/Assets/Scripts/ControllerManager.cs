using UnityEngine;
using System.Collections;
using System;

public class ControllerManager : MonoBehaviour{

    public event Action<ButtonLabel, PlayerNumber> OnButtonPress;
    public event Action<AnalogLabel, PlayerNumber, float> OnAnalogMovement;

    public enum AnalogLabel
    {
        LeftAnalogX,
        LeftAnalogY,
        RightAnalogX,
        RightAnalogY
    }

    public enum ButtonLabel
    {
        TopButton,
        LeftButton,
        BottomButton,
        RightButton,
        L1Button,
        L2Button,
        R1Button,
        R2Button,
        StartButton
    }

    public enum PlayerNumber
    {
        P1,
        P2
    }

	// Update is called once per frame
	void Update () 
    {
		
		if( OnButtonPress != null )
		{
	        foreach (string button in Enum.GetNames(typeof(ButtonLabel)))
	        {
	            foreach (string playerNum in Enum.GetNames(typeof(PlayerNumber)))
	            {
	                if (Input.GetButtonDown(button + playerNum))
	                {
	                    OnButtonPress((ButtonLabel)Enum.Parse(typeof(ButtonLabel), button), (PlayerNumber) Enum.Parse(typeof(PlayerNumber), playerNum));
	                }
	            }
	        }
		}

		if( OnAnalogMovement != null )
		{
	        foreach (string analog in Enum.GetNames(typeof(AnalogLabel)))
	        {
	            foreach (string playerNum in Enum.GetNames(typeof(PlayerNumber)))
	            {
	                float analogMovement = Input.GetAxis(analog + playerNum);
	                OnAnalogMovement((AnalogLabel)Enum.Parse(typeof(AnalogLabel), analog), (PlayerNumber)Enum.Parse(typeof(PlayerNumber), playerNum), analogMovement);
	            }
	        }
		}
	}
}
