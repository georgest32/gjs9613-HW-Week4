using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackASphere : MonoBehaviour
{
    private Vector3 clickPos;

    public Camera cam;

    public float speed = 5000;
    
    public GameObject munition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject instaMunition = Instantiate(munition, transform.position, Quaternion.identity) as GameObject;
            Rigidbody rb = instaMunition.GetComponent<Rigidbody>();
            rb.AddForce(cam.transform.forward * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
