using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeAmount;
    [SerializeField]
    private float shakeLength;

    private Camera mainCamera;
    private Vector3 camPosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        camPosition = gameObject.transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            InitShake();
        }
        print(camPosition);
    }

    public void InitShake()
    {
        InvokeRepeating("StartShake", 0f, 0.01f);
        Invoke("StopShake", shakeLength);
    }
    private void StartShake()
    {
        if (shakeAmount != 0)
        {
            camPosition = gameObject.transform.position;

            float shakeX = (Random.value * shakeAmount * 2) - shakeAmount;
            float shakeY = (Random.value * shakeAmount * 2) - shakeAmount;
            camPosition.x += shakeX;
            camPosition.y += shakeY;

            gameObject.transform.position = camPosition;
        }
    }

    private void StopShake()
    {
        CancelInvoke("StartShake");
        gameObject.transform.position = new Vector3(0,0,-10);
    }

}
