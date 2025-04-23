using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class PowerUpSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] GameObject prefab;
    [SerializeField] Transform[] spawnPoints;

    [Header("Settings")]
    [SerializeField] float delay;

    GameObject powerUp;

    public override void OnNetworkSpawn()
    {
        InvokeRepeating(nameof(SpawnRpc), 2f, delay);
    }

    [Rpc(SendTo.Server)]
    private void SpawnRpc()
    {
        if (IsServer && powerUp == null)
        {   
            Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;   
            powerUp = Instantiate(prefab, position, Quaternion.identity);
            powerUp.GetComponent<NetworkObject>().Spawn();
        }
    }
}
