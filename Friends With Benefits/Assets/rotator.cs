using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Update is called once per frame
    void Update()
    {
        float current = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(0, 0, current + speed);
    }
}
