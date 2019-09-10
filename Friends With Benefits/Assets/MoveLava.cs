using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLava : MonoBehaviour
{
    [SerializeField]
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(-0.30f, 0.5f, 10.0f));
       Vector3 pos = Camera.main.ViewportToWorldPoint(position);
       gameObject.transform.position = pos;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * 1f * Time.deltaTime;
    }
}
