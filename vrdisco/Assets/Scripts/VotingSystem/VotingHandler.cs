using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VotingSystem
{
    public class VotingHandler : MonoBehaviour
    {
        public static VotingHandler Instance { get; private set; }

        public int PlayerPoints { get; private set; }
        public int OpponentPoints { get; private set; }

        public int ExpectedMaxVoteCount
        {
            get { return _expectedMaxVoteCount; }
        }

        private int _playerPointQueue;
        private int _opponentPointQueue;

        private bool _canVote;
        private bool _consumeVotes;
        private bool _isCountPointsRunning;

        [Header("Settings")] [SerializeField] private float _secondsBetweenVoteCount = 0.1f;
        [SerializeField] private int _expectedMaxVoteCount = 10;

        public UnityEvent PointsUpdatedEvent;

        public void AddPlayerPoint()
        {
            _playerPointQueue++;
        }

        public void AddOpponentPoint()
        {
            _opponentPointQueue++;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (_canVote)
            {
                // As long as audience can vote, also count votes.
                if (_consumeVotes && !_isCountPointsRunning)
                {
                    StartCoroutine(CountPoints());
                }

                // This is for testing purposes, but allows voting from keyboard through arrow keys.
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    AddPlayerPoint();
                    Debug.Log("Voted for the player! Now at: " + PlayerPoints);
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    AddOpponentPoint();
                    Debug.Log("Voted for the opponent! Now at: " + OpponentPoints);
                }
            }
        }

        private IEnumerator CountPoints()
        {
            _isCountPointsRunning = true;
            // As long as there are points in just one of the queues, continue counting.
            while (_playerPointQueue > 0 || _opponentPointQueue > 0)
            {
                if (_playerPointQueue > 0)
                {
                    PlayerPoints++;
                    _playerPointQueue--;
                }
                if (_opponentPointQueue > 0)
                {
                    OpponentPoints++;
                    _opponentPointQueue--;
                }

                // Update the expected max, if the current votes go too high.
                if (PlayerPoints > (int) (_expectedMaxVoteCount * 0.75) ||
                    OpponentPoints > (int) (_expectedMaxVoteCount * 0.75))
                {
                    _expectedMaxVoteCount = (int)(_expectedMaxVoteCount * 1.5);
                }

                yield return new WaitForSeconds(_secondsBetweenVoteCount);
            }
            _isCountPointsRunning = false;
            PointsUpdatedEvent.Invoke();
        }

        [ContextMenu("Start fake voting")]
        private void StartFakeVoting()
        {
            Debug.Log("Fake voting has officially started!");
            _canVote = true;
            Invoke("StartConsumingVotes", 3);
            Invoke("StopFakeVoting", 10);
        }

        private void StartConsumingVotes()
        {
            _consumeVotes = true;
        }

        private void StopFakeVoting()
        {
            _canVote = false;
            _consumeVotes = true;
            Debug.Log("Fake voting has stopped!");
        }
    }
}