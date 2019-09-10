using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float current = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(0, 0, current + speed);
        //Quaternion rotation = transform.rotation;
        //Quaternion k = rotation;
        //k.Set(k.x, k.y, k.z + Time.deltaTime * speed, k.w);
        //transform.rotation = k;
        //Debug.Log(k.z);
        // transform.Rotate(Vector3.left * Time.deltaTime * speed);
    }
}
