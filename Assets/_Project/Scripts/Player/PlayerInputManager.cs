using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

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

    private bool gameStarted = false;

    Vector2 moveAxis = Vector2.zero;
    public void GetMovementAxis(CallbackContext ctx)
    {
        AssignAxis(ctx.ReadValue<Vector2>());
    }

    public void AssignAxis(Vector2 axis){
        moveAxis = axis;
    }

    public void OnGameStart()
    { 
        gameStarted= true;
    }

    private void Update()
    {
        if (!gameStarted) return;

        Vector2 velocity = Vector2.down * gravitySpeed;

        velocity.x = moveAxis.x * movingSpeed;

        velocity.x += Random.Range(horizontalRandomRange.x, horizontalRandomRange.y);

        rigidbody2D.velocity = velocity;
    }

}
