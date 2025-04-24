using Unity.Netcode;
using UnityEngine;

public class ZombieBite : NetworkBehaviour
{
    public const int PLAYER_DAMAGE = 10;

    /// <summary>
    /// Cuando el zombie colisiona con el jugador, se aplica daño al jugador.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsServer)
            {
                other.gameObject.GetComponent<PlayerManager>().ApplyDamageRpc(PLAYER_DAMAGE);
            }
        }
    }
}