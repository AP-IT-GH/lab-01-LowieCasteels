using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ObelixAgent : Agent
{
    private bool carriesMenhir = false;
    public Transform Menhir;
    public Transform Destination;
    private Rigidbody rb;

    public float moveSpeed = 5f;
    public float rotationSpeed = 150f;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public override void OnEpisodeBegin()
    {
        carriesMenhir = false;
        this.transform.localPosition = new Vector3(0, 0.5f, 0);
        this.transform.localRotation = Quaternion.identity;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Menhir.gameObject.SetActive(true);
        Menhir.localPosition = new Vector3(Random.Range(-2f, 2f), 0.5f, Random.Range(-2f, 2f));
        Destination.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(carriesMenhir);
        sensor.AddObservation(transform.InverseTransformPoint(Menhir.position));
        sensor.AddObservation(transform.InverseTransformPoint(Destination.position));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveAction = actions.DiscreteActions[0];
        int rotateAction = actions.DiscreteActions[1];

        float rotation = 0f;
        if (rotateAction == 1) rotation = -1f;
        else if (rotateAction == 2) rotation = 1f;

        transform.Rotate(0, rotation * rotationSpeed * Time.fixedDeltaTime, 0);

        Vector3 velocity = Vector3.zero;
        if (moveAction == 1) velocity = transform.forward * moveSpeed;
        else if (moveAction == 2) velocity = -transform.forward * moveSpeed;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        if (this.transform.localPosition.y < -0.1f)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        AddReward(-0.001f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Menhir"))
        {
            if (!carriesMenhir)
            {
                carriesMenhir = true;
                collision.gameObject.SetActive(false);
                AddReward(1.0f);
            }
        }

        if (collision.gameObject.CompareTag("Destination"))
        {
            if (carriesMenhir)
            {
                carriesMenhir = false;
                AddReward(2.0f);
                EndEpisode();
            }
            else
            {
                AddReward(-0.05f);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        float vertical = Input.GetAxisRaw("Vertical");
        discreteActionsOut[0] = vertical > 0 ? 1 : (vertical < 0 ? 2 : 0);
        float horizontal = Input.GetAxisRaw("Horizontal");
        discreteActionsOut[1] = horizontal < 0 ? 1 : (horizontal > 0 ? 2 : 0);
    }
}