using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnim : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    private NavMeshAgent agent;

    /// <summary>
    /// Cuando el zombie se genera en la red, se inicia la animación de correr y se obtiene el componente NavMeshAgent.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        anim.SetBool("IsRunning", true);
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Cuando el zombie colisiona con el jugador, se detiene el movimiento del agente y se inicia la animación de ataque.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (agent != null && !agent.isStopped)
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
            }

            anim.SetBool("IsAttacking", true);
        }
    }

    /// <summary>
    /// Cuando el zombie deja de colisionar con el jugador, se detiene la animación de ataque y se reanuda el movimiento del agente.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("IsAttacking", false);

            Invoke("ResumeAgent", 3f);
        }
    }

    /// <summary>
    /// Reanuda el movimiento del agente después de un tiempo de espera.
    /// </summary>
    private void ResumeAgent()
    {
        agent.isStopped = false;
    }
}