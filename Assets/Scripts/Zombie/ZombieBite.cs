using UnityEngine;
using Unity.Netcode;

public class ZombieBite : NetworkBehaviour
{
    public const int PLAYER_DAMAGE = 10;
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(IsServer){
                other.gameObject.GetComponent<PlayerManager>().ApplyDamageRpc(PLAYER_DAMAGE);
            }
        }
    }
    
}
