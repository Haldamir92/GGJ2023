using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PlayerLobbyText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI lobbyText;

    [SerializeField]
    private GameObjectValueList playerList;

    [SerializeField]
    private LobbyManager lobby;

    // Update is called once per frame
    void Update()
    {
        lobbyText.text = $"Waiting for players: {playerList.Count}/{lobby.minPlayerCount}";
    }
}
