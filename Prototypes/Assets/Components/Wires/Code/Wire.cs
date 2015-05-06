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
        private Material _onMaterial;

        [SerializeField]
        private Material _offMaterial;

        [SerializeField]
        private int _pathSegments = 32;
        [SerializeField]
        private int _shapeSegments = 8;
        [SerializeField]
        private float _stiffnessNearPlug = 0.25f;

        private Loft _loft;
        private Bezier _path;

        public void OnEnable()
        {
            transform.parent = _in.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            InitializeLoft();
        }

        public void OnDisable()
        {
            DestroyLoft();
        }

        public void Update()
        {
            UpdatePath();
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

            _loft = new Loft(_path, Bezier.Circle(0.025f));

            var skin = gameObject.AddComponent<MeshRenderer>();
            skin.sharedMaterial = _onMaterial;
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = _loft.GenerateMesh(_pathSegments, _shapeSegments);
        }

        private void DestroyLoft()
        {
            
            var skin = GetComponent<SkinnedMeshRenderer>();
            if( skin ) 
            {
                foreach(var bone in skin.bones )
                {
                    Destroy(bone.gameObject);
                }
                Destroy(skin);
            }
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

            var skin = gameObject.GetComponent<MeshRenderer>();
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = _loft.GenerateMesh(_pathSegments, _shapeSegments);
        }
    }
}
