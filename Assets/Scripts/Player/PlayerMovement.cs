using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [Header("Settings")]
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;

    private Rigidbody rb;
    private CapsuleCollider col;

    /// <summary>
    /// Se ejecuta cuando el objeto se genera en la red. Se utiliza para inicializar el Rigidbody y el CapsuleCollider.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    /// <summary>
    /// Método Update se ejecuta una vez por frame. Se utiliza para manejar la entrada del jugador y el movimiento.
    /// </summary>
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

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

    /// <summary>
    /// Se encarga de mover al jugador en la red. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="moveInput"></param>

    [Rpc(SendTo.Server)]
    private void TranslateRpc(Vector2 moveInput)
    {
        transform.Translate(moveInput.x, 0, moveInput.y);
    }

    /// <summary>
    /// Se encarga de saltar al jugador en la red. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>

    [Rpc(SendTo.Server)]
    private void JumpRpc()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Verifica si el jugador está en el suelo. Se utiliza un raycast para detectar la colisión con el suelo.
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        // raycast para detectar si el jugador está tocando el suelo
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }
}