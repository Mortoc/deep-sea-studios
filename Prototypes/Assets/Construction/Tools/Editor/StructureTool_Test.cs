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

        public StructureTool_Test()
        {
            var toolObj = new GameObject("structure tool unit test mock object");
            _tool = toolObj.AddComponent<StructureTool>();

            _tool._prefabs = new StructureTool.StructurePrefab[]
            {
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = false,
                    Right = true, Forward = false, Backward = false
                },
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = true,
                    Right = true, Forward = false, Backward = false
                },
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = true,
                    Right = true, Forward = true, Backward = false
                },
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = false, Down = false, Left = true,
                    Right = true, Forward = true, Backward = true
                },
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = true, Down = false, Left = true,
                    Right = true, Forward = true, Backward = true
                },
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = true, Down = false, Left = true,
                    Right = false, Forward = false, Backward = false
                },
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = true, Down = true, Left = true,
                    Right = true, Forward = true, Backward = true
                },
                new StructureTool.StructurePrefab()
                {
                    Prefab = null, Rotation = Quaternion.identity,
                    Up = true, Down = false, Left = true,
                    Right = true, Forward = true, Backward = false
                }
            };

            // generate all the rotation variants of the initial prefabs
            _tool.OnSelect(); 
        }

        public void Dispose()
        {
            GameObject.DestroyImmediate(_tool.gameObject);
        } 

        [Test]
        public void OrientationBitmaskUniquenessTest()
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

        [Theory]
        public void AllOrientationsAreFindableTest(bool[][] variantList)
        {
            Assume.That(variantList.All(list => list.Length == 6));


        }
    }
}