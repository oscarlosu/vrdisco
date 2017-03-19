using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Foot {
    public WandController Controller;
    public Transform IKTarget;
    public Transform FootTransform;
    [HideInInspector]
    public Vector3 LastGround;
    [HideInInspector]
    public Vector3 MoveTarget;

}
