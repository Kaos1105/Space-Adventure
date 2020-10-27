using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrackEnemy : MonoBehaviour
{
    public float speed;

    private GameObject trackedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        trackedEnemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> enemys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        enemys = enemys.Where(e => e.layer == LayerMask.NameToLayer("Enemy")).ToList();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (enemys.Count == 0)
        {
            rb.velocity = new Vector3(0, 0, speed);
            return;
        } 
        if (trackedEnemy == null || trackedEnemy.activeInHierarchy == false)
            trackedEnemy = enemys[Random.Range(0, enemys.Count)];
        this.transform.LookAt(trackedEnemy.transform);

       
        rb.velocity = transform.forward * speed;
    }
}
