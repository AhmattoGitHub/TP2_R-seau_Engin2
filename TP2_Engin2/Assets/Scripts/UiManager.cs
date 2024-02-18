using UnityEngine;
using Mirror;

public class UiManager : NetworkBehaviour
{
    [SerializeField] private GameObject m_victoryScreen;
    [SerializeField] private GameObject m_defeatScreen;

    [ClientRpc]
    public void RPC_EnableVictoryScreen()
    {        
        m_victoryScreen.SetActive(true);
    }

    [ClientRpc]
    public void RPC_EnableDefeatScreen()
    {        
        m_defeatScreen.SetActive(true);
    }

}
