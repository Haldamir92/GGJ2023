using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float spawnTime = 1;

    [SerializeField]
    private float scale = 1;

    public float Scale { get => scale; set => scale = value; }

    private void Start()
    {
        StartCoroutine(PrintSprite());
    }

    private IEnumerator PrintSprite()
    {
        while (true)
        {
            GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
            go.transform.localScale = Vector3.one * scale;

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SetScale(float size)
    {
        scale = size;
        StopAllCoroutines();
        StartCoroutine(PrintSprite());
    }
}