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
    internal int minPlayerCount; 
    
    [SerializeField]
    private List<Transform> playerSpawns = new();

    [SerializeField]
    private List<RuntimeAnimatorController> heads = new();

    [SerializeField]
    private List<Color> playerColor = new();


    private bool started = false;

    private void Start()
    {
        StartCoroutine(CheckPlayers());
    }

    public void OnPlayerJoin(PlayerInput input)
    {
        var player = input.gameObject;
        playerList.Add(player);

        player.transform.position = playerSpawns[input.user.index].position;
        player.GetComponentInChildren<Animator>().runtimeAnimatorController = heads[input.user.index];
        Color myColor = playerColor[input.user.index];
        player.GetComponentInChildren<SpriteRenderer>().color = myColor;
        player.GetComponent<PrefabSpawner>().customColor = myColor;
        player.GetComponent<SingleSpawner>().playerColor = myColor;
    }

    public void OnPlayerLeave(PlayerInput input)
    {
        playerList.Remove(input.gameObject);
    }

    IEnumerator CheckPlayers()
    {
        while(!started)
        {
            if (playerList.Count() == minPlayerCount)
            {
                OnPlayerReady.Raise();
                started = true;
            }
            yield return new WaitForSeconds(.25f);
        }
    }
}
