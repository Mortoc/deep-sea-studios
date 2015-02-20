using UnityEngine;
using System.Collections;


namespace Botter
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
