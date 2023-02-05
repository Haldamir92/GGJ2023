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
    [SerializeField] internal Color playerColor = Color.white;

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
        var obj = Instantiate(this.prefabToSpawn, spawnPosition.position, Quaternion.identity);
        obj.GetComponent<PrefabSpawner>().customColor = playerColor;
        return obj;
        //add direction
    }

    public void Spawn()
    {
        SpawnAtPosition();
    }
}