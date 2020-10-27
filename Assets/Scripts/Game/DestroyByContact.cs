using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject[] items;
    public int scoreValue;
    public bool isImmortal = false;
    public float itemRatio = 0.3f;

    private GameController gameController;
    private bool isItemProduced;

    private void Start()
    {
        if (itemRatio == 0)
            itemRatio = 0.000001f;
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController>();
        if (gameController == null)
        {
            Debug.Log("Can not find GameController Script");
            Destroy(gameObject);
        }
           

        isItemProduced = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Item"))
            return;


        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            other.gameObject.SetActive(false);
            gameController.playerLose();
            return;
        }
        else if (!CompareTag("Bolt") && !isItemProduced)
        {
            isItemProduced = true;
            int itemIndex = Random.Range(0, Mathf.FloorToInt(items.Length / itemRatio));
            if (itemIndex < items.Length && !isImmortal)
                Instantiate(items[itemIndex], transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (other.tag != "Player")
            gameController.AddScore(scoreValue);

      
        if (other.CompareTag("Shield") && other.GetComponent<ShieldIndex>().isImmortal)
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
            return;
        }
        if (!isImmortal || other.CompareTag("Shield") || other.CompareTag("Rocket"))
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }

        Destroy(other.gameObject);
    }
}
