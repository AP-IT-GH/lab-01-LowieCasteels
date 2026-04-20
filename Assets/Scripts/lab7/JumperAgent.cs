using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class JumperAgent : Agent
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private bool isJumping = false;

    public WorldManager worldManager;
    private List<GameObject> obstacles;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startPosition;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        isJumping = false;
        obstacles = worldManager.obstacles;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.y);

        if (obstacles != null && obstacles.Count > 0 && obstacles[0] != null)
        {
            float relativeDist = obstacles[0].transform.localPosition.z - transform.localPosition.z;
            sensor.AddObservation(relativeDist);
            sensor.AddObservation(obstacles[0].GetComponent<ObstacleMovement>().obstacleSpeed);
        }
        else
        {
            sensor.AddObservation(10f);
            sensor.AddObservation(0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.001f);

        if (actions.DiscreteActions[0] == 1 && !isJumping)
        {
            Jump();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            if (isJumping)
            {
                AddReward(0.5f);
            }
            isJumping = false;
        }
        else if (collision.gameObject.CompareTag("obstacle"))
        {
            SetReward(-1f);
            EndEpisode();

            GameObject[] objects = GameObject.FindGameObjectsWithTag("obstacle");
            foreach (GameObject gameObject in objects)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Bonus"))
        {
            AddReward(2.0f);
            Destroy(collision.gameObject);
        }
    }
}