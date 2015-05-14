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
				return BitmaskForOrientation(Up, Down, Left, Right, Forward, Backward);
            }

			public static int BitmaskForOrientation(bool up, bool down, bool left, bool right, bool forward, bool backward)
			{
				return Convert.ToInt32(up) << 1 |
					   Convert.ToInt32(down) << 2 |
					   Convert.ToInt32(left) << 4 |
					   Convert.ToInt32(right) << 8 |
					   Convert.ToInt32(forward) << 16 |
					   Convert.ToInt32(backward) << 32;
			}

			public override string ToString ()
			{
				return String.Format
				(
					"[StructurePrefab(Up={0}, Down={1}, Left={2}, Right={3}, Forward={4}, Backward={5})]",
					Up, Down, Left, Right, Forward, Backward
				);
			}
        }

        public StructurePrefab[] _prefabs;

        private Dictionary<int, StructurePrefab> _allStructures;
        public IEnumerable<StructurePrefab> AllStructures
        {
            get { return _allStructures.Values; }
        }

        private void CalculateAllPartRotations()
        {
			_allStructures = new Dictionary<int, StructurePrefab>();
            for (var i = 0; i < _prefabs.Length; ++i)
            {
                var prefab = _prefabs[i];
                prefab.Rotation = Quaternion.identity;

				_allStructures[prefab.OrientationBitmask()] = prefab;

                foreach (var variant in BuildAllVariants(prefab))
                {
					if( !variant.Up && !variant.Down && !variant.Left && !variant.Right && variant.Forward && !variant.Backward )
						Debug.Log( variant );

					_allStructures[variant.OrientationBitmask()] = variant;
                }
            }
        }

		private delegate void StepRotation(ref bool up, ref bool down, ref bool left, ref bool right, 
		                                   ref bool forward, ref bool backward);

        private IEnumerable<StructurePrefab> BuildAllVariants(StructurePrefab prefab)
        {
			if( prefab.OrientationBitmask() == 0 )
				throw new ArgumentException("prefab has no orientation");

            StepRotation upStep = delegate(ref bool up, ref bool down, ref bool left, ref bool right, ref bool forward, ref bool backward)
            {
                var originalForward = forward;
                backward = left;
                right = backward;
                forward = right;
                left = originalForward;
            };

			StepRotation forwardStep = delegate(ref bool up, ref bool down, ref bool left, ref bool right, ref bool forward, ref bool backward)
            {
                var originalUp = up;
                up = right;
                right = down;
                down = left;
                left = originalUp;
            };

			StepRotation leftStep = delegate(ref bool up, ref bool down, ref bool left, ref bool right, ref bool forward, ref bool backward)
            {
                var originalUp = up;
                up = forward;
                forward = down;
                down = backward;
                backward = originalUp;
            };

            foreach (var variant in BuildVariants(prefab, s_upRotations, upStep))
            {
				if( variant.OrientationBitmask() == 0 ) throw new Exception("Generated a variant with no orientation");
	            
				yield return variant;
            }

            foreach (var variant in BuildVariants(prefab, s_forwardRotations, forwardStep))
			{
				if( variant.OrientationBitmask() == 0 ) throw new Exception("Generated a variant with no orientation");

				yield return variant;
            }

            foreach (var variant in BuildVariants(prefab, s_leftRotations, leftStep))
			{
				if( variant.OrientationBitmask() == 0 ) throw new Exception("Generated a variant with no orientation");

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

				connectivityRotFunc(ref up, ref down, ref left, ref right, ref forward, ref backward);

				variant.Up = up;
				variant.Down = down;
                variant.Left = left;
                variant.Right = right;
                variant.Forward = forward;
                variant.Backward = backward;

                yield return variant;
            }
        }

		public StructurePrefab GetPrefabForOrientation(bool up, bool down, bool left, bool right, bool forward, bool backward)
		{
			StructurePrefab result;
			var orientation = StructurePrefab.BitmaskForOrientation(up, down, left, right, forward, backward);
			if( !_allStructures.TryGetValue(orientation, out result) )
			{
				throw new InvalidOperationException
				(
					String.Format
					(
						"There isn't a prefab for the requested orientation (Up={0}, Down={1}, Left={2}, Right={3}, Forward={4}, Backward={5})",
						up, down, left, right, forward, backward
					)
				);
			}
			return result;
		}

        public override void OnSelect()
        {
            base.OnSelect();

            if (_allStructures == null || _allStructures.Count() == 0)
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
