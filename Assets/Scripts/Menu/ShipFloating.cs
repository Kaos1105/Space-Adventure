using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFloating : MonoBehaviour
{
    public float floatingValue;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = startPosition + Vector3.up * Mathf.Cos(Time.time) * floatingValue;
    }
}
