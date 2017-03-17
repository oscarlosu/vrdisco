using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour {
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _resolution;
    [SerializeField]
    private float _length;
    [SerializeField]
    private AnimationCurve _bendCurve;
    [SerializeField]
    private float _minSpeed;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private AnimationCurve _influenceCurve;


    private LineRenderer _line;

	// Use this for initialization
	void Start () {
        BuildRope();

    }
	
	// Update is called once per frame
	void Update () {
        UpdateRope(false);

    }

    void BuildRope() {
        _line = GetComponent<LineRenderer>();

        UpdateRope(true);

    }

    
    void UpdateRope(bool immedidate) {

        if(_target && _line) {

            // Adjust rope length
            int nSegments = (int)(_length * _resolution);

            _line.numPositions = nSegments;

            // Restrict rope ends to length
            Vector3 dir = _target.position - transform.position;
            _target.position = transform.position + dir.normalized * Mathf.Clamp(dir.magnitude, 0, _length);

            // Calculate sag        
            float d = Vector3.Distance(transform.position, _target.position);
            float h = Mathf.Clamp(_length - d, 0, _length) / 2.0f;
            Vector3 midpoint = (transform.position + _target.position) / 2.0f + h * Vector3.down;


            float segmentLength = _length / _line.numPositions;
            int midpointIndex = (int)(d / segmentLength);

            // Cache frame-rate scaled min and max speeds
            float deltaMinSpeed = _minSpeed * Time.deltaTime, deltaMaxSpeed = _maxSpeed * Time.deltaTime;

            // Assign positions to line renderer points
            float hangT;
            float leftSideMax = midpointIndex - 1;
            float rightSideMax = _line.numPositions - midpointIndex;
            for (int i = 0; i < _line.numPositions; ++i) {
                Vector3 pos = Vector3.Lerp(transform.position, _target.position, i / (float)(_line.numPositions - 1));
                if(i < midpointIndex) {
                    hangT = i / leftSideMax;
                } else {
                    hangT = (_line.numPositions - i) / rightSideMax;
                }
                

                float hang = h > 0 ? Mathf.Lerp(0, h, _bendCurve.Evaluate(Mathf.Clamp(hangT, 0, 1))) : 0;
                Vector3 restPos = pos + hang * Vector3.down;

                Vector3 newPos;
                if(immedidate) {
                    newPos = restPos;
                } else {
                    float normalizedIndex = i / (float)_line.numPositions;
                    newPos = Vector3.Lerp(_line.GetPosition(i), restPos, Mathf.Lerp(deltaMinSpeed, deltaMaxSpeed, _influenceCurve.Evaluate(normalizedIndex)));
                }
                

                _line.SetPosition(i, newPos);
            }
        }

        
    }

    private void OnDrawGizmos() {
        if(_line) {
            Gizmos.color = Color.blue;
            for (int i = 0; i < _line.numPositions; ++i) {
                Gizmos.DrawSphere(_line.GetPosition(i), 0.01f);
            }
        }        
    }
}
