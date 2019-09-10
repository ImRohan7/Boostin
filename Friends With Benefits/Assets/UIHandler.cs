using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    public GameObject countDown_3;
    public GameObject countDown_2;
    public GameObject countDown_1;
    public GameObject countDown_GO;

    [SerializeField]
    private float waittime;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // display countdown images
    public void showCountDown()
    {
        StartCoroutine(countDown());

    }

    IEnumerator countDown()
    {
        countDown_3.gameObject.SetActive(true);
        yield return new WaitForSeconds(waittime);

        countDown_3.gameObject.SetActive(false);
        countDown_2.gameObject.SetActive(true);
        yield return new WaitForSeconds(waittime);

        countDown_2.gameObject.SetActive(false);
        countDown_1.gameObject.SetActive(true);
        yield return new WaitForSeconds(waittime);

        countDown_1.gameObject.SetActive(false);
        countDown_GO.gameObject.SetActive(true);
        yield return new WaitForSeconds(waittime-0.3f);

        countDown_GO.gameObject.SetActive(false);

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
