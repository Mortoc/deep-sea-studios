using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.UI;

namespace DSS.Construction
{
    public class EditableStructureHandle : MonoBehaviour
    {
        [SerializeField]
        private Material _hoverMaterial;
        private Material _normalMaterial;

        private EditableStructureVolume _volume;
        private int _structureIndex;
        private ScreenRaycaster _raycaster;
        private TapRecognizer _tapRecognizer;

        public void Init(EditableStructureVolume volume, int structureIndex)
        {
            _volume = volume;
            _structureIndex = structureIndex;

            if (!gameObject.GetComponent<BoxCollider>())
            {
                gameObject.AddComponent<BoxCollider>();
            }

            _normalMaterial = GetComponent<Renderer>().material;

            _tapRecognizer = GetComponent<TapRecognizer>();
            _raycaster = FindObjectOfType<ScreenRaycaster>();
            _tapRecognizer.Raycaster = _raycaster;
        }

        public void OnHoverStart()
        {
            GetComponent<Renderer>().material = _hoverMaterial;
        }

        public void OnHoverEnd()
        {
            GetComponent<Renderer>().material = _normalMaterial;
        }

        void OnTap(TapGesture gesture)
        {
            if (gesture.Selection == gameObject)
            {
                _volume.AddStructure(_structureIndex);
            }
        }
    }
}
