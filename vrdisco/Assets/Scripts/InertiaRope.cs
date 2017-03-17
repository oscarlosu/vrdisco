using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class InertiaRope : MonoBehaviour {
    [SerializeField]
    private VelocityTracker _target;
    [SerializeField]
    private float _resolution;
    [SerializeField]
    private float _length;
    [Header("Inertia Params")]
    [SerializeField]
    private AnimationCurve _inertiaCurve;
    [SerializeField]
    private float _inertiaStrength = 1;
    [SerializeField]
    private float _inertiaDuration = 1;

    [Header("Bending Params")]
    [SerializeField]
    private AnimationCurve _bendCurve;
    [SerializeField]
    private bool _useDelayFollow = true;
    [SerializeField]
    private float _minSpeed;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private AnimationCurve _influenceCurve;

    private LineRenderer _line;


    float velInertiaTime = 0;
    float angVelInertiaTime = 0;


    Vector3 inertiaOffset;

    void Start () {
        BuildRope();
    }
	
	void Update () {
        UpdateInertiaRef();

        UpdateRope(false);

    }

    void UpdateInertiaRef() {
        if (_target.Velocity.magnitude > 0) {
            velInertiaTime = 0;
            inertiaOffset = Vector3.zero;
        } else {
            inertiaOffset = _target.LastNotZeroVel * _inertiaCurve.Evaluate(velInertiaTime / _inertiaDuration) * _inertiaStrength;
            velInertiaTime += Time.deltaTime;
        }

        // TODO: Angular velocity ?
    }

    void BuildRope() {
        _line = GetComponent<LineRenderer>();

        UpdateRope(true);
    }

    void UpdateRope(bool immedidate) {

        if (_target && _line) {

            // Adjust rope length
            int nSegments = (int)(_length * _resolution);

            _line.numPositions = nSegments;

            // Restrict rope ends to length
            Vector3 dir = _target.transform.position - transform.position;
            _target.transform.position = transform.position + dir.normalized * Mathf.Clamp(dir.magnitude, 0, _length);

            // Calculate sag        
            float d = Vector3.Distance(transform.position, _target.transform.position);
            float h = Mathf.Clamp(_length - d, 0, _length) / 2.0f;
            Vector3 midpoint = (transform.position + _target.transform.position) / 2.0f + h * Vector3.down;


            float segmentLength = _length / _line.numPositions;
            //int midpointIndex = (int)(d / segmentLength);
            int midpointIndex = (int)(_line.numPositions / 2.0f);

            // Cache frame-rate scaled min and max speeds
            float deltaMinSpeed = _minSpeed * Time.deltaTime, deltaMaxSpeed = _maxSpeed * Time.deltaTime;

            // Assign positions to line renderer points
            float hangT;
            float leftSideMax = midpointIndex - 1;
            float rightSideMax = _line.numPositions - midpointIndex;
            for (int i = 0; i < _line.numPositions; ++i) {
                // Calculate rest position
                Vector3 pos = Vector3.Lerp(transform.position, _target.transform.position, i / (float)(_line.numPositions - 1));
                if (i < midpointIndex) {
                    hangT = i / leftSideMax;
                } else {
                    hangT = (_line.numPositions - i) / rightSideMax;
                }


                float hang = h > 0 ? Mathf.Lerp(0, h, _bendCurve.Evaluate(Mathf.Clamp(hangT, 0, 1))) : 0;
                Vector3 restPos = pos + hang * Vector3.down;

                Vector3 newPos;
                if (immedidate) {
                    newPos = restPos;
                } else if(_target.Velocity.magnitude > 0) {
                    if(_useDelayFollow) {
                        float normalizedIndex = i / (float)_line.numPositions;
                        newPos = Vector3.Lerp(_line.GetPosition(i), restPos, Mathf.Lerp(deltaMinSpeed, deltaMaxSpeed, _influenceCurve.Evaluate(normalizedIndex)));
                    } else {
                        newPos = restPos;
                    }
                } else {
                    float interpolation = (i <= midpointIndex ? Mathf.InverseLerp(0, midpointIndex, i) : 
                                                                Mathf.InverseLerp(_line.numPositions - 1, midpointIndex, i));
                    newPos = restPos + Vector3.Lerp(Vector3.zero, inertiaOffset, _bendCurve.Evaluate(interpolation));
                }


                _line.SetPosition(i, newPos);
            }
        }


    }


    private void OnDrawGizmos() {
        if (_line) {
            int midpointIndex = (int)(_line.numPositions / 2.0f);


            for (int i = 0; i < _line.numPositions; ++i) {
                if(i == midpointIndex) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(_line.GetPosition(i), 0.1f);
                } else {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(_line.GetPosition(i), 0.01f);
                }
                
            }
        }
    }
}
