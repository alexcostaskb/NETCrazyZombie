using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]

    [Header("Settings")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    Rigidbody rb;
    CapsuleCollider col;

    public override void  OnNetworkSpawn()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {  
        if(!IsOwner) return;

        // desplazamiento del jugador
        Vector2 moveInput = Vector2.zero;
        moveInput.x = Input.GetAxis("Horizontal") * speed;  // desplazamiento lateral (eje X)
        moveInput.y = Input.GetAxis("Vertical") * speed;    // desplazamiento frontal (eje Z)
        moveInput *= Time.deltaTime;                        // ajustar la velocidad al frame rate

        TranslateRpc(moveInput);

        // salto del jugador
        if (Input.GetButtonDown("Jump"))
        {
            JumpRpc();        
        }     

        // liberar el cursor al presionar la tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    [Rpc(SendTo.Server)]
    void TranslateRpc(Vector2 moveInput)
    {
        transform.Translate(moveInput.x, 0, moveInput.y);
    }

    [Rpc(SendTo.Server)]
    void JumpRpc()
    {
        if(IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        // raycast para detectar si el jugador está tocando el suelo
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }
}
