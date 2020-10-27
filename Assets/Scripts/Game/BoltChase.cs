using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltChase : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    private void Start()
    { 
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && player.transform.position.z < transform.position.z)
        {
            this.transform.LookAt(player.transform);
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = this.transform.forward * speed;
        }
        else
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = this.transform.forward * speed;
        }
    }
    void Update()
    {
    }
}
