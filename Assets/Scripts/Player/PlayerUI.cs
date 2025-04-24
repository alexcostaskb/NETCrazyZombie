using Unity.Netcode;
using UnityEngine;

public class PlayerUI : NetworkBehaviour
{
    public GameObject playerUI; // Referencia al Canvas UI del jugador

    /// <summary>
    /// Cuando el objeto se genera en la red, se activa o desactiva la UI del jugador según si es el dueño o no.
    /// </summary>
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