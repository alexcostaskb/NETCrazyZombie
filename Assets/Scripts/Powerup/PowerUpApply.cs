using Unity.Netcode;
using UnityEngine;

public class PowerUpApply : NetworkBehaviour
{
    private const int POWER = 50;

    [SerializeField] private AudioClip clip;

    /// <summary>
    /// Cuando el objeto colisiona con el jugador, se aplica daño negativo al jugador y se destruye el objeto.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.SendMessage("ApplyDamage", -POWER);

                AudioSource.PlayClipAtPoint(clip, transform.position);

                GetComponent<NetworkObject>().Despawn();
                Destroy(gameObject);
            }
        }
    }
}