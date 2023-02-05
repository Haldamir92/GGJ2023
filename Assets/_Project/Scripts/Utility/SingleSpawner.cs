using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class SingleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnPosition;

    // [SerializeField] private bool isFacingRight;
    //private void Start()
    //{
    //    StartCoroutine(StartBranching());
    //}

    //private IEnumerator StartBranching()
    //{
    //    while (true)
    //    {
    //        SpawnAtPosition();
    //        yield return new WaitForSeconds(6);
    //    }
    //}
    public GameObject SpawnAtPosition()
    {
        return Instantiate(this.prefabToSpawn, spawnPosition.position, Quaternion.identity);
        //add direction
    }

    public void Spawn()
    {
        SpawnAtPosition();
    }
}