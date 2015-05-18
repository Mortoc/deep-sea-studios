using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.Construction
{
    public enum StructureConnectivity
    {
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        Forward = 16,
        Backward = 32
    };

    [Serializable]
    public class StructureVariant
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
            return Convert.ToInt32(up) << (int)StructureConnectivity.Up |
                    Convert.ToInt32(down) << (int)StructureConnectivity.Down |
                    Convert.ToInt32(left) << (int)StructureConnectivity.Left |
                    Convert.ToInt32(right) << (int)StructureConnectivity.Right |
                    Convert.ToInt32(forward) << (int)StructureConnectivity.Forward |
                    Convert.ToInt32(backward) << (int)StructureConnectivity.Backward;
        }

        public override string ToString()
        {
            return String.Format
            (
                "[StructurePrefab(Up={0}, Down={1}, Left={2}, Right={3}, Forward={4}, Backward={5})]",
                Up, Down, Left, Right, Forward, Backward
            );
        }

        public static IEnumerable<StructureVariant> BuildAllVariants(StructureVariant initial)
        {
            foreach (var variation in s_variations)
            {
                var variant = new StructureVariant();
                variant.Prefab = initial.Prefab;
                variant.Rotation = variation.Rot;
                variation.SetConnectivity(initial, variant);
                yield return variant;
            }
        }

        private struct ConnectivityMapping
        {
            public StructureConnectivity From;
            public StructureConnectivity To;

            public override string ToString()
            {
                return String.Format("ConnectivityMapping[{0} => {1}]", From, To);
            }
        }

        private struct RotationVariant
        {
            public Quaternion Rot;
            public ConnectivityMapping[] ConnectivityChanges;

            private static bool GetConnectivity(StructureConnectivity c, StructureVariant sp)
            {
                switch (c)
                {
                    case StructureConnectivity.Up:
                        return sp.Up;
                    case StructureConnectivity.Down:
                        return sp.Down;
                    case StructureConnectivity.Left:
                        return sp.Left;
                    case StructureConnectivity.Right:
                        return sp.Right;
                    case StructureConnectivity.Forward:
                        return sp.Forward;
                    case StructureConnectivity.Backward:
                        return sp.Backward;
                    default:
                        throw new NotImplementedException();
                }
            }

            private static void SetConnectivity(StructureConnectivity c, StructureVariant sp, bool value)
            {
                switch (c)
                {
                    case StructureConnectivity.Up:
                        sp.Up = value;
                        break;
                    case StructureConnectivity.Down:
                        sp.Down = value;
                        break;
                    case StructureConnectivity.Left:
                        sp.Left = value;
                        break;
                    case StructureConnectivity.Right:
                        sp.Right = value;
                        break;
                    case StructureConnectivity.Forward:
                        sp.Forward = value;
                        break;
                    case StructureConnectivity.Backward:
                        sp.Backward = value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            public void SetConnectivity(StructureVariant src, StructureVariant target)
            {
                target.Up = src.Up;
                target.Down = src.Down;
                target.Left = src.Left;
                target.Right = src.Right;
                target.Forward = src.Forward;
                target.Backward = src.Backward;

                foreach (var change in ConnectivityChanges)
                {
                    bool from = GetConnectivity(change.From, src);
                    SetConnectivity(change.To, target, from);
                }
            }
        }

        private static readonly RotationVariant[] s_variations = new RotationVariant[] 
        {
            // Up Axis Rotations
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(90.0f, Vector3.up), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(180.0f, Vector3.up), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(270.0f, Vector3.up), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward }
                }
            },

            // Forward Axis Rotations
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(90.0f, Vector3.forward), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(180.0f, Vector3.forward), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(270.0f, Vector3.forward), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left }
                }
            },

            // Left Axis Rotations
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(90.0f, Vector3.left), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(180.0f, Vector3.left), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(270.0f, Vector3.left), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Backward }
                }
            },

            // Forward then Up Rotations
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(90.0f, Vector3.forward) * Quaternion.AngleAxis(90.0f, Vector3.up), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
					new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
					new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
					new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward },                    
					new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Left },
					new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Right }
				}
			},
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(90.0f, Vector3.forward) * Quaternion.AngleAxis(180.0f, Vector3.up), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left },                    
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(90.0f, Vector3.forward) * Quaternion.AngleAxis(270.0f, Vector3.up), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left }
                }
            },
            

            // Up then Forward Rotations
			new RotationVariant()
			{ 
				Rot = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, Vector3.forward), 
				ConnectivityChanges = new ConnectivityMapping[]
				{
					new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
					new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
					new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
					new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up },                    
					new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left },
					new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right }
				}
			},
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
					new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
					new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
					new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
					new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left },                    
					new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
					new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up }
                }
            },
            new RotationVariant()
            { 
                Rot = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(270.0f, Vector3.forward), 
                ConnectivityChanges = new ConnectivityMapping[]
                {
					new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
					new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
					new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
					new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down },                    
					new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
					new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left }
                }
            },
        };
    }


}
