using Coherence.Toolkit;
using System;
using System.Collections;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class NetworkLobby : MonoBehaviour
{
    //[SerializeField]
    //GameObjectValueList playerList;

    [SerializeField]
    int minPlayerToStart = 2;

    [SerializeField]
    private CoherenceMonoBridge bridge;

    [SerializeField]
    VoidEvent onAllPlayersReady;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckClientsReady());
    }
     

    IEnumerator CheckClientsReady()
    {
        while (true)
        {
            if (bridge.ClientConnections.ClientConnectionCount >= minPlayerToStart
                && bridge.ClientConnections.GetAll()
                    .Select(x => x.GameObject.GetComponent<NetworkedPlayerReady>())
                    .All(x => x.IsReady))
            {
                //Start game
                onAllPlayersReady.Raise();
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
