using Unity.Netcode;
using UnityEngine;

public class CameraMouseLook : NetworkBehaviour
{
    private const float CLAMP_MIN = -45.0f;
    private const float CLAMP_MAX = 45.0f;

    [SerializeField] private float lookSensitivity;
    private Vector2 rotation = Vector2.zero;
    private Vector2 smoothRot = Vector2.zero;
    private Vector2 velRot = Vector2.zero;

    private GameObject player;

    public override void OnNetworkSpawn()
    {
        player = transform.parent.gameObject;
    }

    /// <summary>
    /// Se ejecuta una vez por frame. Se utiliza para manejar la entrada del jugador y la rotación de la cámara.
    /// </summary>
    private void Update()
    {
        if (IsOwner)
        {
            float axis_x = Input.GetAxis("Mouse X");
            // giro up/down de la cámara/caberza
            rotation.y += Input.GetAxis("Mouse Y");
            rotation.y = Mathf.Clamp(rotation.y, CLAMP_MIN, CLAMP_MAX);
            smoothRot.y = Mathf.SmoothDamp(smoothRot.y, rotation.y, ref velRot.y, 0.1f);

            LookAroundRpc(smoothRot, axis_x);
        }
    }

    /// <summary>
    /// Se encarga de rotar la cámara y el jugador en la red. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="smoothRot"></param>
    /// <param name="axis_x"></param>
    [Rpc(SendTo.Server)]
    private void LookAroundRpc(Vector2 smoothRot, float axis_x)
    {
        transform.localEulerAngles = new Vector3(-smoothRot.y, 0, 0);
        player.transform.RotateAround(transform.position, Vector3.up, axis_x * lookSensitivity);
    }
}