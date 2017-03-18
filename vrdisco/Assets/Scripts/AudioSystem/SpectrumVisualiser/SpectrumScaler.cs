using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;

namespace SpectrumVisualiser
{
    public class SpectrumScaler : MonoBehaviour
    {
        [SerializeField]
        private int _freqBandIndex;
        [SerializeField]
        private bool _randomiseFreqBandIndex;

        [SerializeField] private bool _randomiseSizes;
        [SerializeField] private float _randLowEnd = 0.01f, _randHighEnd = 0.1f;
        [SerializeField] private float _minValue = .1f;
        [SerializeField] private float _multiplier = 1;

        private void Start()
        {
            if (_randomiseFreqBandIndex)
            {
                _freqBandIndex = Random.Range(0, AudioPlayer.Instance.FreqBandCount);
            }
        }

        // Update is called once per frame
        void Update()
        {
            float freqBandValue = MusicPlayer.Instance.BufferedFreqBandValue(_freqBandIndex);

            float randomMultiplier = 1;
            if (_randomiseSizes)
            {
                randomMultiplier += Random.Range(_randLowEnd, _randHighEnd) * (Random.Range(0, 1) < .5f ? -1 : 1);
            }
            transform.localScale = new Vector3(1, 1, 1) * (freqBandValue * _multiplier * randomMultiplier + _minValue);
        }
    }
}