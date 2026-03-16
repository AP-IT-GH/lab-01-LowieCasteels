using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CubeAgentGreenZone : Agent
{
    public Transform Target;
    public Transform GreenZone;
    public float speedMultiplier = 0.1f;

    private bool hasTouchedTarget = false;

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
        }

        hasTouchedTarget = false;
        Target.gameObject.SetActive(true);
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(GreenZone.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(hasTouchedTarget);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-1f / MaxStep);

        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = -actions.ContinuousActions[1];
        controlSignal.z = actions.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);


        if (!hasTouchedTarget)
        {
            float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

            AddReward(0.001f * (1.0f / distanceToTarget));

            if (distanceToTarget < 1.42f)
            {
                hasTouchedTarget = true;
                Target.gameObject.SetActive(false);
                AddReward(0.5f);
            }
        }
        else
        {
            if (this.transform.localPosition.x > 3.5f)
            {
                SetReward(1.0f);
                EndEpisode();
            }
        }

        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}