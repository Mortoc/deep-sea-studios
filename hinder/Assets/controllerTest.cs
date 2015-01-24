using UnityEngine;
using System.Collections;

public class controllerTest : MonoBehaviour {

    int updateCount = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        string[] inputNames = new string[]{"TopButton", "LeftButton", "BottomButton", "RightButton", "L1", "L2", "R1", "R2"};
        string[] playerNames = new string[]{"P1", "P2"};

        foreach(string inputName in inputNames)
        {
            foreach(string playerName in playerNames)
            {
                if (Input.GetButtonDown(inputName+playerName))
                {
                    Debug.Log(inputName + " " + playerName);
                }
            }
        }
        if (Input.GetAxis("HorizontalP1") != 0)
        {
            Debug.Log("Horizontal P1");
        }
        if (Input.GetAxis("HorizontalP2") != 0)
        {
            Debug.Log("Horizontal P2");
        }
        if (Input.GetAxis("VerticalP1") != 0)
        {
            Debug.Log("Vertical P1");
        }
        if (Input.GetAxis("VerticalP2") != 0)
        {
            Debug.Log("Vertical P2");
        }
        
        updateCount += 1;
	}
}
