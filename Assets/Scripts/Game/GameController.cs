using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<GameObject> hazards;

    public List<GameObject> enemies;

    public List<GameObject> bosses;

    public Animator animator;

    public PlayerController playerController;

    public int score;
    public int playerCount;
    public int asteroidPerEnemyMin;
    public int asteroidPerEnemyMax;
    public int hazardCount;
    public int wavePerBoss;


    public Vector3 spawnValues;

    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text finishScoreText;
    public Text highScore;
    public Text playerCountText;

    private GameObject boss;
    private void Start()
    {
        score = 0;
        boss = null;
        UpdateScore();
        UpdatePlayerCount();
        StartCoroutine(SpawnWaves());
    }
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        int waveCount = 0;
        while (true)
        {
            if (boss == null)
            { 
                hazardCount++;
                waveCount++;
            }
            else
            {
                waveCount = 0;
            }
            Vector3 spawnPosition;
            Quaternion spawnRotation = Quaternion.identity;
            int enemyFrequency = Random.Range(asteroidPerEnemyMin, asteroidPerEnemyMax);
            for (int i = 0; i < hazardCount; i++)
            {
                spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                if (i % enemyFrequency == 0 && enemies.Count > 0 && boss == null)
                {
                    Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPosition, spawnRotation);
                    spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    yield return new WaitForSeconds(spawnWait);
                }
                List<Quaternion> hazardRotations = new List<Quaternion>() { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 7.95f, 0), Quaternion.Euler(0, -7.95f, 0) };
                int hazardIndex = Random.Range(0, hazards.Count);
                Instantiate(hazards[hazardIndex], spawnPosition, hazardRotations[hazardIndex == 0 ? hazardIndex : Random.Range(1, hazardRotations.Count)]);
                yield return new WaitForSeconds(spawnWait);
            }
            spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            if (waveCount % wavePerBoss == 0 && bosses.Count > 0 && boss == null)
            {
                boss = Instantiate(bosses[Random.Range(0, bosses.Count)], spawnPosition, spawnRotation);
                asteroidPerEnemyMin = asteroidPerEnemyMin > 1 ? asteroidPerEnemyMin - 1 : asteroidPerEnemyMin;
                asteroidPerEnemyMax = asteroidPerEnemyMax > asteroidPerEnemyMin ? asteroidPerEnemyMax - 1 : asteroidPerEnemyMax;
            }
            yield return new WaitForSeconds(waveWait);

        }
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    public void playerLose()
    {
        playerCount--;
        UpdatePlayerCount();
        if (playerCount == 0)
            OpenFinishMenu();
        else
        {
            StartCoroutine(playerController.respawn());
        }
    }
    public string formatScore(int score, int outputLength, char placeholder)
    {
        string result = "";
        while (result.Length + score.ToString().Length < outputLength)
        {
            result += placeholder;
        }
        result += score.ToString();
        return result;
    }
    public void UpdateHighScore()
    {
        int intHighScore = PlayerPrefs.GetInt("highscore");
        if (intHighScore < score)
        {
            intHighScore = score;
            PlayerPrefs.SetInt("highscore", intHighScore);
        }
        highScore.text = formatScore(intHighScore, 8, ' ');
    }
    public void OpenFinishMenu()
    {
        finishScoreText.text = formatScore(score, 8, ' ');
        animator.SetBool("isFinishOpened", true);
        UpdateHighScore();
    }
    private void UpdatePlayerCount()
    {
        playerCountText.text = "";
        for (int i = 0; i < playerCount; i++)
            playerCountText.text += "✡";
    }
    private void UpdateScore()
    {
        string text = "";
        scoreText.text = "";
        text = formatScore(score, 8, '0');
        for (int i = 0; i < text.Length; i++)
        {
            scoreText.text += text[i];
            if (i == 3)
                scoreText.text += " ";
        }
        animator.SetBool("isShaked", true);
    }
}
