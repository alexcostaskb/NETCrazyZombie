using Unity.Netcode;
using UnityEngine;

public class CameraOwnerComponentManager : NetworkBehaviour
{
    [SerializeField] private Camera _camera; // This is your camera, assign it in the prefab
    [SerializeField] private AudioListener _audioListener; // This is your listener, assign it in the prefab

    /// <summary>
    /// Se ejecuta cuando el objeto se genera en la red. Se utiliza para inicializar la cámara y el audio.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { return; } // ALL players will read this method, only player owner will execute past this line
        _camera.enabled = true; // only enable YOUR PLAYER'S camera, all others will stay disabled
        _audioListener.enabled = true;
    }
}