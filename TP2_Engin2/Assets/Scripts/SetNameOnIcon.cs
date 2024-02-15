using Mirror;
using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SetNameOnIcon : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text m_name;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetNameOnServer();
    }

    [Command(requiresAuthority =false)]
    private void SetNameOnServer()
    {
        foreach (var player in NetManagerCustom._Instance.m_players)
        {
            if (player.identity.isLocalPlayer)
            {
                m_name.text = player.m_name;
                SetNameOnClients(m_name.text);
                return;
            }
        }
    }

    [ClientRpc]
    private void SetNameOnClients(string playerName)
    {
        m_name.text = playerName;
    }
}
