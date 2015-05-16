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
		enum Connectivity 
		{
			Up = 1,
			Down = 2,
			Left = 4,
			Right = 8,
			Forward = 16,
			Backward = 32
		};

		private struct ConnectivityMapping
		{
			public Connectivity From;
			public Connectivity To;
		}

		private struct RotationVariant
		{
			public Quaternion Rot;
			public ConnectivityMapping[] ConnectivityChanges;

			public void SetConnectivity(StructurePrefab src, StructurePrefab target)
			{

			}
		}

		private static readonly RotationVariant[] s_variations = new Quaternion[] 
        {
			// Up Axis Rotations
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(90.0f, Vector3.up), 
				ConnectivityChanges = new ConnectivityMapping[] 
				{
					new ConnectivityMapping() { From = Connectivity.Forward, To = Connectivity.Left },
					new ConnectivityMapping() { From = Connectivity.Left, To = Connectivity.Backward },
					new ConnectivityMapping() { From = Connectivity.Backward, To = Connectivity.Right },
					new ConnectivityMapping() { From = Connectivity.Right, To = Connectivity.Forward }
				}
			},
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(180.0f, Vector3.up), 
				ConnectivityChanges = new ConnectivityMapping[] 
				{
					new ConnectivityMapping() { From = Connectivity.Forward, To = Connectivity.Backward },
					new ConnectivityMapping() { From = Connectivity.Left, To = Connectivity.Right },
					new ConnectivityMapping() { From = Connectivity.Backward, To = Connectivity.Forward },
					new ConnectivityMapping() { From = Connectivity.Right, To = Connectivity.Left }
				}
			},
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(270.0f, Vector3.up), 
				ConnectivityChanges = new ConnectivityMapping[] 
				{
					new ConnectivityMapping() { From = Connectivity.Forward, To = Connectivity.Right },
					new ConnectivityMapping() { From = Connectivity.Left, To = Connectivity.Forward },
					new ConnectivityMapping() { From = Connectivity.Backward, To = Connectivity.Left },
					new ConnectivityMapping() { From = Connectivity.Right, To = Connectivity.Backward }
				}
			},

			// Forward Axis Rotations
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(90.0f, Vector3.forward), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = Connectivity.Right, To = Connectivity.Up },
					new ConnectivityMapping() { From = Connectivity.Up, To = Connectivity.Left },
					new ConnectivityMapping() { From = Connectivity.Left, To = Connectivity.Down },
					new ConnectivityMapping() { From = Connectivity.Down, To = Connectivity.Right }
				}
			},
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(180.0f, Vector3.forward), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = Connectivity.Right, To = Connectivity.Left },
					new ConnectivityMapping() { From = Connectivity.Up, To = Connectivity.Down },
					new ConnectivityMapping() { From = Connectivity.Left, To = Connectivity.Right },
					new ConnectivityMapping() { From = Connectivity.Down, To = Connectivity.Up }
				}
			},
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(270.0f, Vector3.forward), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = Connectivity.Right, To = Connectivity.Down },
					new ConnectivityMapping() { From = Connectivity.Up, To = Connectivity.Right },
					new ConnectivityMapping() { From = Connectivity.Left, To = Connectivity.Up },
					new ConnectivityMapping() { From = Connectivity.Down, To = Connectivity.Left }
				}
			},

			// Left Axis Rotations
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(90.0f, Vector3.left), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = Connectivity.Forward, To = Connectivity.Up },
					new ConnectivityMapping() { From = Connectivity.Up, To = Connectivity.Backward },
					new ConnectivityMapping() { From = Connectivity.Backward, To = Connectivity.Down },
					new ConnectivityMapping() { From = Connectivity.Down, To = Connectivity.Forward }
				}
			},
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(180.0f, Vector3.left), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = Connectivity.Forward, To = Connectivity.Backward },
					new ConnectivityMapping() { From = Connectivity.Up, To = Connectivity.Down },
					new ConnectivityMapping() { From = Connectivity.Backward, To = Connectivity.Forward },
					new ConnectivityMapping() { From = Connectivity.Down, To = Connectivity.Up }
				}
			},
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(270.0f, Vector3.left), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = Connectivity.Forward, To = Connectivity.Down },
					new ConnectivityMapping() { From = Connectivity.Up, To = Connectivity.Forward },
					new ConnectivityMapping() { From = Connectivity.Backward, To = Connectivity.Up },
					new ConnectivityMapping() { From = Connectivity.Down, To = Connectivity.Backward }
				}
			}
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
				return Convert.ToInt32(up) << Connectivity.Up |
						Convert.ToInt32(down) << Connectivity.Down |
						Convert.ToInt32(left) << Connectivity.Left |
					    Convert.ToInt32(right) << Connectivity.Right |
					    Convert.ToInt32(forward) << Connectivity.Forward |
					    Convert.ToInt32(backward) << Connectivity.Backward;
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

				foreach (var variant in BuildVariants(prefab))
				{
					_allStructures[variant.OrientationBitmask()] = variant;
                }
            }
        }

        private IEnumerable<StructurePrefab> BuildVariants(StructurePrefab initial)
        {
            foreach (var variation in s_variations)
            {
                var variant = new StructurePrefab();
                variant.Prefab = initial.Prefab;
                variant.Rotation = variation.Rot;
				variation.SetConnectivity(initial, variant);
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
