using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;

namespace RecordingSystem
{
    public class Recorder : MonoBehaviour
    {
        public bool IsRecording { get { return _isRecording; } }
        public Recording LastRecording { get { return _recording; } }

        [SerializeField]
        private Transform[] _transforms;

        [SerializeField]
        private float _sampleRate = 1;

        private bool _isRecording;
        private Recording _recording;
        
        private float _timeSinceLastSample;
        private float _timeBetweenSamples;

        [ContextMenu("Start recording")]
        public void StartRecording()
        {
            if (MusicPlayer.Instance == null || MusicPlayer.Instance.MusicPiece == null)
            {
                Debug.LogError("Can't record the sweet moves, if there is no music playing...");
            }

            // Create new recording.
            _recording = new Recording(_transforms.Length, _sampleRate, MusicPlayer.Instance.MusicPiece.Id);

            // Calculate time between samples.
            _timeBetweenSamples = 1 / _recording.SampleRate;

            // Start recording.
            _isRecording = true;
            StartCoroutine(TakeSample());

            Debug.Log("Recorder is now recording!");
        }

        [ContextMenu("Stop recording")]
        public void StopRecording()
        {
            // Stop recording.
            StopCoroutine(TakeSample());
            _isRecording = false;

            Debug.Log("Recording: " + _recording);

            // Save recording to the recording repo.
            RecordingRepoHandler.Instance.AddRecording(_recording);
        }

        /*private void Update()
        {
            if (_isRecording)
            {
                // Update current target sample.
                _timeSinceLastSample += Time.deltaTime;
                if (_timeSinceLastSample >= _timeBetweenSamples)
                {
                    // Reset timer.
                    _timeSinceLastSample = 0;

                    // Create new sample.
                    RecordingSample sample = new RecordingSample(_transforms.Length);

                    // Move towards target.
                    for (var i = 0; i < _transforms.Length; i++)
                    {
                        var t = _transforms[i];

                        sample.Positions[i] = t.position;
                        sample.Rotations[i] = t.eulerAngles;
                    }

                    // Add sample to recording.
                    _recording.Samples.Add(sample);

                    Debug.Log("Sample: " + sample);
                }
            }
        }*/

        private IEnumerator TakeSample()
        {
            while (_isRecording)
            {
                // Create new sample.
                RecordingSample sample = new RecordingSample(_transforms.Length);

                // Move towards target.
                for (var i = 0; i < _transforms.Length; i++)
                {
                    var t = _transforms[i];

                    sample.Positions[i] = t.localPosition;
                    sample.Rotations[i] = t.localEulerAngles;
                }

                // Add sample to recording.
                _recording.Samples.Add(sample);

                Debug.Log("Sample: " + sample);

                yield return new WaitForSeconds(_timeBetweenSamples);
            }
        }

    }
}