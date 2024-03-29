﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    public GameObject countDown_3;
    public GameObject countDown_2;
    public GameObject countDown_1;

    public Sprite sprite_MVB;

    [SerializeField]
    private float waittime;

    private void Awake()
    {
        Instance = this;
    }

    // display countdown images
    public void showCountDown()
    {
        StartCoroutine(countDown());

    }

    IEnumerator countDown()
    {

        yield return new WaitForSeconds(1f);

        countDown_3.gameObject.SetActive(true);
        yield return new WaitForSeconds(waittime);

        countDown_3.gameObject.SetActive(false);
        countDown_2.gameObject.SetActive(true);
        yield return new WaitForSeconds(waittime);

        countDown_2.gameObject.SetActive(false);
        countDown_1.gameObject.SetActive(true);
        yield return new WaitForSeconds(waittime);

        countDown_1.gameObject.SetActive(false);
       
    }

}
