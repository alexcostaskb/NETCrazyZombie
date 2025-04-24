using Unity.Netcode;
using UnityEngine;

public class ZombieSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject zombie;

    [Header("Settings")]
    [SerializeField] private float spawnDelay;

    [SerializeField] private int zombieMax;

    private int numZombies = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InvokeRepeating(nameof(SpawnZombieRpc), 2f, spawnDelay);
        }
    }

    [Rpc(SendTo.Server)]
    public void DestroyZombieRpc(NetworkObjectReference networkObjectReference)
    {
        NetworkObject target = networkObjectReference;
        target.Despawn();
        Destroy(target.gameObject);
        numZombies--;
    }

    [Rpc(SendTo.Server)]
    private void SpawnZombieRpc()
    {
        if (!IsServer)
        {
            return;
        }

        if (numZombies < zombieMax)
        {
            GameObject enemy = Instantiate(zombie, transform.position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
            numZombies++;
        }
    }
}