﻿using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public class ConstructionUI : MonoBehaviour
    {
        [Serializable]
        public struct ConstructionPart
        {
            public string Name;

            public ConstructionTool Tool;
        }

        [SerializeField]
        private Color _selectedPartColor;

        [SerializeField]
        private ConstructionPart[] _parts;

        [SerializeField]
        private Button _partBtnPrototype;

        [SerializeField]
        private Button _deleteButton;

        private ConstructionTool _activeTool = null;

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

                var tool = part.Tool;
                var btn = newButton.GetComponentInChildren<Button>();
                
                btn.onClick.AddListener(() =>
                {
                    ToolButtonClicked(tool, btn);
                });

                btn.transform.SetParent(btnParent);
            }

            _deleteButton.onClick.AddListener(DeleteToolSelected);
        }

        private void ToolButtonClicked(ConstructionTool tool, Button btn)
        {
            if (tool == _activeTool)
                return;

            ActivateTool(tool);
            foreach (var potentiallyOnButton in btn.transform.parent.GetComponentsInChildren<Button>())
            {
                potentiallyOnButton.colors = _partBtnPrototype.colors;
                var pos = potentiallyOnButton.transform.position;
                pos.y = _partBtnPrototype.transform.position.y;
                potentiallyOnButton.transform.position = pos;
            }
            var colors = btn.colors;
            colors.normalColor = _selectedPartColor;
            colors.highlightedColor = _selectedPartColor;
            btn.colors = colors;

            var selectedPos = btn.transform.position;
            selectedPos.y = _partBtnPrototype.transform.position.y + 10.0f;
            btn.transform.position = selectedPos;
        }

        private void ActivateTool(ConstructionTool tool)
        {
            if( _activeTool )
            {
                _activeTool.OnDeselect();
            }
            _activeTool = tool;

            if (_activeTool)
            {
                _activeTool.OnSelect();
            }
        }


        public void DeleteToolSelected()
        {
        }
    }
}
