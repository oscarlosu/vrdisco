using System.Collections;
using System.Collections.Generic;
using MusicSystem;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public static MusicPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
