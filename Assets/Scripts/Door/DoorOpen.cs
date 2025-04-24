using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private Animator anim;

    /// <summary>
    /// Se ejecuta cuando el objeto entra en contacto con otro objeto. Se utiliza para abrir la puerta si el objeto que entra es el jugador.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");

        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("Open", true);
        }
    }
}