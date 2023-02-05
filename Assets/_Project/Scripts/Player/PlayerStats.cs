using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int health = 3;

    [SerializeField]
    private GameObjectValueList players;

    [SerializeField]
    private BoolVariableInstancer isPlayerInsideCameraBounds;

    [SerializeField]
    private UnityEvent onGameOver;

    private void Update()
    {
        if (!isPlayerInsideCameraBounds.Value)
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
        List<PlayerStats> playersStatsList = new List<PlayerStats>();

        foreach (GameObject gObj in players.List)
        {
            PlayerStats ps = gObj.GetComponent<PlayerStats>();
            if (ps != this && health > 0)
            {
                playersStatsList.Add(ps);
            }
        }
        if (playersStatsList.Count > 0)
        {
            health--;
            playersStatsList[0].health--;
            playersStatsList.Shuffle();
            transform.position = playersStatsList[0].transform.position;
        }
        else
        {
            onGameOver.Invoke();
        }

        //int randomIndex = Random.Range(0, players.Count - 1);

        //for (int i = 0; i < players.Count; i++)
        //{
        //    if (players[i] != gameObject)
        //    {
        //        if (randomIndex == 0)
        //        {
        //            transform.position = players[i].transform.position;
        //        }
        //        else
        //        {
        //            randomIndex--;
        //        }
        //    }
        //}
    }
}