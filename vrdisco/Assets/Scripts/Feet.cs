using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {
    [SerializeField]
    protected Foot _left;
    [SerializeField]
    protected Foot _right;
    [SerializeField]
    protected Transform _body;

    [SerializeField]
    protected float maxStride;
    [SerializeField]
    protected AnimationCurve _stepCurve;

    [SerializeField]
    protected float _bodyToFloor;

    [SerializeField]
    protected float _feetVelocity;
    [SerializeField]
    protected float _stepHeight = 0.5f;

    private Foot lastFootMoved;

    public Vector3 FeetCenter {
        get {
            RaycastHit hit;
            if(Physics.Raycast(_body.position, Vector3.down, out hit, _bodyToFloor)) {
                return hit.point;
            } else {
                return _body.position + _bodyToFloor * Vector3.down;
            }

            
        }
    }

    public Vector3 BodyXZ {
        get {
            return _body.position - Vector3.up * _body.position.y;
        }
    }

    [SerializeField]
    protected Transform _groundLevel;


    void Start () {
        lastFootMoved = _left;

        _left.Controller.PadClicked += LiftFoot;
        _right.Controller.PadClicked += LiftFoot;

        _left.Controller.PadUnclicked += LowerFoot;
        _right.Controller.PadUnclicked += LowerFoot;

        // Initialize feet
        _left.MoveTarget = _left.IKTarget.position;
        _left.LastGround = _left.IKTarget.position;
        _right.MoveTarget = _right.IKTarget.position;
        _right.LastGround = _right.IKTarget.position;

    }

    private void OnDisable() {
        _left.Controller.PadClicked -= LiftFoot;
        _right.Controller.PadClicked -= LiftFoot;

        _left.Controller.PadUnclicked -= LowerFoot;
        _right.Controller.PadUnclicked -= LowerFoot;
    }

    void Update () {
        // Register last ground for each foot
        // Left
        if(Mathf.Abs(_left.MoveTarget.y - _groundLevel.position.y) < Mathf.Epsilon) {
            _left.LastGround = _left.FootTransform.position;
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
        f.MoveTarget = MidStepTarget(f.LastGround, e.padX, e.padY);
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
        f.MoveTarget = StepTarget(e.padX, e.padY);
    }

    Vector3 MidStepTarget(Vector3 lastGround, float padX, float padY) {
        Vector3 stepTarget = StepTarget(padX, padY);
        Vector3 midPointXZ = (lastGround + stepTarget) / 2.0f;
        return midPointXZ + _stepHeight * Vector3.up;
    }

    Vector3 StepTarget(float padX, float padY) {
        return BodyXZ + new Vector3(padX, _groundLevel.position.y, padY);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        int lines = 30;
        
        // Valid feet area
        if (_body != null) {
            DrawFlatCircle(_body.position + Vector3.down * _body.position.y, lines, maxStride);
        }

        Gizmos.DrawSphere(FeetCenter, 0.05f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_body.position, FeetCenter);

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
