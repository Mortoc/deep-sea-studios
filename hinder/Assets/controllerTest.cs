using UnityEngine;
using System.Collections;
using System;


public class controllerTest : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        var controllerManager = GameObject.FindObjectOfType<ControllerManager>();
        controllerManager.OnButtonPress += OnButtonPress;
        controllerManager.OnAnalogMovement += OnAnalogMovement;
	}
	
	
    void OnButtonPress(ControllerManager.ButtonLabel bl, ControllerManager.PlayerNumber pn)
    {
        Debug.Log(bl.ToString() + " " + pn.ToString());
    }

    void OnAnalogMovement(ControllerManager.AnalogLabel al, ControllerManager.PlayerNumber pn, float val)
    {
        Debug.Log(al.ToString() + " " + pn.ToString() + " " + val);
    }

}
