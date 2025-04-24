using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMove : NetworkBehaviour
{
    private Transform target;

    private NavMeshAgent agent;

    /// <summary>
    /// Cuando el zombie se genera en la red, se inicializa el agente de navegación y se busca el objetivo.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            agent = GetComponent<NavMeshAgent>();
            Invoke("FindTarget", 3);
        }
    }

    /// <summary>
    /// Actualiza la posición del zombie hacia el objetivo más cercano (jugador) cada frame.
    /// </summary>
    private void Update()
    {
        if (!IsServer)
        {
            return; // Solo el servidor controla el movimiento del enemigo
        }

        FindTarget();

        if (!agent.isStopped && target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    /// <summary>
    /// Busca el objetivo más cercano (jugador) y lo asigna a la variable target.
    /// </summary>
    private void FindTarget()
    {
        target = GetNearestPlayer();
    }

    /// <summary>
    /// Busca el jugador más cercano y lo devuelve como objetivo.
    /// </summary>
    /// <returns></returns>
    private Transform GetNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform nearestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            if (CanReachTarget(player.transform.position))
            {
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

    /// <summary>
    /// Verifica si el zombie puede alcanzar la posición del objetivo utilizando el NavMeshAgent.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private bool CanReachTarget(Vector3 targetPosition)
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