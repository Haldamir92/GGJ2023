using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SingleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnPosition;
    // [SerializeField] private bool isFacingRight;
    [Button]
    public GameObject SpawnAtPosition(){
        return Instantiate(this.prefabToSpawn, spawnPosition.position, Quaternion.identity);
        //add direction
    }
}
