using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private GameObjectValueList players;

    [SerializeField]
    private BoolVariableInstancer isPlayerInsideCameraBounds;

    private void Update()
    {
        if(!isPlayerInsideCameraBounds.Value)
        {
            Die();
        }
    }

    public void Die()
    {
        Respawn();
    }

    private void Respawn()
    {
        int randomIndex = Random.Range(0, players.Count - 1);

        for(int i = 0; i < players.Count; i++)
        {
            if (players[i] != gameObject)
            {
                if(randomIndex == 0)
                {
                    transform.position = players[i].transform.position;
                }
                else
                {
                    randomIndex--;
                }
            }
        }
    }
}
