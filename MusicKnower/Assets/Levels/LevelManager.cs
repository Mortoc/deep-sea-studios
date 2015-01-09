using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public void ResetLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
