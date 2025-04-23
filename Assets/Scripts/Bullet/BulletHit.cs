using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [SerializeField] GameObject particle;

    void OnCollisionEnter(Collision collision)
    {
        // Instanciamos el efecto de impacto
        Instantiate(particle, transform.position, Quaternion.identity);
        
        // Destruimos el proyectil
        gameObject.SetActive(false);
    }
}
