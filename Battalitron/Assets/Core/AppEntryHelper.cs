using UnityEngine;
using System.Collections;


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
