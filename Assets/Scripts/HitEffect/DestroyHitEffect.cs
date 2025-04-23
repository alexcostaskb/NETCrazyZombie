using UnityEngine;

public class DestroyHitEffect : MonoBehaviour
{
    const float TIME = 1;
    float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TIME)
        {
            Destroy(gameObject);
        }
    }
}
