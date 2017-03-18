using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LimbVisualizer : MonoBehaviour {
    [SerializeField]
    protected List<Transform> _joints;

    private LineRenderer _line;

    void Start () {
        _line = GetComponent<LineRenderer>();
        _line.numPositions = _joints.Count;
	}

    void FixedUpdate () {
        for(int i = 0; i < _joints.Count; ++i) {
            _line.SetPosition(i, _joints[i].position);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        for (int i = 0; i < _joints.Count - 1; ++i) {
            Gizmos.DrawLine(_joints[i].position, _joints[i+1].position);
        }
    }
}
