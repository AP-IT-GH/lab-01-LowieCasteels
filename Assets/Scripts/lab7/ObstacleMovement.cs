using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float obstacleSpeed = 0.5f;
    public GameObject obstaclePrefab;

    void Update()
    {
        transform.localPosition += Vector3.forward * obstacleSpeed * Time.deltaTime;

        if (transform.localPosition.z > 5f)
        {
            Destroy(gameObject);
        }
    }
}
