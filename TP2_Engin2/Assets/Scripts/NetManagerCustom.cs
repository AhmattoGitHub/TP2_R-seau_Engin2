using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManagerCustom : NetworkManager
{
    public GameObject levelPlayerPrefab;
    public GameObject characterPlayerPrefab;
    public bool spawnRunner = true;

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        //var go = Instantiate(characterPlayerPrefab);
        //NetworkServer.Spawn(go, conn);
        if (spawnRunner)
        {
            OnServerAddPlayer(conn, characterPlayerPrefab);
        }
        else
        {
            OnServerAddPlayer(conn, levelPlayerPrefab);
        }
    }

    public override void OnValidate()
    {
        base.OnValidate();
        
        if (levelPlayerPrefab != null && !levelPlayerPrefab.TryGetComponent(out NetworkIdentity _))
        {
            Debug.LogError("NetworkManager - Player Prefab must have a NetworkIdentity.");
            levelPlayerPrefab = null;
        }
        if (characterPlayerPrefab != null && !characterPlayerPrefab.TryGetComponent(out NetworkIdentity _))
        {
            Debug.LogError("NetworkManager - Player Prefab must have a NetworkIdentity.");
            characterPlayerPrefab = null;
        }
        
        if (levelPlayerPrefab != null && spawnPrefabs.Contains(levelPlayerPrefab))
        {
            Debug.LogWarning("NetworkManager - Player Prefab doesn't need to be in Spawnable Prefabs list too. Removing it.");
            spawnPrefabs.Remove(levelPlayerPrefab);
        }
        if (characterPlayerPrefab != null && spawnPrefabs.Contains(characterPlayerPrefab))
        {
            Debug.LogWarning("NetworkManager - Player Prefab doesn't need to be in Spawnable Prefabs list too. Removing it.");
            spawnPrefabs.Remove(characterPlayerPrefab);
        }
    }

    public override void RegisterClientMessages()
    {
        base.RegisterClientMessages();

        if (levelPlayerPrefab != null)
            NetworkClient.RegisterPrefab(levelPlayerPrefab);
        if (characterPlayerPrefab != null)
            NetworkClient.RegisterPrefab(characterPlayerPrefab);
    }

    //public override void OnServerAddPlayerInternal(NetworkConnectionToClient conn, AddPlayerMessage msg)
    //{
    //        Debug.Log("AddPlayerInternal");
    //    
    //    base.OnServerAddPlayerInternal(conn, msg);
    //
    //    if (conn.identity != null)
    //    {
    //        Debug.LogError("There is already a player for this connection.");
    //        return;
    //    }
    //
    //    if (spawnRunner)
    //    {
    //        if (autoCreatePlayer && characterPlayerPrefab == null)
    //        {
    //            Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
    //            return;
    //        }
    //
    //        if (autoCreatePlayer && !characterPlayerPrefab.TryGetComponent(out NetworkIdentity _))
    //        {
    //            Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab.");
    //            return;
    //        }
    //
    //        OnServerAddPlayer(conn, characterPlayerPrefab);
    //    }
    //    else
    //    {
    //        if (autoCreatePlayer && levelPlayerPrefab == null)
    //        {
    //            Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
    //            return;
    //        }
    //
    //        if (autoCreatePlayer && !levelPlayerPrefab.TryGetComponent(out NetworkIdentity _))
    //        {
    //            Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab.");
    //            return;
    //        }
    //        
    //        OnServerAddPlayer(conn, levelPlayerPrefab);
    //    }
    //
    //
    //
    //}
    
}
