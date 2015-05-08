using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class EditableStructure : MonoBehaviour
    {
        public enum Connectivity
        {
            None = 0,
            All = 0xFFFF,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8,
            Forward = 16,
            Backward = 32
        }

        private int _connections;
    }
}
