using System.Collections;
using UnityEngine;

public class LifetimeLimiter : MonoBehaviour
{
    public float Lifetime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CountdownDestroy());
    }

    public IEnumerator CountdownDestroy()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}
