using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;

public class ZombieMove : NetworkBehaviour
{
    Transform target;

    NavMeshAgent agent;
    
    public override void OnNetworkSpawn()
    {
        if(IsServer){
            agent = GetComponent<NavMeshAgent>();
            Invoke("FindTarget",3);
        }
    }
    
    void Update()
    {
        if (!IsServer) return; // Solo el servidor controla el movimiento del enemigo

        FindTarget();

        if (!agent.isStopped && target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void FindTarget()
    {
        target = GetNearestPlayer();
    }

    private Transform GetNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform nearestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            if(CanReachTarget(player.transform.position)){
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlayer = player.transform;
                }
            } 
        }

        return nearestPlayer;
    }

    bool CanReachTarget(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(targetPosition, path))
        {
            Debug.Log("path.status: " + path.status);
            return path.status == NavMeshPathStatus.PathComplete; // Only returns true if path is fully reachable
        }
        return false;
    }
}
