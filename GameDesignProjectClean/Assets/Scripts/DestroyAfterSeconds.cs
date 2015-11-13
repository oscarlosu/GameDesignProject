using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 5.0f;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}