using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public List<GameObject> obstacles = new List<GameObject>();

    private float spawnTimer = 0f;
    public float spawnInterval = 3f;

    void Update()
    {
        obstacles.RemoveAll(obstacle => obstacle == null);

        if (obstacles.Count == 0)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnObstacle();
                spawnTimer = 0f;
            }
        }
    }

    private void SpawnObstacle()
    {
        float randomZ = Random.Range(-5f, -3f);
        GameObject newObstacle = Instantiate(obstaclePrefab, transform);
        newObstacle.transform.localPosition = new Vector3(0f, 0f, randomZ);

        newObstacle.GetComponent<ObstacleMovement>().obstacleSpeed = Random.Range(2f, 6f);

        obstacles.Add(newObstacle);
    }
}
