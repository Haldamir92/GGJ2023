using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed = 1;

    [SerializeField]
    private KeyCode leftKey = KeyCode.A;

    [SerializeField]
    private KeyCode rightKey = KeyCode.D;
    
    private void Update()
    {
        if (Input.GetKey(leftKey))
        {
            transform.position += Vector3.left * Time.deltaTime * movingSpeed;
        }
        else if (Input.GetKey(rightKey))
        {
            transform.position += Vector3.right * Time.deltaTime * movingSpeed;
        }

        transform.position += Vector3.down * Time.deltaTime * movingSpeed;
    }

}
