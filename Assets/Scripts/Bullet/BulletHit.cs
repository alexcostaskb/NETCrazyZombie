using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    /// <summary>
    /// Se ejecuta cuando el objeto entra en contacto con otro objeto. Se utiliza para instanciar un efecto de impacto y destruir el proyectil.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // Instanciamos el efecto de impacto
        Instantiate(particle, transform.position, Quaternion.identity);

        // Destruimos el proyectil
        gameObject.SetActive(false);
    }
}