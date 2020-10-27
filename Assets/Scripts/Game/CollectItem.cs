using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject shield;
    public GameObject rocket;
    public GameObject exShotSpawn1;
    public GameObject exShotSpawn2;


    public float validExtraShotSpawnDuration;
    public float fireRateIncreasedUnit;
    public float rocketSpawnZ;
    public float rocketSpawnRangeX;
    public float rocketCapacity;

    private float startValidItemMaker;

    private bool isExtraShotSpawn = false;

    private void Start()
    {
        setExtraShotSpawn(false);
        //Instantiate(shield, transform.position, transform.rotation);
    }
    public void openShield(bool attachToPlayer = true)
    {
        if(GameObject.FindWithTag("Shield") != null)
        {
            Destroy(GameObject.FindWithTag("Shield"));
        }
        if (attachToPlayer)
            Instantiate(shield, transform.position, transform.rotation).transform.parent = transform;
        else
            Instantiate(shield, transform.position, transform.rotation);

    }
    public void callRescueRocket()
    {
        for (int i = 0; i < Random.Range(1, rocketCapacity); i++)
            Instantiate(rocket, new Vector3(Random.Range(-rocketSpawnRangeX, rocketSpawnRangeX), 0, rocketSpawnZ), Quaternion.identity);
    }

    public void setExtraShotSpawn(bool value)
    {
        isExtraShotSpawn = value;
        exShotSpawn1.SetActive(value);
        exShotSpawn2.SetActive(value);
    }
    public void Collect(int id)
    {
        switch (id)
        {
            case 0:
                GetComponent<PlayerController>().fireRate -= GetComponent<PlayerController>().fireRate * fireRateIncreasedUnit;
                //isIncreasedFireRate = true;
                //startValidItemMaker = Time.time;
                break;
            case 1:
                openShield();
                break;
            case 2:
                callRescueRocket();
                break;
            case 3:
                setExtraShotSpawn(true);
                startValidItemMaker = Time.time;
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isExtraShotSpawn)
        {
            if (Time.time - startValidItemMaker >= validExtraShotSpawnDuration)
            {
                setExtraShotSpawn(false);
            }

        }
    }
}
