using Coherence.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class NetworkedInputManager : MonoBehaviour
{
    [SerializeField]
    CoherenceInput inputSync;
    [SerializeField]
    CoherenceSync objectSync;
    [SerializeField]
    NetworkedCharacterController charController;

    Vector2 horizontalValue = Vector2.zero;
    public void GetHorizontalAxis(CallbackContext ctx)
    {
        if (ctx.started)
        {
            horizontalValue = ctx.ReadValue<Vector2>();
            inputSync.SetAxisState("horizontal", horizontalValue);
            Debug.Log($"Sending input value: {horizontalValue}");
        }
    }

    private void FixedUpdate()
    {
        if (!objectSync.MonoBridge.IsSimulatorOrHost)
            return;

        if (inputSync != null)
        {
            var horizontal = inputSync.GetAxisState("horizontal");
            charController.SetMoveInput(horizontal);
            Debug.Log($"Receiving value: {horizontal}");
        }
    }

}
