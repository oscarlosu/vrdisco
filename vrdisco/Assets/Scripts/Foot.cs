using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Foot {
    public WandController Controller;
    public Transform Target;
    public Transform FootTransform;

    public enum FootState {
        Up, Down, MovingUp, MovingDown
    }
    [HideInInspector]
    public FootState State = FootState.Down;
    [HideInInspector]
    public bool AutoLower = false;

}
