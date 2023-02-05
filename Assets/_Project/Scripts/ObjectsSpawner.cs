using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    
    [SerializeField] private Vector3Variable minCameraWorldBound;
    [SerializeField] private Vector3Variable maxCameraWorldBound;
    [SerializeField] private GameObject[] obstaclesPrefabs;
    [SerializeField] private GameObject[] fertilizersPrefabs;

    [SerializeField] private float obstaclesProbability;

    [SerializeField] private float spawnDistanceBetweenObjects;
    private float nextDistance;

    private Camera myCamera;

    private void Start()
    {
        myCamera = Camera.main;
    }

    private void Update()
    {
        if (Mathf.Abs(myCamera.transform.position.y) >= nextDistance)
        {
            nextDistance = Mathf.Abs(myCamera.transform.position.y) + spawnDistanceBetweenObjects;
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        GameObject[] objectsArray;
        if (Probability.IsOccurred(obstaclesProbability))
        {
            objectsArray = obstaclesPrefabs;
        }
        else
        {
            objectsArray = fertilizersPrefabs;
        }

        objectsArray.Shuffle();
        Vector3 randomPosition = new Vector3(Random.Range(minCameraWorldBound.Value.x + 5, maxCameraWorldBound.Value.x - 5), minCameraWorldBound.Value.y, 0);
        Instantiate(objectsArray[0], randomPosition, objectsArray[0].transform.rotation);
    }
}