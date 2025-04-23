using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.Networking.Transport.Error;

public class SpawnPointManager: MonoBehaviour
{
    public Vector3 GetRandomSpawnPoint()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(new Vector3(0,0,0), out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else{
            return Vector3.zero;
        }
    }
}
