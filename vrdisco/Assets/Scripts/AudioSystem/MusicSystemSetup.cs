﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MusicSystem
{

    public class MusicSystemSetup : MonoBehaviour
    {
        public static MusicSystemSetup Instance { get; private set; }

        [Header("Music pieces")]
        [SerializeField]
        private List<MusicPiece> _musicPieces;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        [ContextMenu("Generate music piece ids")]
        private void GenerateMusicPieceIds()
        {
            for (int i = 0; i < _musicPieces.Count; i++)
            {
                _musicPieces[i].Id = i;
            }
        }

        [ContextMenu("Play first piece on player")]
        public void PlayFirstMusicOnPlayer()
        {
            MusicPlayer.Instance.Play(_musicPieces.First());
        }

        [Serializable]
        public class MusicPiece
        {
            public int Id;
            public AudioClip AudioClip;
        }
    }
}