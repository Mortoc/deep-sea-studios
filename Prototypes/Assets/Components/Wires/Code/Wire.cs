using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.Procedural;

namespace DSS
{
    public class Wire : MonoBehaviour
    {
        [SerializeField]
        private Plug _in;
        public Plug In
        {
            get { return _in; }
        }

        [SerializeField]
        private Plug _out;
        public Plug Out
        {
            get { return _out; }
        }

        [SerializeField]
        private int _pathSegments = 24;
        [SerializeField]
        private int _shapeSegments = 8;
        [SerializeField]
        private float _stiffnessNearPlug = 0.25f;
        [SerializeField]
        private float _wireThickness = 0.01f;

        private Loft _loft;
        private Bezier _path;
        private Material _onMaterial;
        private Material _offMaterial;

        public void SetupWire(Plug inPlug, Plug outPlug, Material onMaterial, Material offMaterial)
        {
            _in = inPlug;
            _out = outPlug;
            _onMaterial = onMaterial;
            _offMaterial = offMaterial;

            transform.parent = _in.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            InitializeLoft();
            //StartCoroutine(AnimateOnOffTest()); // test
        }

        // Test Code
        /*
        private IEnumerator AnimateOnOffTest()
        {
            while (gameObject)
            {
                if (Rand.value > 0.5f) 
                    PowerOn();
                else 
                    PowerOff();

                yield return new WaitForSeconds(Rand.value);
            }
        }
        */
        //

        public void PowerOn()
        {
            GetComponent<Renderer>().material = _onMaterial;
        }

        private void PowerOff()
        {
            GetComponent<Renderer>().material = _offMaterial;
        }

        public void Update()
        {
            if (_in && _out)
            {
                UpdatePath();
            }
        }

        private void GetPlugPositions(out Vector3 start, out Vector3 startTan, out Vector3 endTan, out Vector3 end)
        {
            start = transform.InverseTransformPoint(_in.transform.position);
            end = transform.InverseTransformPoint(_out.transform.position);

            startTan = start + (transform.InverseTransformVector(_in.transform.up) * _stiffnessNearPlug);
            endTan = end + (transform.InverseTransformVector(_out.transform.up) * _stiffnessNearPlug);
        }

        private void InitializeLoft()
        {
            Vector3 start, startTan, endTan, end;
            GetPlugPositions(out start, out startTan, out endTan, out end);

            _path = new Bezier(new Bezier.ControlPoint[]{
                new Bezier.ControlPoint(start, start - startTan, startTan),
                new Bezier.ControlPoint(end, endTan, endTan - end)
            });

            _loft = new Loft(_path, Bezier.Circle(_wireThickness));

            var skin = gameObject.AddComponent<SkinnedMeshRenderer>();
            skin.material = _offMaterial;
            _loft.GenerateSkinnedMesh(_pathSegments, _shapeSegments, skin);
        }

        public void UpdatePath()
        {
            Vector3 start, startTan, endTan, end;
            GetPlugPositions(out start, out startTan, out endTan, out end);
            _path.ControlPoints[0].Point = start;
            _path.ControlPoints[0].InTangent = startTan - start;
            _path.ControlPoints[0].OutTangent = startTan;

            _path.ControlPoints[1].Point = end;
            _path.ControlPoints[1].InTangent = endTan;
            _path.ControlPoints[1].OutTangent = endTan;
        }
    }
}
