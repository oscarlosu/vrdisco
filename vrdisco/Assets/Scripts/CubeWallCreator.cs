using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject _cubePrefab;
    [SerializeField]
    private int _gridWidth, _gridHeight;
    [SerializeField]
    private float _cubeWidth, _cubeHeight;
    [SerializeField]
    private float _spacingHorizontal, _spacingVertical;

	// Use this for initialization
	private void Start ()
	{

	    float startX = transform.position.x - ((_gridWidth * _cubeWidth) / 2f) - ((_gridWidth * _spacingHorizontal) / 2f) + _cubeWidth / 2f;
	    float startY = transform.position.y - ((_gridHeight * _cubeHeight) / 2f) - ((_gridHeight * _spacingVertical) / 2f) + _cubeHeight / 2f;

	    for (int y = 0; y < _gridHeight; y++)
	    {
	        for (int x = 0; x < _gridWidth; x++)
	        {
	            Vector3 pos = new Vector3(startX + x * _cubeWidth + x * _spacingHorizontal, startY + y * _cubeHeight + y * _spacingVertical, 0);
                GameObject cube = Instantiate(_cubePrefab, pos, Quaternion.identity);
            }
	    }
	    

	}
}
