using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject player;
    public GameObject focalPoint;
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject playerGoal;
    public float waveSpeedBump;

    private readonly float spawnRangeX = 10;
    private readonly float spawnZMin = 15;
    private readonly float spawnZMax = 25;
    private readonly Vector3 powerupSpawnZOffset = new(0, 0, -15);
    private readonly Vector3 playerStart = new(0, 1, -7);
    private readonly Vector3 focalPointStart = new(0, 0.2f, 0);

    private int wave = 0;
    private GoalControllerX playerGoalController;
    private GameObject[] enemies;

    private Vector3 SpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    private void ResetPlayer()
    {
        player.transform.position = playerStart;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void ResetFocalPoint()
    {
        focalPoint.transform.position = focalPointStart;
        focalPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void SpawnWave(int wave)
    {
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Vector3 pos = SpawnPosition() + powerupSpawnZOffset;
            Instantiate(powerupPrefab, pos, powerupPrefab.transform.rotation);
        }

        int numEnemies = playerGoalController.goals + 1;
        playerGoalController.goals = 0;

        enemies = new GameObject[numEnemies];
        for (int i = 0; i < numEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, SpawnPosition(),
                enemyPrefab.transform.rotation);
            enemy.GetComponent<EnemyX>().speed += (wave * waveSpeedBump);
            enemies[i] = enemy;
        }

        ResetPlayer();
        ResetFocalPoint();
    }

    private void Start()
    {
        playerGoalController = playerGoal.GetComponent<GoalControllerX>();
    }

    void Update()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCount == 0)
        {
            wave++;
            SpawnWave(wave);
        }

        if (Input.GetKey(KeyCode.R))
        {
            ResetPlayer();
            ResetFocalPoint();
        }

        if (Input.GetKey(KeyCode.X))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i]);
            }
        }
    }
}
