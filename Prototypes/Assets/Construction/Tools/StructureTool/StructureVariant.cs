using UnityEngine;

using System;
using System.Linq;
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
        public VariantInfo Variation { get; private set; }
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
            initial.Variation = s_initialVariant;
            foreach (var variation in s_variations.Reverse())
            {
                var variant = new StructureVariant();
                variant.Prefab = initial.Prefab;
                variant.Rotation = variation.Rot;
                variation.SetConnectivity(initial, variant);
                variant.Variation = variation;
                yield return variant;
            }
        }

        public struct ConnectivityMapping
        {
            public StructureConnectivity From;
            public StructureConnectivity To;

            public override string ToString()
            {
                return String.Format("ConnectivityMapping[{0} => {1}]", From, To);
            }
        }

        public class VariantInfo
        {
            public string Name;
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

        private static readonly VariantInfo s_initialVariant = new VariantInfo()
        {
            Name = "Initial",
            Rot = Quaternion.identity,
            ConnectivityChanges = new ConnectivityMapping[0]
        };

        private static readonly VariantInfo[] s_variations = new VariantInfo[] 
        {
            // Right
            new VariantInfo()
            { 
                Name = "Right90",
                Rot = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up }
                }
            },
            new VariantInfo()
            { 
                Name = "Right180",
                Rot = Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward }
                }
            },
            new VariantInfo()
            { 
                Name = "Right270",
                Rot = Quaternion.Euler(new Vector3(270.0f, 0.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down }
                }
            },

            // Up
            new VariantInfo()
            { 
                Name = "Up90",
                Rot = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left }
                }
            },
            new VariantInfo()
            { 
                Name = "Up180",
                Rot = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward }
                }
            },
            new VariantInfo()
            { 
                Name = "Up270",
                Rot = Quaternion.Euler(new Vector3(0.0f, 270.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Right }
                }
            },

            
            // Forward
            new VariantInfo()
            { 
                Name = "Forward90",
                Rot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left }
                }
            },
            new VariantInfo()
            { 
                Name = "Forward180",
                Rot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 180.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down }
                }
            },
            new VariantInfo()
            { 
                Name = "Forward270",
                Rot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 270.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right }
                }
            },

            
            // Right x Up
            new VariantInfo()
            { 
                Name = "Right90Up90",
                Rot = Quaternion.Euler(new Vector3(90.0f, 90.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up }
                }
            },
            new VariantInfo()
            {
                Name = "Right90Up180",
                Rot = Quaternion.Euler(new Vector3(90.0f, 180.0f, 0.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up }
                }
            },
            new VariantInfo()
            { 
                Name = "Right180Up90",
                Rot = Quaternion.Euler(new Vector3(180.0f, 90.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Right }
                }
            },
            new VariantInfo()
            { 
                Name = "Right270Up90",
                Rot = Quaternion.Euler(new Vector3(270.0f, 90.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down }
                }
            },
            new VariantInfo()
            { 
                Name = "Right180Up180",
                Rot = Quaternion.Euler(new Vector3(180.0f, 180.0f, 0.0f)), 
                ConnectivityChanges = new ConnectivityMapping[] 
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward }
                }
            },

            new VariantInfo()
            {
                Name = "Right270Up180",
                Rot = Quaternion.Euler(new Vector3(270.0f, 180.0f, 0.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down }
                }
            },

            new VariantInfo()
            {
                Name = "Right90Up270",
                Rot = Quaternion.Euler(new Vector3(90.0f, 270.0f, 0.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up }
                }
            },

            new VariantInfo()
            {
                Name = "Right270Up270",
                Rot = Quaternion.Euler(new Vector3(270.0f, 270.0f, 0.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down }
                }
            },

            // Up x Forward
            new VariantInfo()
            {
                Name = "Up90Forward90",
                Rot = Quaternion.Euler(new Vector3(0.0f, 90.0f, 90.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left }
                }
            },

            new VariantInfo()
            {
                Name = "Up90Forward180",
                Rot = Quaternion.Euler(new Vector3(0.0f, 90.0f, 180.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left }
                }
            },

            new VariantInfo()
            {
                Name = "Up180Forward90",
                Rot = Quaternion.Euler(new Vector3(0.0f, 180.0f, 90.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up},
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left }
                }
            },
            
            new VariantInfo()
            {
                Name = "Up270Forward90",
                Rot = Quaternion.Euler(new Vector3(0.0f, 270.0f, 90.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Right }
                }
            },

            new VariantInfo()
            {
                Name = "Up270Forward180",
                Rot = Quaternion.Euler(new Vector3(0.0f, 270.0f, 180.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Right }
                }
            },

            new VariantInfo()
            {
                Name = "Up90Forward270",
                Rot = Quaternion.Euler(new Vector3(0.0f, 90.0f, 270.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Left }
                }
            },


            new VariantInfo()
            {
                Name = "Up180Forward270",
                Rot = Quaternion.Euler(new Vector3(0.0f, 180.0f, 270.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward }
                }
            },

            new VariantInfo()
            {
                Name = "Up270Forward90",
                Rot = Quaternion.Euler(new Vector3(0.0f, 270.0f, 90.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Right }
                }
            },

            // Right x Forward
            new VariantInfo()
            {
                Name = "Right90Forward90",
                Rot = Quaternion.Euler(new Vector3(90.0f, 0.0f, 90.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up }
                }
            },

            new VariantInfo()
            {
                Name = "Right90Forward180",
                Rot = Quaternion.Euler(new Vector3(90.0f, 0.0f, 180.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up }
                }
            },

            new VariantInfo()
            {
                Name = "Right90Forward270",
                Rot = Quaternion.Euler(new Vector3(90.0f, 0.0f, 270.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Up }
                }
            },
            
            new VariantInfo()
            {
                Name = "Right180Forward90",
                Rot = Quaternion.Euler(new Vector3(180.0f, 0.0f, 90.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Down },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Forward }
                }
            },

            new VariantInfo()
            {
                Name = "Right270Forward90",
                Rot = Quaternion.Euler(new Vector3(270, 0.0f, 90.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down }
                }
            },

            new VariantInfo()
            {
                Name = "Right270Forward180",
                Rot = Quaternion.Euler(new Vector3(270.0f, 0.0f, 180.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down }
                }
            },

            new VariantInfo()
            {
                Name = "Right270Forward270",
                Rot = Quaternion.Euler(new Vector3(270.0f, 0.0f, 270.0f)),
                ConnectivityChanges = new ConnectivityMapping[]
                {
                    new ConnectivityMapping() { From = StructureConnectivity.Up, To = StructureConnectivity.Right },
                    new ConnectivityMapping() { From = StructureConnectivity.Down, To = StructureConnectivity.Left },
                    new ConnectivityMapping() { From = StructureConnectivity.Left, To = StructureConnectivity.Backward },
                    new ConnectivityMapping() { From = StructureConnectivity.Right, To = StructureConnectivity.Forward },
                    new ConnectivityMapping() { From = StructureConnectivity.Forward, To = StructureConnectivity.Up },
                    new ConnectivityMapping() { From = StructureConnectivity.Backward, To = StructureConnectivity.Down }
                }
            },
        };
    }
}
