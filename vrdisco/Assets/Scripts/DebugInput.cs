using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour {
    public WandController left;
    public WandController right;

	void Start () {
		
	}
	
	void Update () {
		
	}

    void OnGUI() {
        // Trackpads
        GUIContent content = new GUIContent("Left trackpad " + left.GetTouchpadAxis().ToString());
        GUI.Label(new Rect(Vector2.zero, new Vector2(100, 50)), content);

        content = new GUIContent("Right trackpad " + right.GetTouchpadAxis().ToString());
        GUI.Label(new Rect(new Vector2(0, 50), new Vector2(100, 50)), content);

        // Triggers

    }
}
