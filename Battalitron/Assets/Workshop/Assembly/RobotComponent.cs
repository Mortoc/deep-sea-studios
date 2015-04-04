using UnityEngine;

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class RobotComponent : MonoBehaviour
    {
        [SerializeField]
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        [SerializeField]
        private Joint _connectionJoint;
        public Joint ConnectionJoint
        {
            get { return _connectionJoint; }
        }

        private GameObject _hingeObj;
        private Type _storedHingeType;
        private Dictionary<string, object> _storedHinge = new Dictionary<string, object>();

        private void BackupHinge()
        {
            _storedHinge.Clear();
            _hingeObj = _connectionJoint.gameObject;
            _storedHingeType = _connectionJoint.GetType();
            foreach(var prop in _storedHingeType.GetProperties())
            {
                try
                {
                    if (prop.GetSetMethod() != null)
                    {
                        Debug.Log("Storing field " + prop.Name + " to " + prop.GetValue(_connectionJoint, null));
                        _storedHinge.Add(prop.Name, prop.GetValue(_connectionJoint, null));
                    }
                }
                catch (NotSupportedException) { } // cool, ignore that crap
            }

            Destroy(_connectionJoint);
            foreach(var rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = true;
            }
        }

        private IEnumerator RebuildHinge(Rigidbody onBody)
        {
            object joint = _hingeObj.AddComponent(_storedHingeType);
            yield return 0;

            var properties = _storedHingeType.GetProperties();
            for (var i = 0; i < properties.Length; ++i)
            {   
                try
                {
                    var prop = properties[i];

                    if (prop.GetSetMethod() != null)
                    {
                        Debug.Log("Setting field " + prop.Name + " to " + _storedHinge[prop.Name].ToString());
                        prop.SetValue(joint, _storedHinge[prop.Name], null);
                    }
                }
                catch (Exception) { } // ehhhh, we tried :(
            }

            foreach (var rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }

            _connectionJoint = (Joint)joint;
            _connectionJoint.connectedBody = onBody;
        }

        void Awake()
        {
            if (String.IsNullOrEmpty(_name)) throw new InvalidOperationException("No Name for " + gameObject.name);
            if (!_connectionJoint) throw new InvalidOperationException("No Joint Configured for " + gameObject.name);

            BackupHinge();
        }

        public void ObjectPlaced(Rigidbody onBody)
        {
            StartCoroutine(RebuildHinge(onBody));
        }
    }
}