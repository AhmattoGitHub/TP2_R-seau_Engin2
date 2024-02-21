using Mirror;
using TMPro;
using UnityEngine;

public class SetNameOnIcon : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text m_name;

    void Start()
    {
        DisplayIconName();
    }

    void Update()
    {

    }
    private void DisplayIconName()
    {
        m_name.text = connectionToClient.m_name;
        CMDDisplayIconName(connectionToClient.m_name);
    }

    [Command(requiresAuthority =false)]
    private void CMDDisplayIconName(string name)
    {
        m_name.text = name;
        RPCDisplayIconName(name);
    }

    [ClientRpc]
    private void RPCDisplayIconName(string name)
    {
        m_name.text = name;
    }
}
