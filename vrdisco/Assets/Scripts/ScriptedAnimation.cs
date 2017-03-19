using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScriptedAnimation : MonoBehaviour
{
    [SerializeField]
    private bool _usePosition = true;
    [SerializeField]
    private bool _useRotation = true;
    [SerializeField]
    private bool _useAdditiveRotations = false;

    [SerializeField]
    private List<Location> _locations = new List<Location>();

    private int _locationIndex;

    private void Start()
    {
        SetToInitialLocation();
    }

    [ContextMenu("Add current location")]
    private void AddLocation()
    {
        _locations.Add(new Location(transform.localPosition, transform.localEulerAngles));
    }

    [ContextMenu("Clear locations")]
    private void ClearLocations()
    {
        int locationCount = _locations.Count;
        _locations.Clear();
        Debug.Log("Locations removed: " + locationCount);
    }

    [ContextMenu("Set to initial location")]
    public void SetToInitialLocation()
    {
        _locationIndex = 0;
        if (_usePosition)
        {
            transform.localPosition = _locations[0].Position;
        }
        if (_useRotation)
        {
            transform.localEulerAngles = _locations[0].Rotation;
        }
    }

    [ContextMenu("Go to next location")]
    public void GoToNextLocation()
    {
        if (_locationIndex <= _locations.Count - 1)
        {
            _locationIndex++;
        }
        else
        {
            _locationIndex = 0;
        }
        Sequence sequence = DOTween.Sequence();
        if (_usePosition)
        {
            sequence.Insert(0, transform.DOLocalMove(_locations[_locationIndex].Position, _locations[_locationIndex].MovementTime));
        }
        if (_useRotation)
        {
            if (_useAdditiveRotations) {
                int prevLocIndex = _locationIndex > 0 ? _locationIndex - 1 : _locations.Count - 1;
                Vector3 result = _locations[_locationIndex].Rotation - _locations[prevLocIndex].Rotation;
                sequence.Insert(0, transform.DOLocalRotate(result, _locations[_locationIndex].MovementTime, RotateMode.WorldAxisAdd));
            }
            else
            {
                sequence.Insert(0, transform.DOLocalRotate(_locations[_locationIndex].Rotation, _locations[_locationIndex].MovementTime, RotateMode.FastBeyond360));
            }
        }

        sequence.Play();
    }

    public void GoToNextLocationInSeconds(float seconds)
    {
        Invoke("GoToNextLocation", seconds);
    }

    [Serializable]
    public class Location
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public float MovementTime;

        public Location(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
            MovementTime = 2f;
        }
    }
}
