using UnityEngine;
using System.Collections.Generic;


namespace BackstreetBots
{
    public class AppEntryHelper : MonoBehaviour
    {
        void Start()
        {
            if( !FindObjectOfType<AppEntry>() )
            {
                Application.LoadLevel("AppEntry");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
