using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : MonoBehaviour {
    [SerializeField]
    protected Transform _target;
    [SerializeField]
    protected Transform _torso;
    [SerializeField]
    protected Transform _face;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Torso and face follow target position
        //_torso.position = _target.position;

        // Torso follows target xz rotation
        _torso.rotation = _target.rotation;
        //_torso.localEulerAngles = new Vector3(_target.eulerAngles.x, _torso.localEulerAngles.y, _target.eulerAngles.z);
        // Face folllows y rotation
        //_face.localEulerAngles = new Vector3(_face.localEulerAngles.x, _target.eulerAngles.y, _face.localEulerAngles.z);
	}
}
