using System.Collections;
using UnityEngine;

public class RangedExplosionParticle : MonoBehaviour
{

    private void Awake()
    {
        DestroyObject();
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
