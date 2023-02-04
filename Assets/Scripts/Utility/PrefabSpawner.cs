using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float spawnTime = 1;

    void Start()
    {
        StartCoroutine(PrintSprite());
    }

    [SerializeField]
    private float rotationSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private IEnumerator PrintSprite()
    {
        while (true)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
