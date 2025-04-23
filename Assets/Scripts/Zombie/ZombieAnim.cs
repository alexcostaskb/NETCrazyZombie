using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
public class ZombieAnim : NetworkBehaviour
{
    [SerializeField] Animator anim;
    NavMeshAgent agent;

    public override void OnNetworkSpawn()
    {
        anim.SetBool("IsRunning", true);
        agent = GetComponent<NavMeshAgent>();
    }

    void OnCollisionEnter(Collision collision)
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

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("IsAttacking", false);

            Invoke("ResumeAgent", 3f);
        }
    }

    void ResumeAgent()
    {
        agent.isStopped = false;
    }

}
