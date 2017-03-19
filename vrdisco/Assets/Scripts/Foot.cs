using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Foot {
    public WandController Controller;
    public Transform IKTarget;
    public Transform FootTransform;
    public Transform Thigh;
    public float LegLength;

    public Vector3 Center {
        get {
            RaycastHit hit;
            if (Physics.Raycast(Thigh.position, Vector3.down, out hit, LegLength)) {
                return hit.point;
            } else {
                return Thigh.position + LegLength * Vector3.down;
            }
        }
    }

    public Vector3 CenterXZ {
        get {
            return Thigh.position - Vector3.up * Thigh.position.y;
        }
    }


    [HideInInspector]
    public Vector3 LastGround;
    [HideInInspector]
    public Vector3 MoveTarget;

}
