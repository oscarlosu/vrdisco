using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VotingSystem
{
    public class VotingUiHandler : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField]
        private bool _isPlayer;
        [SerializeField]
        private float _voteBarSpeed = 3f;
        [SerializeField]
        private float _voteBarStartHeight = 25f;
        [SerializeField]
        private float _voteBarMaxHeight = 250f;

        [Header("UI elements")]
        [SerializeField]
        private Text _voteCountText;
        [SerializeField]
        private RectTransform _voteBarTransform;

        private float _voteBarYPosTarget;
        private float _voteBarHeightTarget;

        private int VoteCount { get { return _isPlayer ? VotingHandler.Instance.PlayerPoints : VotingHandler.Instance.OpponentPoints; } }

        private void Start()
        {
            // Set initial values and positions.
            _voteCountText.text = "0";
            // Update the vote count bar.
            float voteBarHeight = 0;
            float voteBarYPos = _voteBarStartHeight;
            _voteBarTransform.anchoredPosition = new Vector2(_voteBarTransform.anchoredPosition.x, voteBarYPos);
            _voteBarTransform.sizeDelta = new Vector2(_voteBarTransform.sizeDelta.x, voteBarHeight);

            // Listen for voting updates.
            VotingHandler.Instance.PointsUpdatedEvent.AddListener(UpdateUi);
        }

        public void UpdateUi()
        {
            // Set vote count as text.
            _voteCountText.text = VoteCount.ToString();
            // Find the new vote count bar targets.
            float voteBarHeight = ((_voteBarMaxHeight / VotingHandler.Instance.ExpectedMaxVoteCount) * VoteCount);
            float voteBarYPos = _voteBarStartHeight + voteBarHeight / 2;
            // Save targets.
            _voteBarYPosTarget = voteBarYPos;
            _voteBarHeightTarget = voteBarHeight;
        }

        private void Update()
        {
            // Lerp towards targets.
            float voteBarYPos = Mathf.Lerp(_voteBarTransform.anchoredPosition.y, _voteBarYPosTarget, _voteBarSpeed * Time.deltaTime);
            float voteBarHeight = Mathf.Lerp(_voteBarTransform.sizeDelta.y, _voteBarHeightTarget, _voteBarSpeed * Time.deltaTime);

            // Set new values.
            _voteBarTransform.anchoredPosition = new Vector2(_voteBarTransform.anchoredPosition.x, voteBarYPos);
            _voteBarTransform.sizeDelta = new Vector2(_voteBarTransform.sizeDelta.x, voteBarHeight);
        }

    }
}