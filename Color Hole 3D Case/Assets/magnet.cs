using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class magnet : MonoBehaviour
{

    Transform coinDetectorObj;

    // Start is called before the first frame update
    void Start()
    {
        coinDetectorObj = GameObject.FindGameObjectWithTag("CoinDetector").transform;
    }
    private void Update()
    {
        if (Math.Abs(coinDetectorObj.position.z - transform.position.z) < 1.2)
        {
            //x alan kontrol
            if (Math.Abs(coinDetectorObj.position.x - transform.position.x) < 1.2)
            {
                //spherelerin kipirdamamasi icin
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                transform.position = Vector3.MoveTowards(transform.position, coinDetectorObj.position,
            5f * Time.deltaTime);
            }
        }
    }
}
