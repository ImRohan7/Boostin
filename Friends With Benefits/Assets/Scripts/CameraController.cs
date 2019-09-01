using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void TriggerShake(float magnitude, float duration)
    {
        if(shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake(magnitude, duration));
    }

    public IEnumerator Shake(float magnitude, float duration)
    {
        float timer = duration;

        while(timer > 0)
        {
            timer -= Time.deltaTime;

            float randX = Random.Range(-1, 1) * magnitude;
            float randY = Random.Range(-1, 1) * magnitude;

            transform.position = new Vector3(randX, randY, 0f);

            yield return null;
        }

        transform.position = Vector3.zero;
    }
}
