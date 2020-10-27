using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax;
    public float zMin, zMax;
}
public class PlayerController : MonoBehaviour
{
    public Boundary boundary;

    public float fireRate = 0.5F;
    public float nextFire = 0.0F;
    public float speed;
    public float tilt;
    private float camField;

    public GameObject shot;

    public TouchHandler touchHandler;

    public Camera mainCam;


    public Transform shotSpawn;
    public Transform exShotSpawn1;
    public Transform exShotSpawn2;

    private bool isReady;

    private void Start()
    {
        isReady = false;
        StartCoroutine(getReady());
        //Set the default camera field (x-axis)
        camField = mainCam.orthographicSize / 2;
        Time.timeScale = 1.0f;
    }

    private void FixedUpdate()
    {
        if (!isReady)
            return;
        Vector2 movementVector2 = touchHandler.GetDirection();
        Vector3 movement = new Vector3(movementVector2.x, 0.0f, movementVector2.y);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity += movement * speed;
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
        transform.rotation = Quaternion.Euler(0, 0, rb.rotation.eulerAngles.z);
    }
    public IEnumerator respawn()
    {
        //Nếu camera ở ngoài thì dịch chuyển vô
        while (Mathf.Abs(mainCam.transform.position.x) > 0)
        {
            yield return null;
            mainCam.transform.position = new Vector3(Mathf.MoveTowards(mainCam.transform.position.x, 0, 0.2f), mainCam.transform.position.y, mainCam.transform.position.z);
        }

        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(0f, 0f, 0f);
        GetComponent<CollectItem>().openShield(false);
        yield return new WaitForSeconds(1.25f);
        GameObject.FindWithTag("Shield").transform.parent = transform;
        gameObject.SetActive(true);
        yield return null;
    }
    public IEnumerator getReady()
    {
        transform.position = new Vector3(0, 0, -5.55f);
        gameObject.SetActive(true);
        Rigidbody rb = GetComponent<Rigidbody>();
        float originDrag = rb.drag;
        GetComponent<CollectItem>().openShield();
        rb.drag = 0.5f;
        rb.velocity = new Vector3(0, 0, 0);
        rb.velocity = Vector3.forward * 3.5f;
        yield return new WaitForSeconds(3.75f);
        rb.velocity = new Vector3(0, 0, 0);
        rb.drag = originDrag;
        isReady = true;
    }
    private void LateUpdate()
    {
        Debug.Log(camField);
        if (Mathf.Abs(transform.position.x) > (camField - 1.25f))
        {
            mainCam.transform.position = new Vector3(Mathf.Sign(transform.position.x) * (Mathf.Abs(transform.position.x) - (camField - 1.25f)), mainCam.transform.position.y, mainCam.transform.position.z);
            Debug.Log(Mathf.Sign(transform.position.x) * (Mathf.Abs(transform.position.x) - camField));
        }
    }
    private void Update()
    {
        if (!isReady)
            return;
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            if (exShotSpawn1.gameObject.activeInHierarchy && exShotSpawn2.gameObject.activeInHierarchy)
            {
                Instantiate(shot, exShotSpawn1.position, exShotSpawn1.rotation);
                Instantiate(shot, exShotSpawn2.position, exShotSpawn2.rotation);
            }
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
    }
}
