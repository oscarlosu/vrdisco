using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;

namespace RecordingSystem.Helpers
{
    public class RecordingPlayerHelper : MonoBehaviour
    {
        [SerializeField]
        [Header("Click P to start and stop playing")]
        private RecordingPlayer _player;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                MusicSystemSetup.Instance.PlayFirstMusicOnPlayer();
                _player.PlayFirstRecording();
                Debug.Log("Playback started!");
            }

        }
    }
}