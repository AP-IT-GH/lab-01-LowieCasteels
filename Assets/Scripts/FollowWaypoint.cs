using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWaypoint = 0;

    public float speed = 10.0f;
    public float rotSpeed = 10.0f;
    public float lookAhead = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = waypoints[currentWaypoint].transform.position - transform.position;
        direction.y = 0f;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position);
        
        if (distance < lookAhead)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }

    }
}
