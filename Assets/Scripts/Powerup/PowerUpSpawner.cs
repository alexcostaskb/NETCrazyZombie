using Unity.Netcode;
using UnityEngine;

public class PowerUpSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject prefab;

    [SerializeField] private Transform[] spawnPoints;

    [Header("Settings")]
    [SerializeField] private float delay;

    private GameObject powerUp;

    /// <summary>
    /// Cuando el objeto se genera en la red, se inicia la invocación repetida del método SpawnRpc.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        InvokeRepeating(nameof(SpawnRpc), 2f, delay);
    }

    /// <summary>
    /// Genera un objeto en una posición aleatoria de los puntos de generación.
    /// </summary>
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