using UnityEngine;
using Mirror;


public class BoundsDetection : NetworkBehaviour
{
    [SerializeField] private GameObject m_prefab = null;

    [SerializeField] private SpawnAfterFalling m_spawnAfterFalling = null;
    //private NetworkIdentity m_networkIdentity = null;
    [SerializeField] private float m_minRadius = 5f; // Rayon minimum
    [SerializeField] private float m_maxRadius = 10f;



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered vertical bounds!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        NetworkIdentity networkIdentity = other.transform.root.GetComponent<NetworkIdentity>();

        Debug.Log(networkIdentity.gameObject);

        if(networkIdentity.gameObject.name == "RunnerNetwork [connId=0]" 
            || networkIdentity.gameObject.name == "RunnerNetwork(Clone)")
        {



            Debug.Log("ENNNNTTRRREEEEESSSS");
        }





        Vector2 randomCirclePos = RandomPosBetweenTwoCircles(m_minRadius, m_maxRadius);
                
        Vector3 randomPosition = new Vector3(randomCirclePos.x, 10, randomCirclePos.y);

        Transform rootTransform = other.transform.root;
        rootTransform.position = randomPosition;




        //NetworkIdentity networkIdentity = other.transform.root.GetComponent<NetworkIdentity>();






        //Debug.Log("This the noetwork identity" + networkIdentity);
        //Debug.Log(m_prefab);


        //if (networkIdentity != null)
        //{
        //
        //
        //
        //    if (networkIdentity.gameObject == m_prefab)
        //    {
        //
        //        Debug.Log("entered yoooooooooooooo");
        //
        //        Debug.Log(networkIdentity.gameObject);
        //        Debug.Log(m_prefab);
        //
        //    }
        //
        //
        //
        //}



    }

    private Vector2 RandomPosBetweenTwoCircles(float minRadius, float maxRadius)
    {        
        float randomRadius = Random.Range(minRadius, maxRadius);

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
                
        float randomX = randomRadius * Mathf.Cos(randomAngle);
        float randomY = randomRadius * Mathf.Sin(randomAngle);
                
        return new Vector2(randomX, randomY);
    }


}
