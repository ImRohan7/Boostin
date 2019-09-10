using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLava : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(-0.25f, 0.5f, 10.0f));
        gameObject.transform.position = v3Pos;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
