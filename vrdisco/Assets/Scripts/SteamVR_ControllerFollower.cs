using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVR_ControllerFollower : MonoBehaviour {
    [SerializeField]
    protected Transform _target;
    SteamVR_Events.Action newPosesAppliedAction;
    protected void Awake() {
        newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);
    }

    private void OnEnable() {
        newPosesAppliedAction.enabled = true;
    }

    private void OnDisable() {
        newPosesAppliedAction.enabled = false;
    }

    private void OnNewPosesApplied() {
        if (_target != null) {
            transform.position = _target.position;
            transform.rotation = _target.rotation;
        }
    }
}
