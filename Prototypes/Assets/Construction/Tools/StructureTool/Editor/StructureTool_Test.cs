using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using UnityEngine;
using UnityTest;

namespace DSS.Construction.UnitTests
{
    [TestFixture]
    [Category("Tool Tests")]
    public class StructureTool_Test : IDisposable
    {
        private StructureTool _tool;
        private GameObject _structurePrefabMock;

        public StructureTool_Test()
        {
            var toolObj = new GameObject("structure tool unit test mock object");
            _tool = toolObj.AddComponent<StructureTool>();
            _structurePrefabMock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var upObj = new GameObject(StructureConnectivity.Up.ToString());
            upObj.transform.parent = _structurePrefabMock.transform;
            upObj.transform.localPosition = Vector3.up;

            var downObj = new GameObject(StructureConnectivity.Down.ToString());
            downObj.transform.parent = _structurePrefabMock.transform;
            downObj.transform.localPosition = Vector3.down;

            var leftObj = new GameObject(StructureConnectivity.Left.ToString());
            leftObj.transform.parent = _structurePrefabMock.transform;
            leftObj.transform.localPosition = Vector3.left;

            var rightObj = new GameObject(StructureConnectivity.Right.ToString());
            rightObj.transform.parent = _structurePrefabMock.transform;
            rightObj.transform.localPosition = Vector3.right;

            var forwardObj = new GameObject(StructureConnectivity.Forward.ToString());
            forwardObj.transform.parent = _structurePrefabMock.transform;
            forwardObj.transform.localPosition = Vector3.forward;

            var backwardObj = new GameObject(StructureConnectivity.Backward.ToString());
            backwardObj.transform.parent = _structurePrefabMock.transform;
            backwardObj.transform.localPosition = Vector3.back;

            _tool._prefabs = new StructureVariant[]
            {
                // Endcap
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = false,
                    Right = true, Forward = false, Backward = false
                },

                // Thru Section
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = true,
                    Right = true, Forward = false, Backward = false
                },

                // T Intersection
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = true,
                    Right = true, Forward = true, Backward = false
                },

