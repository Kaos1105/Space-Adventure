using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;

    public Transform shotSpawn;
    public GameObject shot;
    public float fireRate;
    public float delay;

    void Start()
    {
        fireRate = Random.Range(1.0F, 2.5F);
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire", delay, fireRate);
    }
     
    void Fire()
    {
        Instantiate(shot, shotSpawn.position,shotSpawn.rotation);
        audioSource.Play();
        fireRate = Random.Range(1.0F, 2.5F);
    }
}
