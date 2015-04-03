using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class RobotAssemblyUI : MonoBehaviour
    {
        [SerializeField]
        private RobotAssembly _assembly;

        [SerializeField]
        private Image _container;

        [SerializeField]
        private Button _prototypeComponentButton;

        [SerializeField]
        private float _buttonMargin = 5.0f;

        void Start()
        {
            var buttonRect = _prototypeComponentButton.GetComponent<RectTransform>();
            var buttonY = buttonRect.anchoredPosition.y;
            var buttonOffset = buttonRect.rect.width + _buttonMargin;
            var currentOffset = buttonRect.anchoredPosition.x;

            _prototypeComponentButton.transform.parent = null;
            foreach (var part in _assembly.Parts)
            {
                var newButton = Instantiate(_prototypeComponentButton.gameObject);
                var newButtonTrans = newButton.GetComponent<RectTransform>();
                
                newButtonTrans.transform.parent = _container.transform;
                newButtonTrans.anchoredPosition = new Vector2(currentOffset, buttonY);

                currentOffset += buttonOffset;

                newButton.GetComponentInChildren<Text>().text = part.Name;

                var button = newButton.GetComponent<Button>();
                var thisPart = part;
                button.onClick.AddListener(() => { _assembly.PartSelected(thisPart); });
            }
        }
    }
}
