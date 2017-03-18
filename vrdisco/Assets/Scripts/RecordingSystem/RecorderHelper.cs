using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;

namespace RecordingSystem.Helpers
{
    public class RecorderHelper : MonoBehaviour
    {
        [SerializeField]
        [Header("Click R to start and stop recording")]
        private Recorder _recorder;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                if (!_recorder.IsRecording)
                {
                    MusicSystemSetup.Instance.PlayFirstMusicOnPlayer();
                    _recorder.StartRecording();
                    Debug.Log("Recording started!");
                }
                else
                {
                    _recorder.StopRecording();
                    MusicPlayer.Instance.Stop();
                    Debug.Log("Recording ended!\nSamples: " + _recorder.LastRecording.Samples.Count);
                }
            }

        }
    }
}