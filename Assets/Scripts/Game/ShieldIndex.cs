using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldIndex : MonoBehaviour
{
    public bool isImmortal = true;
    public float immortalTime;

    private float awakeTime;
    // Start is called before the first frame update
    void Start()
    {
        isImmortal = true;
        awakeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - awakeTime) > immortalTime && isImmortal)
            isImmortal = false;
    }
}
