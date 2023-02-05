using Coherence.Toolkit;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class NetworkedInputManager : MonoBehaviour
{
    [SerializeField]
    CoherenceSync objectSync;
    [SerializeField]
    NetworkedCharacterController charController;

    Vector2 horizontalValue = Vector2.zero;

    public void GetHorizontalAxis(CallbackContext ctx)
    {
        horizontalValue = ctx.ReadValue<Vector2>();
        charController.SetMoveInput(horizontalValue);
    }
}
