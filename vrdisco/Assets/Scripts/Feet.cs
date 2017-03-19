using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {
    [SerializeField]
    protected Foot _left;
    [SerializeField]
    protected Foot _right;

    [SerializeField]
    protected float maxStride;
    [SerializeField]
    protected AnimationCurve _stepCurve;

    [SerializeField]
    protected float _feetVelocity;
    [SerializeField]
    protected float _stepHeight = 0.5f;

    private Foot lastFootMoved;

    [SerializeField]
    protected Transform _groundLevel;    


    void Start () {
        lastFootMoved = _left;

        // Listen to controller connection
        _left.Controller.ControllerConnected += ListenToLeftTrackpad;
        _left.Controller.ControllerDisconnected += StopListenToLeftTrackpad;
        _right.Controller.ControllerConnected += ListenToRightTrackpad;
        _right.Controller.ControllerDisconnected += StopListenToRightTrackpad;

        // Initialize feet
        _left.MoveTarget = _left.IKTarget.position;
        _left.LastGround = _left.IKTarget.position;
        _right.MoveTarget = _right.IKTarget.position;
        _right.LastGround = _right.IKTarget.position;

    }

    private void ListenToLeftTrackpad() {
        _left.Controller.PadClicked += LiftFoot;
        _left.Controller.PadUnclicked += LowerFoot;
    }

    private void StopListenToLeftTrackpad() {
        _left.Controller.PadClicked -= LiftFoot;
        _left.Controller.PadUnclicked -= LowerFoot;
    }

    private void ListenToRightTrackpad() {
        _right.Controller.PadClicked += LiftFoot;
        _right.Controller.PadUnclicked += LowerFoot;
    }

    private void StopListenToRightTrackpad() {
        _right.Controller.PadClicked -= LiftFoot;
        _right.Controller.PadUnclicked -= LowerFoot;
    }

    private void OnDisable() {
        StopListenToLeftTrackpad();
        StopListenToRightTrackpad();
    }

    void Update () {
        // Register last ground for each foot
        // Left
        if(Mathf.Abs(_left.MoveTarget.y - _groundLevel.position.y) < Mathf.Epsilon) {
            _left.LastGround = _left.FootTransform.position;
        }

        // Update targets with trackpad position
        // Left
        if(_left.Controller.isPadDown) {
            Vector2 pad = _left.Controller.GetTouchpadAxis();
            _left.MoveTarget = MidStepTarget(_left, pad.x, pad.y);
        }
        // Right
        if (_right.Controller.isPadDown) {
            Vector2 pad = _right.Controller.GetTouchpadAxis();
            _right.MoveTarget = MidStepTarget(_right, pad.x, pad.y);
        }


        // Right
        if (Mathf.Abs(_right.MoveTarget.y - _groundLevel.position.y) < Mathf.Epsilon) {
            _right.LastGround = _right.FootTransform.position;
        }
        // Lerp towards each foot's target
        float timeToMoveTarget = Vector3.Distance(_left.MoveTarget, _left.IKTarget.position) / _feetVelocity;
        if(timeToMoveTarget > Mathf.Epsilon) {
            _left.IKTarget.position = Vector3.Lerp(_left.IKTarget.position, _left.MoveTarget, Time.deltaTime / timeToMoveTarget);
        }        
        timeToMoveTarget = Vector3.Distance(_right.MoveTarget, _right.IKTarget.position) / _feetVelocity;
        if(timeToMoveTarget > Mathf.Epsilon) {
            _right.IKTarget.position = Vector3.Lerp(_right.IKTarget.position, _right.MoveTarget, _stepCurve.Evaluate(Time.deltaTime / timeToMoveTarget));
        }   
             
    }

    void LiftFoot(object sender, ClickedEventArgs e) {
        // Which foot?
        Foot f;
        if (((WandController)sender) == _left.Controller) {
            f = _left;
        } else {
            f = _right;
        }
        // Set target position to midstep position
        f.MoveTarget = MidStepTarget(f, e.padX, e.padY);
    }

    void LowerFoot(object sender, ClickedEventArgs e) {
        // Which foot?
        Foot f;
        if (((WandController)sender) == _left.Controller) {
            f = _left;
        } else {
            f = _right;
        }
        // Set target position to end step position
        f.MoveTarget = StepTarget(f, e.padX, e.padY);
    }

    Vector3 MidStepTarget(Foot foot, float padX, float padY) {
        Vector3 stepTarget = StepTarget(foot, padX, padY);
        Vector3 midPointXZ = (foot.LastGround + stepTarget) / 2.0f;
        return midPointXZ + _stepHeight * Vector3.up;
    }

    Vector3 StepTarget(Foot foot, float padX, float padY) {
        return foot.CenterXZ + new Vector3(padX, 0, padY) * maxStride + Vector3.up * _groundLevel.position.y;
    }

    private void OnDrawGizmos() {        
        int lines = 30;
        // Valid feet area
        if (_left.Thigh != null) {
            Gizmos.color = Color.green;
            DrawFlatCircle(_left.Center, lines, maxStride);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_left.Thigh.position, _left.Center);
        }
        if (_right.Thigh != null) {
            Gizmos.color = Color.green;
            DrawFlatCircle(_right.Center, lines, maxStride);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_right.Thigh.position, _right.Center);
        }


        if(_right != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_right.MoveTarget, 0.05f);
        }
        if(_left != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_left.MoveTarget, 0.05f);
        }
    }

    void DrawFlatCircle(Vector3 center, int lines, float radius) {
        float inc = 2 * Mathf.PI / (float)lines;
        for (int l = 0; l < lines; ++l) {
            int l1 = l % lines;
            int l2 = (l + 1) % lines;
            Vector3 pos1 = center + new Vector3(radius * Mathf.Cos(l1 * inc), 0, radius * Mathf.Sin(l1 * inc));
            Vector3 pos2 = center + new Vector3(radius * Mathf.Cos(l2 * inc), 0, radius * Mathf.Sin(l2 * inc));
            Gizmos.DrawLine(pos1, pos2);
        }
    }
}
