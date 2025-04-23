using System;
using UnityEngine;
using Unity.Netcode;

public class CameraMouseLook : NetworkBehaviour
{
    const float CLAMP_MIN = -45.0f;
    const float CLAMP_MAX = 45.0f;

    [SerializeField] float lookSensitivity;
    Vector2 rotation = Vector2.zero;
    Vector2 smoothRot = Vector2.zero;
    Vector2 velRot = Vector2.zero;

    GameObject player;

    public override void OnNetworkSpawn()
    {
        player = transform.parent.gameObject;    
    }

    void Update()
    {
        if(IsOwner){
            float axis_x = Input.GetAxis("Mouse X");
            // giro up/down de la cámara/caberza
            rotation.y += Input.GetAxis("Mouse Y");
            rotation.y = Mathf.Clamp(rotation.y, CLAMP_MIN, CLAMP_MAX);
            smoothRot.y = Mathf.SmoothDamp(smoothRot.y, rotation.y, ref velRot.y, 0.1f);

            LookAroundRpc(smoothRot, axis_x);
        }

    }

    [Rpc(SendTo.Server)]
    void LookAroundRpc(Vector2 smoothRot, float axis_x)
    {
        transform.localEulerAngles = new Vector3(-smoothRot.y, 0, 0);
        player.transform.RotateAround(transform.position, Vector3.up, axis_x * lookSensitivity);
    }
}
