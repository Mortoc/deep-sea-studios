using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public class StructureTool : ConstructionTool
    {
        private static readonly Quaternion[] s_upRotations = new Quaternion[] 
        {
            Quaternion.AngleAxis(90.0f, Vector3.up),
            Quaternion.AngleAxis(180.0f, Vector3.up),
            Quaternion.AngleAxis(270.0f, Vector3.up),
        };

        private static readonly Quaternion[] s_forwardRotations = new Quaternion[] 
        {
            Quaternion.AngleAxis(90.0f, Vector3.forward),
            Quaternion.AngleAxis(180.0f, Vector3.forward),
            Quaternion.AngleAxis(270.0f, Vector3.forward),
        };

        private static readonly Quaternion[] s_leftRotations = new Quaternion[] 
        {
            Quaternion.AngleAxis(90.0f, Vector3.left),
            Quaternion.AngleAxis(180.0f, Vector3.left),
            Quaternion.AngleAxis(270.0f, Vector3.left),
        };


        [Serializable]
        public struct StructurePrefab
        {
            public GameObject Prefab;

            public Quaternion Rotation { get; set; }

            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;
            public bool Forward;
            public bool Backward;

            public int OrientationBitmask()
            {
                return Convert.ToInt32(Up) << 1 |
                       Convert.ToInt32(Down) << 2 |
                       Convert.ToInt32(Left) << 4 |
                       Convert.ToInt32(Right) << 8 |
                       Convert.ToInt32(Forward) << 16 |
                       Convert.ToInt32(Backward) << 32;
            }
        }

        public StructurePrefab[] _prefabs;

        private StructurePrefab[] _allStructures;
        public IEnumerable<StructurePrefab> AllStructures
        {
            get { return _allStructures; }
        }

        private void CalculateAllPartRotations()
        {
            var allStructureOrientations = new Dictionary<int, StructurePrefab>();
            for (var i = 0; i < _prefabs.Length; ++i)
            {
                var prefab = _prefabs[i];
                prefab.Rotation = Quaternion.identity;

                allStructureOrientations[prefab.OrientationBitmask()] = prefab;

                foreach (var variant in BuildAllVariants(prefab))
                {
                    allStructureOrientations[variant.OrientationBitmask()] = variant;
                }
            }

            _allStructures = allStructureOrientations.Values.ToArray();
        }

        private delegate void StepRotation(ref bool left, ref bool right, ref bool up,
            ref bool down, ref bool forward, ref bool backward);

        private IEnumerable<StructurePrefab> BuildAllVariants(StructurePrefab prefab)
        {
            StepRotation upStep = delegate(ref bool left, ref bool right, ref bool up,
                ref bool down, ref bool forward, ref bool backward)
            {
                var originalForward = forward;
                backward = left;
                right = backward;
                forward = right;
                left = originalForward;
            };

            StepRotation forwardStep = delegate(ref bool left, ref bool right, ref bool up,
                ref bool down, ref bool forward, ref bool backward)
            {
                var originalUp = up;
                up = right;
                right = down;
                down = left;
                left = originalUp;
            };

            StepRotation leftStep = delegate(ref bool left, ref bool right, ref bool up,
                ref bool down, ref bool forward, ref bool backward)
            {
                var originalUp = up;
                up = forward;
                forward = down;
                down = backward;
                backward = originalUp;
            };

            foreach (var variant in BuildVariants(prefab, s_upRotations, upStep))
            {
                yield return variant;
            }

            foreach (var variant in BuildVariants(prefab, s_forwardRotations, forwardStep))
            {
                yield return variant;
            }

            foreach (var variant in BuildVariants(prefab, s_leftRotations, leftStep))
            {
                yield return variant;
            }
        }

        private IEnumerable<StructurePrefab> BuildVariants(StructurePrefab initial, Quaternion[] rotations, StepRotation connectivityRotFunc)
        {
            bool left = initial.Left;
            bool right = initial.Right;
            bool forward = initial.Forward;
            bool backward = initial.Backward;
            bool up = initial.Up;
            bool down = initial.Down;

            foreach (var rot in s_upRotations)
            {
                var variant = new StructurePrefab();
                variant.Prefab = initial.Prefab;
                variant.Rotation = rot;

                connectivityRotFunc(ref left, ref right, ref up, ref down, ref forward, ref backward);

                variant.Left = left;
                variant.Right = right;
                variant.Up = up;
                variant.Down = down;
                variant.Forward = forward;
                variant.Backward = backward;

                yield return variant;
            }
        }

        public override void OnSelect()
        {
            base.OnSelect();

            if (_allStructures == null || _allStructures.Length == 0)
            {
                CalculateAllPartRotations();
            }
        }

        public override void OnDeselect()
        {
            base.OnDeselect();
        }
    }
}
