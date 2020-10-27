using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedByPlayer : MonoBehaviour
{
    public int id;

    private bool isComsumed = false;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isComsumed)
            return;
        other.GetComponent<CollectItem>().Collect(id);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        isComsumed = true;
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 2.5f); 
    }

}
