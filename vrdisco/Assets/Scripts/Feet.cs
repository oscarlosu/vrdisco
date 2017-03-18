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


    void Start () {
        lastFootMoved = _left;
        _left.Controller.PadClicked += LiftFoot;
        _right.Controller.PadClicked += LiftFoot;

        _left.Controller.PadUnclicked += LowerFoot;
        _right.Controller.PadUnclicked += LowerFoot;
    }

    private void OnDisable() {
        _left.Controller.PadClicked -= LiftFoot;
        _right.Controller.PadClicked -= LiftFoot;

        _left.Controller.PadUnclicked -= LowerFoot;
        _right.Controller.PadUnclicked -= LowerFoot;
    }

    void Update () {
		
	}

    void LiftFoot(object sender, ClickedEventArgs e) {
        // Which foot?
        Foot f;
        if (((WandController)sender) == _left.Controller) {
            f = _left;
        } else {
            f = _right;
        }
        // Lift foot
        if (f.State == Foot.FootState.Down) {         
            StartCoroutine(LiftFootCo(f, e.padX, e.padY));
        }
    }

    IEnumerator LiftFootCo(Foot foot, float padX, float padY) {
        foot.State = Foot.FootState.MovingUp;

        Vector3 startPos = foot.FootTransform.position;
        Vector3 xzOfsset = new Vector3(padX, 0, padY);
        xzOfsset = xzOfsset * maxStride;
        Vector3 targetXZ = FeetCenter + xzOfsset;
        // Halfway through the step
        targetXZ = (startPos + targetXZ) / 2.0f;

        float duration = _stepCurve.keys[_stepCurve.length - 1].time;
        for (float time = 0; time < duration; time += Time.deltaTime) {
            float t = time / duration;
            foot.Target.position = Vector3.Lerp(startPos, targetXZ + _stepCurve.Evaluate(time) * Vector3.up, t);
            Debug.Log("Lerp time: " + t + " target pos: " + foot.Target.position);
            yield return null;
        }
        foot.Target.position = Vector3.Lerp(startPos, targetXZ + _stepCurve.Evaluate(duration) * Vector3.up, 1);

        

        // If trackpad was released while foot was still moving up, we lower the foot automatically
        if(foot.AutoLower) {            
            StartCoroutine(LowerFootCo(foot, padX, padY));
        } else {
            foot.State = Foot.FootState.Up;
        }
    }


    void LowerFoot(object sender, ClickedEventArgs e) {
        // Which foot?
        Foot f;
        if (((WandController)sender) == _left.Controller) {
            f = _left;
        } else {
            f = _right;
        }
        // Lower foot
        if (f.State == Foot.FootState.Up) {
            StartCoroutine(LowerFootCo(f, e.padX, e.padY));
        } else if(f.State == Foot.FootState.MovingUp) {
            f.AutoLower = true;
            Debug.Log("Auto lower " + Time.time);
        }
    }

    IEnumerator LowerFootCo(Foot foot, float padX, float padY) {
        if(foot.AutoLower) {
            Debug.Log("auto lower begun...");
            Debug.Log("AutoLower start target pos: " + foot.Target.position);
        }
        foot.AutoLower = false;

        foot.State = Foot.FootState.MovingDown;

        Vector3 startPos = foot.FootTransform.position;

        Vector3 xzOfsset = new Vector3(padX, 0, padY);
        xzOfsset *= maxStride;
        Vector3 targetXZ = FeetCenter + xzOfsset;

        float duration = _stepCurve.keys[_stepCurve.length - 1].time;
        for (float time = 0; time < duration; time += Time.deltaTime) {
            float t = time / duration;
            foot.Target.position = Vector3.Lerp(startPos + _stepCurve.Evaluate(time) * Vector3.down, targetXZ, t);
            yield return null;
        }
        foot.Target.position = Vector3.Lerp(startPos + _stepCurve.Evaluate(duration) * Vector3.down, targetXZ, 1);
        foot.State = Foot.FootState.Down;
    }

    void KeepFeetNearBody() {

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
