using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private GameObjectValueList playerList;

    [SerializeField]
    private VoidEvent OnPlayerReady;

    [SerializeField]
    private int minPlayerCount;

    private bool started = false;

    private void Start()
    {
        StartCoroutine(CheckPlayerReady());
    }

    public void OnPlayerJoin(PlayerInput input)
    {
        playerList.Add(input.gameObject);
    }

    public void OnPlayerLeave(PlayerInput input)
    {
        playerList.Remove(input.gameObject);
    }

    private void OnDestroy()
    {
        playerList.Clear();
    }

    IEnumerator CheckPlayerReady()
    {
        while (!started)
        {
            if (playerList.Count >= minPlayerCount)
            {
                if (playerList.Select(x => x.GetComponent<ReadyStatus>()).All(x => x.IsReady))
                {
                    //StartGame
                    OnPlayerReady.Raise();
                    started = true;
                }
            }
            yield return new WaitForSeconds(.25f);
        }
    }

    private void Update()
    {
        if (playerList.Count >= minPlayerCount)
        {
            //StartGame
        }
    }
}
