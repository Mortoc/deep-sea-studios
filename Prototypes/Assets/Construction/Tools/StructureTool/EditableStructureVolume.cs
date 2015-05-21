using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public struct Vector3i 
    {
        public int x;
        public int y;
        public int z;

        public Vector3i(int x_, int y_, int z_)
        {
            x = x_;
            y = y_;
            z = z_;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", x, y, z);
        }
    }

    public class EditableStructureVolume : MonoBehaviour
    {
        public static readonly int MAX_WIDTH = 16;
        public static readonly int MAX_HEIGHT = 16;
        public static readonly int MAX_DEPTH = 16;

        private StructureTool _tool;
        private Bounds _tilesetSize;
        private GameObject _indicatorRoot;
        private GameObject _structureRoot;

        [SerializeField]
        private bool[] _structure = new bool[MAX_WIDTH * MAX_HEIGHT * MAX_DEPTH];
        
        public static int XYZToI(Vector3i vec)
        {
            return XYZToI(vec.x, vec.y, vec.z);
        }

        public static int XYZToI(int x, int y, int z)
        {
            return (y * MAX_WIDTH * MAX_HEIGHT) + (x * MAX_WIDTH) + z;
        }

        public static Vector3i IToXYZ(int i)
        {
            var heightStep = MAX_WIDTH * MAX_HEIGHT;
            
            var y = i / heightStep;
            var iAfterHeight = i - (y * heightStep);
            var x = iAfterHeight / MAX_WIDTH;
            var z = iAfterHeight - (x * MAX_WIDTH);

            return new Vector3i(x, y, z);
        }

        public Vector3 XYZToLocalPosition(Vector3i vec)
        {
            return XYZToLocalPosition(vec.x, vec.y, vec.z);
        }

        public Vector3 XYZToLocalPosition(int x, int y, int z)
        {
            var position = new Vector3
            (
                _tilesetSize.size.x * MAX_WIDTH * -0.5f,
                _tilesetSize.size.y * 0.5f,
                _tilesetSize.size.z * MAX_DEPTH * -0.5f
            );

            position.x += _tilesetSize.size.x * (float)x;
            position.y += _tilesetSize.size.y * (float)y;
            position.z += _tilesetSize.size.z * (float)z;

            return position;
        }

        public Vector3 IToLocalPosition(int i)
        {
            return XYZToLocalPosition(IToXYZ(i));
        }

        private void GenerateMesh()
        {
            if( _structureRoot )
            {
                GameObject.DestroyImmediate(_structureRoot);
            }
            _structureRoot = new GameObject("StructureRoot");
            _structureRoot.transform.parent = transform;
            _structureRoot.transform.localPosition = Vector3.zero;
            _structureRoot.transform.localRotation = Quaternion.identity;
            _structureRoot.transform.localScale = Vector3.one;

            for(var i = 0; i < _structure.Length; ++i)
            {
                if( _structure[i] )
                {
                    var connectivity = GetNeighbors(i);
                    var variant = _tool.GetPrefabForOrientation(connectivity);
                    var structure = Instantiate<GameObject>(variant.Prefab);
                    structure.transform.parent = _structureRoot.transform;
                    structure.transform.localPosition = IToLocalPosition(i);
                    structure.transform.localRotation = variant.Rotation;
                    structure.transform.localScale = Vector3.one;
                    Debug.Log(variant, structure);
                }
            }
        }

        private bool[] GetNeighbors(int i)
        {
            return GetNeighbors(IToXYZ(i));
        }

        private bool[] GetNeighbors(Vector3i vec)
        {
            return GetNeighbors(vec.x, vec.y, vec.z);
        }

        private bool[] GetNeighbors(int x, int y, int z)
        {
            return new bool[6]
            {
                _structure[XYZToI(x, y + 1, z)], // up
                _structure[XYZToI(x, y - 1, z)], // down
                _structure[XYZToI(x - 1, y, z)], // left
                _structure[XYZToI(x + 1, y, z)], // right
                _structure[XYZToI(x, y, z + 1)], // forward
                _structure[XYZToI(x, y, z - 1)], // backward
            };
        }

        public void SetToDefault()
        {
            var defPos = new Vector3i(MAX_WIDTH / 2, MAX_HEIGHT / 2, MAX_DEPTH / 2);

            _structure[XYZToI(defPos)] = true;
            defPos.x += 1;
            _structure[XYZToI(defPos)] = true;
            defPos.x -= 2;
            _structure[XYZToI(defPos)] = true;

            GenerateMesh();
        }

        public void OnDisable()
        {
            GameObject.DestroyImmediate(_indicatorRoot);
        }

        //private GameObject BuildIndicator(int x, int y, int z)
        //{
        //    var indicator = Instantiate<GameObject>(_tool.VolumeIndicatorPrefab);
        //    indicator.name = String.Format("Indicator");
        //    indicator.transform.parent = _indicatorRoot.transform;
        //    indicator.transform.localPosition = position;
        //    indicator.transform.rotation = Quaternion.identity;
        //    indicator.transform.localScale = _tilesetSize.size;
        //}

        private void CalculateTilesetSize()
        {
            _tilesetSize = new Bounds();
            foreach (var variant in _tool.InitialVariants)
            {
                if (variant.Prefab)
                {
                    var prefab = variant.Prefab;
                    var meshFilter = prefab.GetComponent<MeshFilter>();
                    var bounds = meshFilter.sharedMesh.bounds;

                    _tilesetSize.Encapsulate(bounds);
                }
            }
        }

        public void Initialize(StructureTool tool)
        {
            _tool = tool;
            CalculateTilesetSize();
            SetToDefault();
        }
    }
}
