using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour {
    [Range(0.0f, 1.0f)]
    public float LeakRate;
    public Vector3 Velocity = Vector3.zero;
    public Vector3 LeakyVelocity = Vector3.zero;
    public Vector3 LastNotZeroVel = Vector3.zero;
    public Vector3 AngVelocity = Vector3.zero;
    public Vector3 LeakyAngVelocity = Vector3.zero;
    public Vector3 LastNotZeroAngVel = Vector3.zero;

    private Vector3 lastPos;
    private Vector3 lastRot;

    // Use this for initialization
    void Start () {
        lastPos = transform.position;
        lastRot = transform.eulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        Velocity = (transform.position - lastPos) / Time.deltaTime;
        LeakyVelocity = Vector3.Lerp(Velocity, LeakyVelocity, LeakRate);
        if (Velocity.magnitude > 0) {
            LastNotZeroVel = LeakyVelocity;
        }
        AngVelocity = (lastRot - transform.eulerAngles) / Time.deltaTime;
        LeakyAngVelocity = Vector3.Lerp(AngVelocity, LeakyAngVelocity, LeakRate);
        if (AngVelocity.magnitude > 0) {
            LastNotZeroAngVel = LeakyAngVelocity;
        }

        lastPos = transform.position;
        lastRot = transform.eulerAngles;
    }
}
