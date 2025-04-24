using Unity.Netcode;
using UnityEngine;

public class PlayerFireBullet : NetworkBehaviour
{
    [SerializeField] private GameObject proyectile;

    /// <summary>
    /// Se ejecuta cuando el objeto se genera en la red. Se utiliza para inicializar el proyectil.
    /// </summary>
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FireRpc();
        }
    }

    /// <summary>
    /// Se encarga de instanciar el proyectil en la red. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    [Rpc(SendTo.Server)]
    private void FireRpc()
    {
        GameObject bala = Instantiate(proyectile, transform.position, transform.rotation);
        bala.GetComponent<NetworkObject>().Spawn(true);
    }
}