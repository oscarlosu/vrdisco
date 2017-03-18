using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RecordingSystem
{
    [Serializable]
    public class RecordingSample
    {
        public Vector3[] Positions;
        public Vector3[] Rotations;

        public RecordingSample(int transformCount)
        {
            Positions = new Vector3[transformCount];
            Rotations = new Vector3[transformCount];
        }

        public override string ToString()
        {
            return "P: " + string.Join(", ", Positions.Select(p => p.ToString()).ToArray()) + "\nR: " + string.Join(", ", Rotations.Select(r => r.ToString()).ToArray());
        }
    }
}