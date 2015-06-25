using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public class StructureTool : ConstructionTool
    {
        private UndoBuffer _undoBuffer;
        public UndoBuffer CommandBuffer
        {
            get { return _undoBuffer; }
        }

        public GameObject LastBakedStructure { get; private set; }
        public StructureVariant[] _prefabs;
        public IEnumerable<StructureVariant> InitialVariants
        {
            get { return _prefabs; }
        }

        private Dictionary<int, StructureVariant> _allStructures;
        private EditableStructureVolume _editVolume;

        [SerializeField]
        private GameObject _volumeIndicatorPrefab;
        public GameObject VolumeIndicatorPrefab
        {
            get { return _volumeIndicatorPrefab; }
        }

        public IEnumerable<StructureVariant> AllStructures
        {
            get { return _allStructures.Values; }
        }

        private void Awake()
        {
            ToolMask = 1 << LayerMask.NameToLayer("EditableStructure");
        }

        private void CalculateAllPartRotations()
        {
            _allStructures = new Dictionary<int, StructureVariant>();
            for (var i = 0; i < _prefabs.Length; ++i)
            {
                var prefab = _prefabs[i];
                prefab.Rotation = Quaternion.identity;

                _allStructures[prefab.OrientationBitmask()] = prefab;

                foreach (var variant in StructureVariant.BuildAllVariants(prefab))
                {
                    _allStructures[variant.OrientationBitmask()] = variant;
                }
            }
        }

        public StructureVariant GetPrefabForOrientation(bool[] orientation, int index = 0)
        {
            if( index + 6 > orientation.Length )
            {
                throw new ArgumentException("Orientation requires 6 bools");
            }

            return GetPrefabForOrientation
            (
                orientation[index],
                orientation[index + 1],
                orientation[index + 2],
                orientation[index + 3],
                orientation[index + 4],
                orientation[index + 5]
            );
        }

        public StructureVariant GetPrefabForOrientation(bool up, bool down, bool left, bool right, bool forward, bool backward)
        {
            StructureVariant result;
            var orientation = StructureVariant.BitmaskForOrientation(up, down, left, right, forward, backward);
            if (!_allStructures.TryGetValue(orientation, out result))
            {
                throw new InvalidOperationException
                (
                    String.Format
                    (
                        "There isn't a prefab for the requested orientation {6} (Up={0}, Down={1}, Left={2}, Right={3}, Forward={4}, Backward={5})",
                        up, down, left, right, forward, backward, orientation
                    )
                );
            }
            return result;
        }

        public override void EnterState()
        {
            base.EnterState();

             _undoBuffer = new UndoBuffer();

            if (_allStructures == null || _allStructures.Count() == 0)
            {
                CalculateAllPartRotations();
            }

            if( !_editVolume )
            {
                var editVolumeObj = new GameObject("StructureVolume");

                var cameraRig = FindObjectOfType<CameraRig>();
                editVolumeObj.transform.position = cameraRig.Target.position;
                editVolumeObj.transform.rotation = Quaternion.identity;
                editVolumeObj.transform.localScale = Vector3.one;

                _editVolume = editVolumeObj.AddComponent<EditableStructureVolume>();
                _editVolume.Initialize(this);
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            LastBakedStructure = _editVolume.BakeToObject();
            LastBakedStructure.GetComponent<Rigidbody>().AddTorque(Rand.onUnitSphere, ForceMode.Impulse);

            GameObject.DestroyImmediate(_editVolume.gameObject);
        }

        public override void BackButtonAction()
        {
            base.BackButtonAction();

        }
    }
}
