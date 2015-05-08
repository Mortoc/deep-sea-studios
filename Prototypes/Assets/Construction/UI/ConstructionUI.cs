using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class ConstructionUI : MonoBehaviour
    {
        [Serializable]
        public struct ConstructionPart
        {
            public string Name;

            public GameObject Prefab;
        }

        [SerializeField]
        private ConstructionPart[] _parts;

        [SerializeField]
        private Button _partBtnPrototype;


        [SerializeField]
        private Button _deleteButton;

        void Start()
        {
            var btnParent = _partBtnPrototype.transform.parent;
            _partBtnPrototype.transform.SetParent(null);

            var nextPosition = 0.0f;
            foreach(var part in _parts)
            {
                var newButton = Instantiate<GameObject>(_partBtnPrototype.gameObject);
                var newBtnTransform = newButton.transform.GetComponent<RectTransform>();
                var pos = newBtnTransform.position;
                pos.x = nextPosition;
                newBtnTransform.position = pos;

                nextPosition += newBtnTransform.rect.width;

                newButton.GetComponentInChildren<Text>().text = part.Name;

                var prefab = part.Prefab;
                var btn = newButton.GetComponentInChildren<Button>();
                
                btn.onClick.AddListener(() =>
                {
                    BuildNewPart(prefab);
                });

                btn.transform.SetParent(btnParent);
            }

            _deleteButton.onClick.AddListener(DeleteToolSelected);
        }

        void BuildNewPart(GameObject prefab)
        {
            Debug.Log(prefab.name);
        }

        public void DeleteToolSelected()
        {
        }
    }
}
