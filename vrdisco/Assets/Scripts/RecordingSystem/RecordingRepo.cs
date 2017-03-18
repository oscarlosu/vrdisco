using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordingSystem
{
    [Serializable]
    public class RecordingRepo
    {
        public List<Recording> Recordings;

        public RecordingRepo()
        {
            Recordings = new List<Recording>();
        }
    }
}