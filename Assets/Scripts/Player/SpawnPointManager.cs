using UnityEngine;
using UnityEngine.AI;

public class SpawnPointManager : MonoBehaviour
{
    /// <summary>
    /// Genera un punto de generación aleatorio en el mapa utilizando NavMesh.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomSpawnPoint()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(new Vector3(0, 0, 0), out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}