using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField]
    protected Transform _target;

	protected void Start () {
		
	}
	
	protected void Update () {
        transform.position = _target.position;
        transform.rotation = _target.rotation;
    }
}
