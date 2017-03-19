﻿using System;
using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{

    public static GameHandler Instance { get; private set; }

    public GameState CurrentState { get { return _gameState; } }

    [Header("Current game state")]
    [ReadOnly]
    [SerializeField]
    private GameState _gameState;

    [Header("Player info")]
    [SerializeField]
    private string _playerName;
    [SerializeField]
    private GameObject _playerCharacterPrefab;

    [Header("Opponent info")]
    [ReadOnly]
    [SerializeField]
    private string _opponentName;
    [ReadOnly]
    [SerializeField]
    private GameObject _opponentCharacterPrefab;

    [Header("Scene names")]
    [SerializeField]
    private string _startSceneName;
    [SerializeField]
    private string _dancingSceneName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.MusicEndEvent.AddListener(NextState);
        }
    }

    /// <summary>
    /// Check if all reaquirements are met and go to the next state.
    /// </summary>
    [ContextMenu("Go to next scene")]
    public void NextState()
    {
        switch (_gameState)
        {
            case GameState.Splash:
                SplashEnterPlayerNameTransition();
                break;
            case GameState.EnterPlayerName:
                if (string.IsNullOrEmpty(_playerName) && _playerCharacterPrefab != null)
                {
                    EnterPlayerNameDancingTransition();
                }
                else
                {
                    Debug.Log("Can't continue to next state. Player name or player character was not set.");
                }
                break;
            case GameState.Dancing:
                // When leaving the dancing state, remove the listener for the music end event.
                if (MusicPlayer.Instance != null)
                {
                    MusicPlayer.Instance.MusicEndEvent.RemoveListener(NextState);
                }
                DancingScoringTransition();
                break;
            case GameState.Scoring:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EnterNewState(GameState state)
    {
        // Sets what happens, when a state is entered.
        switch (state)
        {
            case GameState.Splash:
                break;
            case GameState.EnterPlayerName:
                // Reset / Set player and opponent info.
                _playerName = null;
                _playerCharacterPrefab = null; // TODO Add random initial character from a set of characters.
                _opponentName = null; // TODO Select a random opponent and fill out name and character.
                _opponentCharacterPrefab = null;

                // Disable double cameras (dancing) if in the same scene.
                break;
            case GameState.Dancing:
                // Register for when the music stops.
                if (MusicPlayer.Instance != null)
                {
                    MusicPlayer.Instance.MusicEndEvent.AddListener(NextState);
                }
                break;
            case GameState.Scoring:
                break;
            default:
                throw new ArgumentOutOfRangeException("state", state, null);
        }

        _gameState = state;
    }

    /// <summary>
    /// What happens, when moving from the splash to the enter player name state.
    /// </summary>
    private void SplashEnterPlayerNameTransition()
    {
        // Change camera position. // NOTE Perhaps move to look at the player typing their name?

        // Close the lid of the garbage bin.

        // Set new state.
        EnterNewState(GameState.EnterPlayerName);
    }

    /// <summary>
    /// What happens, when moving from the enter player name to the dancing state.
    /// </summary>
    private void EnterPlayerNameDancingTransition()
    {
        // Maybe change scene?

        // Make sure two cameras are working on the computer/tv monitor.

        // Start countdown till music starts.

        // Start music and player recording.
        
        // Set new state.
        EnterNewState(GameState.Dancing);
    }

    private void DancingScoringTransition()
    {
        // Display scoring UI, including sounds.

        // Set new state.
        EnterNewState(GameState.Scoring);
    }

    private void ScoringEnterPlayerNameTransition()
    {
        // Set new state.
        EnterNewState(GameState.EnterPlayerName);
    }

    public enum GameState
    {
        Splash,
        EnterPlayerName,
        Dancing,
        Scoring
    }
}