                // 4-way intersection
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = true,
                    Right = true, Forward = true, Backward = true
                },

                // 5-way intersection
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = true, Down = false, Left = true,
                    Right = true, Forward = true, Backward = true
                },

                // L-corner
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = true, Down = false, Left = true,
                    Right = false, Forward = false, Backward = false
                },

                // 6-way intersection
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = true, Down = true, Left = true,
                    Right = true, Forward = true, Backward = true
                },

                // TT intersection
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = true, Down = false, Left = true,
                    Right = true, Forward = true, Backward = false
                },

                // Tri-corner intersection
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = true, Down = false, Left = true,
                    Right = false, Forward = true, Backward = false
                },
                

                // Block
                new StructureVariant()
                {
                    Prefab = _structurePrefabMock, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = false,
                    Right = false, Forward = false, Backward = false
                }
            };

            // generate all the rotation variants of the initial prefabs
            _tool.OnSelect();
        }

        public void Dispose()
        {
            _tool.OnDeselect();
            GameObject.DestroyImmediate(_tool.gameObject);
            GameObject.DestroyImmediate(_structurePrefabMock);
        }

        [Test]
        public void orientation_bitmasks_are_unique()
        {
            Assert.AreEqual
            (
                _tool.AllStructures.Count(),
                _tool.AllStructures.Select(s => s.OrientationBitmask()).Distinct().Count()
            );
        }

        [Datapoint]
        public bool[][] _allEndcapVariants = new bool[][]
        {
            new bool[] { true, false, false, false, false, false },
            new bool[] { false, true, false, false, false, false },
            new bool[] { false, false, true, false, false, false },
            new bool[] { false, false, false, true, false, false },
            new bool[] { false, false, false, false, true, false },
            new bool[] { false, false, false, false, false, true }
        };

        [Datapoint]
        public bool[][] _allThruVariants = new bool[][]
		{
			new bool[] { true, true, false, false, false, false },
			new bool[] { false, false, true, true, false, false },
			new bool[] { false, false, false, false, true, true }
		};

        [Datapoint]
        public bool[][] _allTIntersections = new bool[][]
		{
			new bool[] { true, true, true, false, false, false },
			new bool[] { true, true, false, true, false, false },
			new bool[] { true, true, false, false, true, false },
			new bool[] { true, true, false, false, false, true },

			new bool[] { true, false, true, true, false, false },
			new bool[] { false, true, true, true, false, false },
			new bool[] { false, false, true, true, true, false },
			new bool[] { false, false, true, true, false, true },
			
			new bool[] { true, false, false, false, true, true },
			new bool[] { false, true, false, false, true, true },
			new bool[] { false, false, true, false, true, true },
			new bool[] { false, false, false, true, true, true }
		};

        [Datapoint]
        public bool[][] _all4WayIntersections = new bool[][]
		{
			new bool[] { true, true, true, true, false, false },
			new bool[] { true, true, false, false, true, true },
			new bool[] { false, false, true, true, true, true }
		};

        [Datapoint]
        public bool[][] _all5WayIntersections = new bool[][]
		{
			new bool[] { false, true, true, true, true, true },
            new bool[] { true, false, true, true, true, true },
            new bool[] { true, true, false, true, true, true },
            new bool[] { true, true, true, false, true, true },
            new bool[] { true, true, true, true, false, true },
            new bool[] { true, true, true, true, true, false }
		};

        [Datapoint]
        public bool[][] _all6WayIntersections = new bool[][]
		{
			new bool[] { true, true, true, true, true, true }
		};

        [Datapoint]
        public bool[][] _allLIntersections = new bool[][]
	    {
		    new bool[] { true, false, true, false, false, false },
		    new bool[] { true, false, false, true, false, false },
		    new bool[] { true, false, false, false, true, false },
		    new bool[] { true, false, false, false, false, true },
            
		    new bool[] { false, true, true, false, false, false },
		    new bool[] { false, true, false, true, false, false },
		    new bool[] { false, true, false, false, true, false },
		    new bool[] { false, true, false, false, false, true },

            new bool[] { false, false, false, true, true, false }
	    };

        [Datapoint]
        public bool[][] _allTTIntersections = new bool[][]
	    {
            new bool[] { true, true, true, true, false, false },
            new bool[] { true, true, true, false, true, false },
            new bool[] { true, true, false, true, true, false },
            new bool[] { true, false, true, true, true, false },
            new bool[] { false, true, true, true, true, false },

		    new bool[] { true, true, true, false, false, true },
		    new bool[] { true, true, false, true, false, true },
		    new bool[] { true, false, true, true, false, true },
		    new bool[] { false, true, true, true, false, true },

            new bool[] { true, true, false, false, true, true },
            new bool[] { true, false, true, false, true, true },
            new bool[] { false, true, true, false, true, true },

            new bool[] { true, true, false, true, true, false },
            new bool[] { true, false, false, true, true, true },
            new bool[] { false, true, false, true, true, true },

            new bool[] { true, false, true, true, true, false },
            new bool[] { false, false, true, true, true, true },
            
            new bool[] { false, true, true, true, true, false },
	    };

        [Datapoint]
        public bool[][] _allTriCornerIntersections = new bool[][]
	    {
            new bool[] { true, false, false, true, true, false },
            new bool[] { true, false, true, false, true, false },
            new bool[] { true, false, true, false, false, true },
            new bool[] { true, false, false, true, false, true },
            
            new bool[] { false, true, false, true, true, false },
            new bool[] { false, true, true, false, true, false },
            new bool[] { false, true, true, false, false, true },
            new bool[] { false, true, false, true, false, true }
	    };

        [Theory]
        public void all_orientations_are_findable(bool[][] variantList)
        {
            Assume.That(variantList.Length > 0);
            Assume.That(variantList.All(list => list.Length == 6));

            foreach (var variant in variantList)
            {
                var prefab = _tool.GetPrefabForOrientation
                (
                    variant[0], variant[1], variant[2], variant[3], variant[4], variant[5]
                );

                Assert.That(prefab != null);
            }
        }

        public static Vector3 PositionForConnectivity(StructureConnectivity dir)
        {
            switch (dir)
            {
                case StructureConnectivity.Up:
                    return Vector3.up;

                case StructureConnectivity.Down:
                    return Vector3.down;

                case StructureConnectivity.Left:
                    return Vector3.left;

                case StructureConnectivity.Right:
                    return Vector3.right;

                case StructureConnectivity.Forward:
                    return Vector3.forward;

                case StructureConnectivity.Backward:
                    return Vector3.back;

                default:
                    throw new NotImplementedException();
            }

        }

        public static bool IsNear(Vector3 position, StructureConnectivity structureDir, float epsilonSqr = 0.0001f)
        {
            return (PositionForConnectivity(structureDir) - position).sqrMagnitude < epsilonSqr;
        }

        private GameObject GetPrefabChild(GameObject prefab, StructureConnectivity childDir)
        {
            var childName = childDir.ToString();
            foreach(Transform child in prefab.transform)
            {
                if( child.gameObject.name == childName )
                {
                    return child.gameObject;
                }
            }
            throw new Exception(String.Format("Cannot find child {0} in prefab {1}", childName, prefab.name));
        }

        [Test]
        public void all_orientations_rotate_correctly()
        {
            foreach (var variant in _tool.AllStructures)
            {
                variant.Prefab.transform.rotation = variant.Rotation;

                foreach(var connectionMapping in variant.Variation.ConnectivityChanges)
                {
                    try
                    {
                        var child = GetPrefabChild(variant.Prefab, connectionMapping.From);
                        Assert.IsTrue
                        (
                            IsNear(child.transform.position, connectionMapping.To),
                            String.Format
                            (
                                "{0} {1} position expected: {2}, actual: {3}",
                                variant.Variation.Name,
                                child.name,
                                PositionForConnectivity(connectionMapping.To),
                                child.transform.position
                            )
                        );
                    }
                    catch(Exception e)
                    {
                        //var failure = GameObject.Instantiate<GameObject>(variant.Prefab);
                        //failure.name = String.Format("FailureCase: {0}", variant.Variation.Name);
                        //failure.transform.rotation = variant.Rotation;
                        throw e;
                    }
                    
                }
            }
        }
    }
}