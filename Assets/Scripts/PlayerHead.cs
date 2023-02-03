using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField]
    private GameObject rootNodePrefab;

    [SerializeField]
    private float movingSpeed = 1;

    [SerializeField]
    private float spawnTime = 1;

    [SerializeField]
    private KeyCode leftKey = KeyCode.A;

    [SerializeField]
    private KeyCode rightKey = KeyCode.D;


    void Start()
    {
        StartCoroutine(PrintRootNode());
    }

    private void Update()
    {
        if(Input.GetKey(leftKey))
        {
            transform.position += Vector3.left * Time.deltaTime * movingSpeed;
        }
        else if (Input.GetKey(rightKey))
        {
            transform.position += Vector3.right * Time.deltaTime * movingSpeed;
        }

        transform.position += Vector3.down * Time.deltaTime * movingSpeed;
    }

    private IEnumerator PrintRootNode()
    {
        while (true)
        {
            Instantiate(rootNodePrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
