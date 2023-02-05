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

    [SerializeField]
    internal Color customColor = Color.white;


    public float Scale { get => scale; set => scale = value; }

    private List<GameObject> tailGameObjectList = new List<GameObject>();

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
            go.GetComponent<SpriteRenderer>().color = customColor;
            tailGameObjectList.Add(go);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SetScale(float size)
    {
        scale = size;
        StopAllCoroutines();
        StartCoroutine(PrintSprite());
    }

    public void IncreaseTailSize()
    {
        foreach (GameObject tailNode in tailGameObjectList)
        {
            tailNode.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}