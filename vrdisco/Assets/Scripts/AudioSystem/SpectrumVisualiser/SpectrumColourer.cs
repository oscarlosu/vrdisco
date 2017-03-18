using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;

namespace SpectrumVisualiser
{

    public class SpectrumColourer : MonoBehaviour
    {
        [SerializeField] private int _freqBandIndex;
        [SerializeField] private Gradient _colourGradient;
        [SerializeField] private Material _material;

        private void Awake()
        {
            _material = new Material(_material);
            GetComponent<Renderer>().material = _material;
        }

        private void Start()
        {
            _freqBandIndex = Random.Range(0, AudioPlayer.Instance.FreqBandCount);
        }

        // Update is called once per frame
        void Update()
        {
            float freqBandValue = MusicPlayer.Instance.BufferedFreqBandValue(_freqBandIndex);

            Color colour = _colourGradient.Evaluate(freqBandValue);

            _material.color = colour;
        }
    }
}