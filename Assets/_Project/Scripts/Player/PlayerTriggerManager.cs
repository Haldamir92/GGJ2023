using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemiesLayerMask;

    [SerializeField]
    private UnityEvent<GameObject> onHitEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, enemiesLayerMask))
        {
            onHitEnemy?.Invoke(collision.gameObject);
        }
        else
        {
            IInteractable[] interactables = collision.GetComponentsInParent<IInteractable>();

            foreach (IInteractable interactable in interactables)
            {
                interactable.Interact();
            }
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }


}
