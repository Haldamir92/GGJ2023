using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDriver : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private float movingSpeed = 8;

    [SerializeField]
    private float gravitySpeed = 8;

    [SerializeField]
    private Vector2 horizontalRandomRange = new Vector2(-1.5f, 1.5f);

    [SerializeField] internal Vector2 direction;
    private void Update()
    {
        Vector2 velocity = Vector2.down * gravitySpeed;

        velocity.x = direction.x * movingSpeed;

        velocity.x += Random.Range(horizontalRandomRange.x, horizontalRandomRange.y);

        rigidbody2D.velocity = velocity;
    }
}
