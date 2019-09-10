using UnityEngine;

public class WrappableObject : MonoBehaviour
{
    private float leftConstraint, rightConstraint, topConstraint, bottomConstraint;
    private Camera cam;
    private float distanceZ;

    public SpriteRenderer spriteRenderer;
    private GameObject[] dummyObjects;

    private GameObject dummyParent;

    public void Start()
    {
        cam = Camera.main;
        distanceZ = Mathf.Abs(cam.transform.position.z + transform.position.z);
        leftConstraint = cam.ScreenToWorldPoint(new Vector3(0f + 190, 0f, distanceZ)).x;
        rightConstraint = cam.ScreenToWorldPoint(new Vector3(Screen.width - 190, 0f, distanceZ)).x;
        //Debug.Log("Width: " + Screen.width);
        bottomConstraint = cam.ScreenToWorldPoint(new Vector3(0f, 0f, distanceZ)).y;
        topConstraint = cam.ScreenToWorldPoint(new Vector3(0f, Screen.height, distanceZ)).y;

        dummyParent = new GameObject("DummyParent");
        dummyParent.transform.parent = transform;

        dummyObjects = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            dummyObjects[i] = new GameObject("WrapDummy");
            dummyObjects[i].transform.parent = dummyParent.transform;
            dummyObjects[i].AddComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
        }
        SetDummyPositions();
    }

    public void Update()
    {
        CheckForWrap();
        SetDummyPositions();
    }

    private void CheckForWrap()
    {
        if(transform.position.x < leftConstraint)
        {
            TriggerWrap();
            Debug.Log("Left Wrap");
            transform.position = new Vector3(rightConstraint, transform.position.y, 0f);
        }
        else if (transform.position.x > rightConstraint)
        {
            TriggerWrap();
            Debug.Log("Right Wrap");
            transform.position = new Vector3(leftConstraint, transform.position.y, 0f);
        }
        else if (transform.position.y > topConstraint)
        {
            TriggerWrap();
            transform.position = new Vector3(transform.position.x, bottomConstraint, 0f);
        }
        else if(transform.position.y < bottomConstraint)
        {
            TriggerWrap();
            transform.position = new Vector3(transform.position.x, topConstraint, 0f);
        }
    }

    public virtual void TriggerWrap(){ } //override in child classes if wrap events are needed

    private void SetDummyPositions()
    {
        dummyObjects[0].transform.position = new Vector3(transform.position.x, transform.position.y + topConstraint * 2, 0f);
        dummyObjects[1].transform.position = new Vector3(transform.position.x, transform.position.y + bottomConstraint * 2, 0f);
        dummyObjects[2].transform.position = new Vector3(transform.position.x + rightConstraint * 2, transform.position.y, 0f);
        dummyObjects[3].transform.position = new Vector3(transform.position.x + leftConstraint * 2, transform.position.y, 0f);
    }
}
