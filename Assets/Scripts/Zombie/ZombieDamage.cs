using Unity.Netcode;
using UnityEngine;

public class ZombieDamage : NetworkBehaviour
{
    private const int HITS_TO_DIE = 3;
    private int hitCount;

    private GameObject zombieManager;
    
    /// <summary>
    /// Cuando el zombie se genera en la red, se inicializa el contador de impactos y se busca el objeto ZombieSpawner.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        zombieManager = GameObject.Find("ZombieSpawner");
    }

    /// <summary>
    /// Cuando el zombie colisiona con una bala, se incrementa el contador de impactos.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                hitCount++;

                if (hitCount == HITS_TO_DIE)
                {
                    zombieManager.GetComponent<ZombieSpawner>().DestroyZombieRpc(gameObject.GetComponent<NetworkObject>());
                }
            }
        }
    }
}