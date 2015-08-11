using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DSS
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _sound;

        private void PlaySound()
        {
			CameraRig.PlaySoundAtCamera(_sound);
        }

        void OnEnable()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(PlaySound);
        }

        void OnDisable()
        {
            var btn = GetComponent<Button>();
            btn.onClick.RemoveListener(PlaySound);
        }
    }

}
