using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace MusicSystem
{
    public class MusicPlayer : MonoBehaviour
    {

        public static MusicPlayer Instance { get; private set; }

        public int SampleSize
        {
            get { return _sampleSize; }
        }

        public int FreqBandCount
        {
            get { return _freqBandCount; }
        }

        public MusicSystemSetup.MusicPiece MusicPiece { get; private set; }

        [SerializeField] private AudioMixerGroup _audioMixerGroup;

        private GameObject _audioSourceObject;
        private AudioSource _audioSource;
        
        [SerializeField] private int _sampleSize = 1024;
        private int _freqBandCount;
        private float[] _samples;
        private float[] _freqBand;
        private float[] _bandBuffer;
        private float[] _bufferDecrease;
        private float[] _freqBandHighest;
        private float[] _normalisedFreqBand;
        private float[] _normalisedBandBuffer;

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

            // Setup sample arrays.
            _samples = new float[_sampleSize];
            // Setup frequency arrays.
            _freqBand = new float[_freqBandCount];
            // Setup band buffer arrays.
            _bandBuffer = new float[_freqBandCount];
            // Setup buffer decrease arrays.
            _bufferDecrease = new float[_freqBandCount];
            // Setup highest frequency arrays.
            _freqBandHighest = new float[_freqBandCount];
            // Setup normalised frequency arrays.
            _normalisedFreqBand = new float[_freqBandCount];
            // Setup normalised band buffer arrays.
            _normalisedBandBuffer = new float[_freqBandCount];
        }

        private void Update()
        {
            if (_audioSource != null && _audioSource.isPlaying)
            {
                GetSamplesAndBands();
            }
        }

        public void Play(MusicSystemSetup.MusicPiece musicPiece)
        {
            SetupPlayer(musicPiece);
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        private void SetupPlayer(MusicSystemSetup.MusicPiece musicPiece)
        {
            // Destroy previous audio source.
            Destroy(_audioSourceObject);

            // Save the audio clip.
            MusicPiece = musicPiece;

            // ----------------------------

            // Setup audio sources.
            _audioSourceObject = new GameObject("Music-" + MusicPiece.AudioClip.name);
            _audioSourceObject.transform.parent = transform;
            _audioSource = _audioSourceObject.AddComponent<AudioSource>();
            _audioSource.clip = MusicPiece.AudioClip;
            _audioSource.loop = false;
            _audioSource.outputAudioMixerGroup = _audioMixerGroup;
        }

        private void GetSamplesAndBands()
        {
            _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
            CalculateFrequencyBands();
            CalculateBandBuffer();
            CalculateNormalisedValues();
        }

        private void CalculateFrequencyBands()
        {
            int count = 0;
            for (int i = 0; i < _freqBandCount; i++)
            {
                float average = 0;
                int sampleCount = (int) Mathf.Pow(2, i) * 2;
                for (int j = 0; j < sampleCount; j++)
                {
                    average += _samples[count] * (count + 1);
                    count++;
                }

                average /= count;
                _freqBand[i] = average * 10;
            }
        }

        private void CalculateBandBuffer()
        {
            for (int i = 0; i < _freqBandCount; i++)
            {
                // If currently higher than the buffer, replace buffer.
                if (_freqBand[i] > _bandBuffer[i])
                {
                    _bandBuffer[i] = _freqBand[i];
                    _bufferDecrease[i] = 0.005f;
                }
                // Else decrease the band buffer.
                else if (_freqBand[i] < _bandBuffer[i])
                {
                    _bandBuffer[i] -= _bufferDecrease[i];
                    _bufferDecrease[i] *= 1.2f;

                }
            }
        }

        private void CalculateNormalisedValues()
        {
            for (int i = 0; i < _freqBandCount; i++)
            {
                // Update highest if necessary.
                if (_freqBand[i] > _freqBandHighest[i])
                {
                    _freqBandHighest[i] = _freqBand[i];
                }
                _normalisedFreqBand[i] = _freqBand[i] / _freqBandHighest[i];
                _normalisedBandBuffer[i] = _bandBuffer[i] / _freqBandHighest[i];

            }
        }

        public float FreqBandValue(int freqBandIndex)
        {
            return _freqBand[freqBandIndex];
        }

        public float BufferedFreqBandValue(int freqBandIndex)
        {
            return _bandBuffer[freqBandIndex];
        }

        public float NormalisedBufferedFreqBandValue(int freqBandIndex)
        {
            return _normalisedBandBuffer[freqBandIndex];
        }

        public float GetVolume()
        {
            return GetAudioSourceRms();
        }

        private float GetAudioSourceRms()
        {
            _audioSource.GetOutputData(_samples, 0); // fill array with samples
            float sum = 0;
            for (int i = 0; i < _sampleSize; i++)
            {
                sum += _samples[i] * _samples[i]; // sum squared samples
            }
            return Mathf.Sqrt(sum / _sampleSize); // rms = square root of average
        }
    }
}