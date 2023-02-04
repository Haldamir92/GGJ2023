using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable[] interactables = collision.GetComponentsInParent<IInteractable>();

        foreach(IInteractable interactable in interactables)
        {
            interactable.Interact();
        }
    }
}
