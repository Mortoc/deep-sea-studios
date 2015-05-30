using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
    public class ConstructionUI : GameStateManager
    {
        [SerializeField]
        private float _fadeOutTime = 0.2f;

        [SerializeField]
        private float _fadeInTime = 0.1f;

        [SerializeField]
        public GameObject _structureSelectionEffect;

        [SerializeField]
        public GameObject _componentSelectionEffect;

        [SerializeField]
        public GameObject _robotHeadSelectionEffect;

        [SerializeField]
        public GameObject _deleteButtonSelectionEffect;

        public IEnumerator FadeOutSelection(GameObject ui)
        {
            if( !ui.activeSelf )
            {
                yield break;
            }

            var image = ui.GetComponentInChildren<Image>();
            var invFloatTime = 1.0f / _fadeOutTime;
            for(var t = 0.0f; t < _fadeOutTime; t += Time.deltaTime)
            {
                var color = image.color;
                color.a = 1.0f - t * invFloatTime;
                image.color = color;
                yield return 0;
            }

            ui.SetActive(false);
        }

        public IEnumerator FadeInSelection(GameObject ui)
        {
            ui.SetActive(true);

            var image = ui.GetComponentInChildren<Image>();
            var invFloatTime = 1.0f / _fadeInTime;
            var color = image.color;
            for (var t = 0.0f; t < _fadeInTime; t += Time.deltaTime)
            {
                color.a = t * invFloatTime;
                image.color = color;
                yield return 0;
            }
            
            color.a = 1.0f;
            image.color = color;
        }

        private void DeactivateAll()
        {
            StartCoroutine(FadeOutSelection(_structureSelectionEffect));
            StartCoroutine(FadeOutSelection(_componentSelectionEffect));
            StartCoroutine(FadeOutSelection(_robotHeadSelectionEffect));
            StartCoroutine(FadeOutSelection(_deleteButtonSelectionEffect));
        }

        public void StructureSelected()
        {
            DeactivateAll();
            StartCoroutine(FadeInSelection(_structureSelectionEffect));

        }

        public void ComponentSelected()
        {
            DeactivateAll();
            StartCoroutine(FadeInSelection(_componentSelectionEffect));

        }

        public void RobotHeadSelected()
        {
            DeactivateAll();
            StartCoroutine(FadeInSelection(_robotHeadSelectionEffect));
        }

        public void DeleteSelected()
        {
            DeactivateAll();
            StartCoroutine(FadeInSelection(_deleteButtonSelectionEffect));

        }
    }
}
