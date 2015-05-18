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
        public StructureVariant[] _prefabs;
        private Dictionary<int, StructureVariant> _allStructures;

        public IEnumerable<StructureVariant> AllStructures
        {
            get { return _allStructures.Values; }
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
