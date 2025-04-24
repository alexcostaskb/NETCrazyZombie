using Unity.Netcode;
using UnityEngine;

public class BulletMove : NetworkBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 5f;

    public int PLAYER_DAMAGE = 10;

    /// <summary>
    /// Se ejecuta al inicio del juego. Se utiliza para inicializar la bala y su tiempo de vida.
    /// </summary>
    private void Start()
    {
        if (IsServer) // Solo el servidor controla la destrucción
        {
            Invoke(nameof(DestroyBulletRpc), lifeTime);
        }
    }

    /// <summary>
    /// Se ejecuta una vez por frame. Se utiliza para mover la bala en la dirección hacia adelante.
    /// </summary>
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    [Rpc(SendTo.Server)]
    private void DestroyBulletRpc()
    {
        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn(); // Despawner en la red
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Se encarga de manejar la colisión de la bala con el jugador. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.SendMessage("ApplyDamage", PLAYER_DAMAGE);
                DestroyBulletRpc();
            }
        }
    }
}