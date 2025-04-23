using UnityEngine;
using Unity.Netcode;

public class PowerUpApply : NetworkBehaviour
{
    const int POWER = 50;

    [SerializeField] AudioClip clip;

    void OnTriggerEnter(Collider other)
    {
        if(IsServer){
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
