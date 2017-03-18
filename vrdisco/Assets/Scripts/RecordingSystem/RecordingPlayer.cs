using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MusicSystem;
using UnityEngine;

namespace RecordingSystem
{
    public class RecordingPlayer : MonoBehaviour
    {
        public bool IsPlaying { get { return _isPlaying; } }

        [SerializeField]
        private Transform[] _transforms;
        
        private Recording _recording;

        private bool _isPlaying;
        private int _recordingSampleIndex;
        private float _timeSinceLastSample;
        private float _timeBetweenSamples;

        [SerializeField]
        private float _testTimeReducer;

        public void PlayRecording()
        {
            // Check that the transforms fit the recording.
            if (_transforms.Length != _recording.TransformCount)
            {
                Debug.LogError("Couldn't play the recording, because the number of recordet transforms doesn't match the number of given transforms.");
                return;
            }

            // Reset variables.
            StopAllCoroutines();
            _recordingSampleIndex = 0;

            // Calculate rate of sample change.
            _timeBetweenSamples = 1 / _recording.SampleRate;
            _timeBetweenSamples -= _testTimeReducer;

            // Play recording.
            _isPlaying = true;
            StartCoroutine(PlaySample());
        }

        /*private void Update()
        {
            if (_isPlaying)
            {
                // Update current target sample.
                _timeSinceLastSample += Time.deltaTime;
                if (_timeSinceLastSample >= _timeBetweenSamples)
                {
                    // Reset timer.
                    _timeSinceLastSample = 0;

                    if (_recordingSampleIndex >= _recording.Samples.Count-1)
                    {
                        _recordingSampleIndex = 0;
                    }
                    else
                    {
                        _recordingSampleIndex++;
                    }

                    // Move towards target.
                    for (var i = 0; i < _transforms.Length; i++)
                    {
                        var t = _transforms[i];

                        t.DOMove(_recording.Samples[_recordingSampleIndex].Positions[i], _timeBetweenSamples);
                        t.DORotate(_recording.Samples[_recordingSampleIndex].Rotations[i], _timeBetweenSamples);
                    }
                }
            }
        }*/

        private IEnumerator PlaySample()
        {
            while (_isPlaying)
            {
                // Move towards target.
                for (var i = 0; i < _transforms.Length; i++)
                {
                    var t = _transforms[i];

                    t.DOLocalMove(_recording.Samples[_recordingSampleIndex].Positions[i], _timeBetweenSamples);
                    t.DOLocalRotate(_recording.Samples[_recordingSampleIndex].Rotations[i], _timeBetweenSamples);
                }

                // Update the sample index.
                if (_recordingSampleIndex >= _recording.Samples.Count - 1)
                {
                    _recordingSampleIndex = 0;
                }
                else
                {
                    _recordingSampleIndex++;
                }

                yield return new WaitForSeconds(_timeBetweenSamples);
            }
        }

        [ContextMenu("Play first recording")]
        public void PlayFirstRecording()
        {
            var recordings = RecordingRepoHandler.Instance.GetMusicPieceRecordings(MusicPlayer.Instance.MusicPiece.Id);
            _recording = recordings[0];

            PlayRecording();
        }

    }
}