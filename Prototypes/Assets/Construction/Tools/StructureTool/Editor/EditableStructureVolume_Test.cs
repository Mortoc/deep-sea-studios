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
    public class EditableStructureVolume_Test : IDisposable
    {
        private EditableStructureVolume _volume;

        public EditableStructureVolume_Test()
        {
            var obj = new GameObject("Editable Structure Volume Test Obj");
            _volume = obj.AddComponent<EditableStructureVolume>();
        }

        public void Dispose()
        {
            GameObject.DestroyImmediate(_volume.gameObject);
        }
                
        [Test]
        public void IToXYZ_and_XYZToI_are_idempotent_and_inverse()
        {
            var max = (EditableStructureVolume.MAX_WIDTH *
                       EditableStructureVolume.MAX_HEIGHT *
                       EditableStructureVolume.MAX_DEPTH) - 1;

            for(var i = 0; i < max; ++i)
            {
                var vect = EditableStructureVolume.IToXYZ(i);
                var result = EditableStructureVolume.XYZToI(vect);
                Assert.AreEqual(i, result);
                var vect2 = EditableStructureVolume.IToXYZ(result);
                Assert.AreEqual(vect.x, vect2.x);
                Assert.AreEqual(vect.y, vect2.y);
                Assert.AreEqual(vect.z, vect2.z);
            }
        }
    }
}