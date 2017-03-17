using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArmVisualizer : MonoBehaviour {
    [SerializeField]
    protected Transform _shoulder;
    [SerializeField]
    protected Transform _elbow;
    [SerializeField]
    protected Transform _hand;

    private LineRenderer _line;

    void Start () {
        _line = GetComponent<LineRenderer>();
        _line.numPositions = 3;
	}

    void FixedUpdate () {
        _line.SetPosition(0, _shoulder.position);
        _line.SetPosition(1, _elbow.position);
        _line.SetPosition(2, _hand.position);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(_shoulder.position, _elbow.position);
        Gizmos.DrawLine(_elbow.position, _hand.position);
    }
}
