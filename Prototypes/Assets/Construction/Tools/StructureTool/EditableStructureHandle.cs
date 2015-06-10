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

        public void Init(EditableStructureVolume volume, int structureIndex)
        {
            _volume = volume;
            _structureIndex = structureIndex;

            if (!gameObject.GetComponent<BoxCollider>())
            {
                gameObject.AddComponent<BoxCollider>();
            }

            _normalMaterial = GetComponent<Renderer>().material;
        }

        public void OnHoverStart()
        {
            GetComponent<Renderer>().material = _hoverMaterial;
        }
        public void OnHoverEnd()
        {
            GetComponent<Renderer>().material = _normalMaterial;
        }

        //public void OnMouseUpAsButton()
        //{
        //    _volume.AddStructure(_structureIndex);
        //}
    }
}
