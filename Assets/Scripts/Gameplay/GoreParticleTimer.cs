using UnityEngine;

public class GoreParticleTimer : MonoBehaviour
{
    [SerializeField] private float desTime = 0.1f;

    void Update()
    {
        Destroy(gameObject, desTime);
    }
}
