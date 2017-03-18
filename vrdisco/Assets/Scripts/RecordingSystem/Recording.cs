using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecordingSystem
{
    [Serializable]
    public class Recording
    {
        public List<RecordingSample> Samples;
        /// <summary>
        /// Samples per second.
        /// </summary>
        public float SampleRate;
        public int MusicPieceId;
        public int TransformCount;

        public Recording(int transformCount, float sampleRate, int musicPieceId)
        {
            Samples = new List<RecordingSample>();
            SampleRate = sampleRate;
            MusicPieceId = musicPieceId;
            TransformCount = transformCount;
        }

        public override string ToString()
        {
            return "MusicPieceId: " + MusicPieceId + ", Transforms: " + TransformCount + ", SampleRate: " + SampleRate + ", Samples: " + Samples.Count;
        }
    }
}