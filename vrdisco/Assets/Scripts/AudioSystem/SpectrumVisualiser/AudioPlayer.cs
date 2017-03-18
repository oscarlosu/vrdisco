using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SpectrumVisualiser
{
    public class AudioPlayer : MonoBehaviour
    {

        public static AudioPlayer Instance { get; set; }

        public int SampleSize { get { return _sampleSize; } }
        public int FreqBandCount { get { return _freqBandCount; } }

        [SerializeField] private AudioPlayerClip[] _audioClips;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;

        private AudioSource[] _audioSources;

        [SerializeField]
        private int _sampleSize = 1024;
        private int _freqBandCount;
        private float[][] _samples;
        private float[][] _freqBand;
        private float[][] _bandBuffer;
        private float[][] _bufferDecrease;
        private float[][] _freqBandHighest;
        private float[][] _normalisedFreqBand;
        private float[][] _normalisedBandBuffer;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Calculate frequency band count.
            _freqBandCount = Mathf.FloorToInt(Mathf.Log(_sampleSize, 2)) - 1; // - Mathf.Log(1, 2)) <- this log returns 0, so don't spend time calculating it.
            Debug.Log("Frequency band count: " + _freqBandCount);

            // Setup audio sources and arrays.
            _audioSources = new AudioSource[_audioClips.Length];
            _samples = new float[_audioClips.Length][];
            _freqBand = new float[_audioClips.Length][];
            _bandBuffer = new float[_audioClips.Length][];
            _bufferDecrease = new float[_audioClips.Length][];
            _freqBandHighest = new float[_audioClips.Length][];
            _normalisedFreqBand = new float[_audioClips.Length][];
            _normalisedBandBuffer = new float[_audioClips.Length][];
            for (var i = 0; i < _audioClips.Length; i++)
            {
                // Setup audio sources.
                GameObject go = new GameObject(_audioClips[i].AudioClip.name);
                go.transform.parent = transform;
                _audioSources[i] = go.AddComponent<AudioSource>();
                _audioSources[i].clip = _audioClips[i].AudioClip;
                _audioSources[i].loop = _audioClips[i].Loop;
                _audioSources[i].outputAudioMixerGroup = _audioClips[i].AudioMixerGroup ?? _audioMixerGroup;

                // Setup sample arrays.
                _samples[i] = new float[_sampleSize];
                // Setup frequency arrays.
                _freqBand[i] = new float[_freqBandCount];
                // Setup band buffer arrays.
                _bandBuffer[i] = new float[_freqBandCount];
                // Setup buffer decrease arrays.
                _bufferDecrease[i] = new float[_freqBandCount];
                // Setup highest frequency arrays.
                _freqBandHighest[i] = new float[_freqBandCount];
                // Setup normalised frequency arrays.
                _normalisedFreqBand[i] = new float[_freqBandCount];
                // Setup normalised band buffer arrays.
                _normalisedBandBuffer[i] = new float[_freqBandCount];
            }
            
        }

        private void Start()
        {
            // Start playing all the tracks at the same time.
            for (var i = 0; i < _audioSources.Length; i++)
            {
                if (_audioClips[i].PlayOnAwake)
                {
                    _audioSources[i].PlayScheduled(AudioSettings.dspTime + 1 + _audioClips[i].StartAfter);
                }
            }
        }

        private void Update()
        {
            GetSamplesAndBands();
        }

        private void GetSamplesAndBands()
        {
            for (var i = 0; i < _audioSources.Length; i++)
            {
                _audioSources[i].GetSpectrumData(_samples[i], 0, FFTWindow.Blackman);
                CalculateFrequencyBands(i);
                CalculateBandBuffer(i);
                CalculateNormalisedValues(i);
            }
        }

        private void CalculateFrequencyBands(int clipIndex)
        {
            int count = 0;
            for (int i = 0; i < _freqBandCount; i++)
            {
                float average = 0;
                int sampleCount = (int)Mathf.Pow(2, i) * 2;
                for (int j = 0; j < sampleCount; j++)
                {
                    average += _samples[clipIndex][count] * (count + 1);
                    count++;
                }

                average /= count;
                _freqBand[clipIndex][i] = average * 10;
            }
        }

        private void CalculateBandBuffer(int clipIndex)
        {
            for (int i = 0; i < _freqBandCount; i++)
            {
                // If currently higher than the buffer, replace buffer.
                if (_freqBand[clipIndex][i] > _bandBuffer[clipIndex][i])
                {
                    _bandBuffer[clipIndex][i] = _freqBand[clipIndex][i];
                    _bufferDecrease[clipIndex][i] = 0.005f;
                }
                // Else decrease the band buffer.
                else if (_freqBand[clipIndex][i] < _bandBuffer[clipIndex][i])
                {
                    _bandBuffer[clipIndex][i] -= _bufferDecrease[clipIndex][i];
                    _bufferDecrease[clipIndex][i] *= 1.2f;

                }
            }
        }

        private void CalculateNormalisedValues(int clipIndex)
        {
            for (int i = 0; i < _freqBandCount; i++)
            {
                // Update highest if necessary.
                if (_freqBand[clipIndex][i] > _freqBandHighest[clipIndex][i])
                {
                    _freqBandHighest[clipIndex][i] = _freqBand[clipIndex][i];
                }
                _normalisedFreqBand[clipIndex][i] = _freqBand[clipIndex][i] / _freqBandHighest[clipIndex][i];
                _normalisedBandBuffer[clipIndex][i] = _bandBuffer[clipIndex][i] / _freqBandHighest[clipIndex][i];

            }
        }

        public float FreqBandValue(int clipIndex, int freqBandIndex)
        {
            return _freqBand[clipIndex][freqBandIndex];
        }

        public float BufferedFreqBandValue(int clipIndex, int freqBandIndex)
        {
            return _bandBuffer[clipIndex][freqBandIndex];
        }

        public float NormalisedBufferedFreqBandValue(int clipIndex, int freqBandIndex)
        {
            return _normalisedBandBuffer[clipIndex][freqBandIndex];
        }

        public float GetVolume(int clipIndex)
        {
            return GetAudioSourceRms(clipIndex);
        }

        private float GetAudioSourceRms(int clipIndex)
        {
            _audioSources[clipIndex].GetOutputData(_samples[clipIndex], 0); // fill array with samples
            float sum = 0;
            for (int i = 0; i < _sampleSize; i++)
            {
                sum += _samples[clipIndex][i] * _samples[clipIndex][i]; // sum squared samples
            }
            return Mathf.Sqrt(sum / _sampleSize); // rms = square root of average
        }
    }
}