using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


namespace DSS
{
    public class RobotAssembly : MonoBehaviour 
    {
        [SerializeField]
        private GameObject _corePrefab;
        [SerializeField]
        private float _coreDistance = 10.0f;

        [SerializeField]
        private float _rotSpeed = 1.0f;

        private GameObject _core;


	    [SerializeField]
	    private RobotComponent[] _parts;

        [SerializeField]
        private float _unattachedDistFromCamera = 4.0f;

        private GameObject _currentlyPlacingPart = null;
        private Coroutine _placePartCoroutine = null;

        private int _robotAssemblyMask;

        public IEnumerable<RobotComponent> Parts
        {
            get { return _parts; }
        }

        private void Start()
        {
            _core = Instantiate<GameObject>(_corePrefab);
            _core.transform.position = Camera.main.ViewportPointToRay(Vector3.one * 0.5f).GetPoint(_coreDistance);
            _core.layer = LayerMask.NameToLayer("RobotAssembly");
            _robotAssemblyMask = 1 << LayerMask.NameToLayer("RobotAssembly");
        }

        public void PartSelected(RobotComponent part)
        {
            if (_currentlyPlacingPart)
            {
                Destroy(_currentlyPlacingPart);
                StopCoroutine(_placePartCoroutine);
            }

            _placePartCoroutine = StartCoroutine(PlacePart(part));
        }
        
        private Vector3 _lastMousePos;
        void Update()
        {
            if( Input.GetMouseButton(0) && !_currentlyPlacingPart )
            {
                var mouseDelta = _lastMousePos - Input.mousePosition;
                Debug.Log(mouseDelta);
                _core.transform.Rotate(Vector3.up * mouseDelta.x * _rotSpeed);
            }
            _lastMousePos = Input.mousePosition;
        }

        private IEnumerator PlacePart(RobotComponent part)
        {
            _currentlyPlacingPart = Instantiate<RobotComponent>(part).gameObject;
            
            var placed = false;

            while(!placed)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rh;
                if (Physics.Raycast(mouseRay, out rh, 100.0f, _robotAssemblyMask))
                {
                    _currentlyPlacingPart.transform.position = rh.point;
                    _currentlyPlacingPart.transform.up = rh.normal;

                    if( Input.GetMouseButtonDown(0) )
                    {
                        placed = true;
                    }
                }
                else
                {
                    _currentlyPlacingPart.transform.position = mouseRay.GetPoint(_unattachedDistFromCamera);
                }
                yield return 0;
            }

            _currentlyPlacingPart.layer = LayerMask.NameToLayer("RobotAssembly");
            _currentlyPlacingPart.transform.parent = _core.transform;
            _currentlyPlacingPart = null;
        }
    }
}