using UnityEngine;
using Unity.Netcode;

public class ZombieDamage : NetworkBehaviour
{
    const int HITS_TO_DIE = 3;
    int hitCount;

    private GameObject zombieManager;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        zombieManager = GameObject.Find("ZombieSpawner");
    }

    void OnCollisionEnter(Collision other)
    {
        if(IsServer){
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
