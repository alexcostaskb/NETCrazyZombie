using UnityEngine;

public class DestroyHitEffect : MonoBehaviour
{
    private const float TIME = 1;
    private float timer;

    /// <summary>
    /// Se ejecuta una vez por frame. Se utiliza para destruir el objeto después de un tiempo determinado.
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= TIME)
        {
            Destroy(gameObject);
        }
    }
}