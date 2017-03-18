using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SpectrumVisualiser
{

    [Serializable]
    public class AudioPlayerClip
    {

        public AudioClip AudioClip;
        public bool PlayOnAwake;
        public bool Loop;
        public double StartAfter;
        public AudioMixerGroup AudioMixerGroup;

    }
}