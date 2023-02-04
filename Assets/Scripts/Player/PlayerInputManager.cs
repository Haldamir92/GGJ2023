using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private float movingSpeed = 8;

    [SerializeField]
    private float gravitySpeed = 8;

    [SerializeField]
    private Vector2 horizontalRandomRange = new Vector2(-1.5f, 1.5f);

    [SerializeField]
    private KeyCode leftKey = KeyCode.A;

    [SerializeField]
    private KeyCode rightKey = KeyCode.D;
    
    private void Update()
    {
        //transform.position += Vector3.down * Time.deltaTime * gravitySpeed;
        Vector2 velocity = Vector2.down * gravitySpeed;

        if (Input.GetKey(leftKey))
        {
            //transform.position += Vector3.left * Time.deltaTime * movingSpeed;
            velocity.x -= movingSpeed;
        }

        if (Input.GetKey(rightKey))
        {
            //transform.position += Vector3.right * Time.deltaTime * movingSpeed;
            velocity.x += movingSpeed;
        }

        velocity.x += Random.Range(horizontalRandomRange.x, horizontalRandomRange.y);

        rigidbody2D.velocity = velocity;
    }

}
