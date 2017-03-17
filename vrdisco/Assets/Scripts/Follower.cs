using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField]
    protected Transform _target;

    [SerializeField]
    protected bool _copyPos = true;
    [SerializeField]
    protected bool _copyRot = true;


    protected void Start () {
		
	}
	
	protected void Update () {
        if(_copyPos) {
            transform.position = _target.position;
        }
        if(_copyRot) {
            transform.rotation = _target.rotation;
        }        
    }
}
