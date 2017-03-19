using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFromHmd : MonoBehaviour {
    [SerializeField]
    protected List<GameObject> _things;
    [SerializeField]
    protected string _invisibleForHmd = "InvisibleForHmd";

    void Start () {
        int layer = LayerMask.NameToLayer(_invisibleForHmd);
        foreach (GameObject thing in _things) {
            thing.layer = layer;
        }
	}
	
	void Update () {
		
	}
}
