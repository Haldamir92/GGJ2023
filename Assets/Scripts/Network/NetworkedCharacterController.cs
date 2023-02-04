using System.Collections;
using UnityEngine;

public class NetworkedCharacterController : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed = 1;

    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 moveAxis;

    public void SetMoveInput(Vector2 axis)
    {
        moveAxis = axis;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 direction = moveAxis * movingSpeed;
        direction.y = -movingSpeed;
        transform.position += (Vector3)direction * Time.deltaTime;
    }
}
