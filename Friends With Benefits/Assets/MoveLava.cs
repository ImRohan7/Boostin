using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLava : MonoBehaviour
{
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private float speed;

    public bool right;
    public bool up;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = Camera.main.ViewportToWorldPoint(position);
        //gameObject.transform.position = pos;

        up = false;
        right = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() !=null)
        {
            collision.gameObject.GetComponent<PlayerController>().TriggerDeath();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(right == true)
        {
            if (gameObject.tag == "Right")
            {
                transform.position += transform.right * speed * Time.deltaTime;
            }
            if (gameObject.tag == "Left")
            {
                transform.position += transform.right * speed * Time.deltaTime;
            }
        }
        else if(up == true)
        {
            if (gameObject.tag == "Up")
            {
                transform.position += -transform.up * speed * Time.deltaTime;
            }
            if (gameObject.tag == "Down")
            {
                transform.position += transform.up * speed * Time.deltaTime;
            }
        }

    }
}
