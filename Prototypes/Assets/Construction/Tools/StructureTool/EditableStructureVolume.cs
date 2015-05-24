using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public struct Vector3i
    {
        public static readonly Vector3i zero = new Vector3i(0, 0, 0);

        public static readonly Vector3i up = new Vector3i(0, 1, 0);
        public static readonly Vector3i down = new Vector3i(0, -1, 0);

        public static readonly Vector3i left = new Vector3i(-1, 0, 0);
        public static readonly Vector3i right = new Vector3i(1, 0, 0);

        public static readonly Vector3i forward = new Vector3i(0, 0, 1);
        public static readonly Vector3i backward = new Vector3i(0, 0, -1);


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

        public static Vector3i operator + (Vector3i a, Vector3i b)
        {
            return new Vector3i()
            {
                x = a.x + b.x,
                y = a.y + b.y,
                z = a.z + b.z
            };
        }
        public static Vector3i operator -(Vector3i a, Vector3i b)
        {
            return new Vector3i()
            {
                x = a.x - b.x,
                y = a.y - b.y,
                z = a.z - b.z
            };
        }
    }

    public class EditableStructureVolume : MonoBehaviour
    {
        public static readonly int MAX_WIDTH = 16;
        public static readonly int MAX_HEIGHT = 16;
        public static readonly int MAX_DEPTH = 16;

        private StructureTool _tool;
        private Bounds _tilesetSize;
        private GameObject _handleRoot;
        private GameObject _structureRoot;
        private int _editableStructureLayer;

        void Start()
        {
            _editableStructureLayer = LayerMask.NameToLayer("EditableStructure");
        }

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
                    if (variant.Prefab)
                    {
                        var structure = Instantiate<GameObject>(variant.Prefab);
                        structure.name += " " + variant.Variation;
                        structure.transform.parent = _structureRoot.transform;
                        structure.transform.localPosition = IToLocalPosition(i);
                        structure.transform.localRotation = variant.Rotation;
                        structure.transform.localScale = Vector3.one;
                        structure.layer = _editableStructureLayer;
                    }
                }
            }
        }

        private void GenerateHandles()
        {
            if (!_tool.VolumeIndicatorPrefab)
            {
                return;
            }

            if (_handleRoot)
            {
                GameObject.DestroyImmediate(_handleRoot);
            }
            _handleRoot = new GameObject("HandleRoot");
            _handleRoot.transform.parent = transform;
            _handleRoot.transform.localPosition = Vector3.zero;
            _handleRoot.transform.localRotation = Quaternion.identity;
            _handleRoot.transform.localScale = Vector3.one;

            var handlesPlaced = new Dictionary<int, GameObject>();
            Action<int> placeHandle = i =>
            {
                GameObject handleObj;
                if( !handlesPlaced.TryGetValue(i, out handleObj) )
                {
                    handleObj = Instantiate<GameObject>(_tool.VolumeIndicatorPrefab);
                    handleObj.name = String.Format("Handle {0}", IToXYZ(i));
                    handleObj.transform.parent = _handleRoot.transform;
                    handleObj.transform.localPosition = IToLocalPosition(i);
                    handleObj.transform.localRotation = Quaternion.identity;
                    handleObj.transform.localScale = _tilesetSize.size;
                    handleObj.GetComponent<EditableStructureHandle>().Init(this, i);
                    handlesPlaced[i] = handleObj;
                }
            };
            for(var i = 0; i < _structure.Length; ++i)
            {
                if( _structure[i] )
                {
                    var structurePos = IToXYZ(i);
                    var connectivity = GetNeighbors(i);
                    if (!connectivity[0] && structurePos.y + 1 < MAX_HEIGHT)
                    {
                        placeHandle(XYZToI(structurePos + Vector3i.up));
                    }
                    if( !connectivity[1] && structurePos.y > 0)
                    {
                        placeHandle(XYZToI(structurePos + Vector3i.down));
                    }

                    if (!connectivity[2] && structurePos.x > 0)
                    {
                        placeHandle(XYZToI(structurePos + Vector3i.left));
                    }

                    if (!connectivity[3] && structurePos.x + 1 < MAX_WIDTH)
                    {
                        placeHandle(XYZToI(structurePos + Vector3i.right));
                    }

                    if (!connectivity[4] && structurePos.z + 1 < MAX_DEPTH)
                    {
                        placeHandle(XYZToI(structurePos + Vector3i.forward));
                    }

                    if (!connectivity[5] && structurePos.z > 0)
                    {
                        placeHandle(XYZToI(structurePos + Vector3i.backward));
                    }
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
            var up = y + 1 < MAX_HEIGHT ? _structure[XYZToI(x, y + 1, z)] : false;
            var down = y > 0 ? _structure[XYZToI(x, y - 1, z)] : false;
            var left = x > 0 ? _structure[XYZToI(x - 1, y, z)] : false;
            var right = x + 1 < MAX_WIDTH ? _structure[XYZToI(x + 1, y, z)] : false;
            var forward = z + 1 < MAX_DEPTH ? _structure[XYZToI(x, y, z + 1)] : false;
            var backward = z > 0 ? _structure[XYZToI(x, y, z - 1)] : false;

            return new bool[] { up, down, left, right, forward, backward };
        }

        public void SetToDefault()
        {
            var defPos = new Vector3i(MAX_WIDTH / 2, MAX_HEIGHT / 2, MAX_DEPTH / 2);

            _structure[XYZToI(defPos)] = true;
            
            GenerateMesh();
            GenerateHandles();
        }

        public void AddStructure(int i)
        {
            _structure[i] = true;

            if (_regenerateAll != null)
            {
                StopCoroutine(_regenerateAll);
            }
            _regenerateAll = StartCoroutine(RegenerateAll());
        }

        private Coroutine _regenerateAll;

        private IEnumerator RegenerateAll()
        {
            yield return 0;
            GenerateMesh();

            yield return 0;
            GenerateHandles();
        }

        public void AddStructure(Vector3i vec)
        {
            AddStructure(XYZToI(vec));
        }

        public void AddStructure(int x, int y, int z)
        {
            AddStructure(XYZToI(x, y, z));
        }

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
