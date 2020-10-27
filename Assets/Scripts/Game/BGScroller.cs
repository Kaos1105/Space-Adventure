using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float tileSizeZ;
    public float scrollSpeed;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // t - t/length*length, modulos, t<0, result + length
        float newPositionOffset = Mathf.Repeat(Time.time*scrollSpeed,tileSizeZ);
        transform.position = startPosition + Vector3.forward * newPositionOffset; 
        
    }
}
