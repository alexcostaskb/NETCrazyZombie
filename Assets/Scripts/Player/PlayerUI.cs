using UnityEngine;
using Unity.Netcode;

public class PlayerUI : NetworkBehaviour
{
    public GameObject playerUI; // Referencia al Canvas UI del jugador

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            playerUI.SetActive(false); // Desactiva la UI para otros jugadores
        }
        else
        {
            playerUI.SetActive(true); // Activa la UI solo para el dueño
        }
    }
}
