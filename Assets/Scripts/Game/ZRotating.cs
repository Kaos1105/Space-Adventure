using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZRotating : MonoBehaviour
{
    public float tumble; 

    // Start is called before the first frame update
 
    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, tumble * 1.0f);
     
    }

    // Update is called once per frame
    void Update()
    { 
    }
}
