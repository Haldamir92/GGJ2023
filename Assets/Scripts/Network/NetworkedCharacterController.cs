using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkedCharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject rootNodePrefab;

    [SerializeField]
    private float movingSpeed = 1;

    [SerializeField]
    private float spawnTime = 1;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 moveAxis;

    public void SetMoveInput(Vector2 axis)
    {
        moveAxis = axis;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrintRootNode());
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAxis == Vector2.zero)
            return;

        Vector2 direction = moveAxis;
        direction.y = -1;
        rb.velocity += movingSpeed * Time.deltaTime * direction;
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
