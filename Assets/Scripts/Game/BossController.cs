using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameObject player;
    public GameObject bossSpawn;
    public GameObject explosion;
    public GameObject boltExplosion;
    public GameObject playerExplosion;

    public Slider hpBar;

    public GameController gameController;

    public List<GameObject> bolts;
    public List<GameObject> shotSpawns;

    public Camera mainCam;

    public float smoothing;
    public float dodge;
    public float fireWait;
    public float fireRate;
    public float coinBonusRatio;
    public float hpRatio;
    public float baseHP;
    public float loseHPunit;
    public float tilt;

    public int fireMin;
    public int fireMax;

    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;


    private bool isRotating = false;
    private bool isLaunched = false;

    private float totalHP;
    private float currentHP;
    private float targetManeuver;
    private float lastFire;

    private Rigidbody rigidBody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bolt") || other.CompareTag("Rocket"))
        {
            Instantiate(boltExplosion, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            if (isLaunched)
                currentHP -= loseHPunit;
            UpdateHPBar();
        }
        if (other.CompareTag("Player"))
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            other.gameObject.SetActive(false);
            gameController.playerLose();
        }
        if (currentHP < 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            gameController.AddScore(Mathf.CeilToInt(gameController.score * coinBonusRatio));
            Destroy(gameObject);
            hpBar.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update 
    void Start()
    {
        bossSpawn = GameObject.FindWithTag("BossSpawn");
        BossSpawnConfig config = bossSpawn.GetComponent<BossSpawnConfig>();
        hpBar = config.hpBar;
        mainCam = config.mainCam;
        gameController = config.gameController;
        player = config.player;

        rigidBody = GetComponent<Rigidbody>();
        transform.LookAt(bossSpawn.transform);
        rigidBody.velocity = transform.forward * 4.5f;
        StartCoroutine(Evade());
        StartCoroutine(Fire());
        hpBar.gameObject.SetActive(true);
        lastFire = Time.time;
        totalHP = gameController.score * hpRatio + baseHP;
        currentHP = totalHP;
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        hpBar.value = currentHP / totalHP;
    }

    IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireWait);
            for (int i = 0; i < Random.Range(fireMin, fireMax); i++)
            {
                yield return new WaitForSeconds(fireRate);
                foreach (var shotSpawn in shotSpawns)
                {
                    yield return new WaitForSeconds(fireRate);
                    Instantiate(bolts[Random.Range(0, bolts.Count)], shotSpawn.transform.position, shotSpawn.transform.rotation);
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }
    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        while (true)
        {
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x - mainCam.transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }
    private void FixedUpdate()
    {
        if (!isLaunched)
            return;
        //Movement

        float newManeuver = Mathf.MoveTowards(rigidBody.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rigidBody.velocity = new Vector3(newManeuver, 0.0f, 0.0f);

        rigidBody.position = new Vector3(
            Mathf.Clamp(rigidBody.position.x, mainCam.transform.position.x + -mainCam.orthographicSize / 2, mainCam.transform.position.x + mainCam.orthographicSize / 2),
            0.0f,
            transform.position.z
            );
        rigidBody.rotation = Quaternion.Euler(0.0f, 180f, rigidBody.velocity.x * -tilt);
        Debug.Log("Boss velocity " + rigidBody.velocity.x);
        //transform.rotation = Quaternion.Euler(0, 180, rigidBody.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= -2 && transform.position.x <= 2 && Mathf.Abs(transform.position.z - bossSpawn.transform.position.z) < 2 && rigidBody.velocity != Vector3.zero && !isLaunched)
        {
            rigidBody.velocity = Vector3.zero;
            isRotating = true;
        }
        if (isRotating && !isLaunched)
        {
            this.transform.rotation = Quaternion.Euler(0, Mathf.MoveTowards(this.transform.rotation.eulerAngles.y, 180, 0.5f), 0);
        }
        if (this.transform.rotation.eulerAngles.y == 180 && !isLaunched)
        {
            isLaunched = true;
            isRotating = false;
        }

    }
}
