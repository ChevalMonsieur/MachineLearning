using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentScriptLvl2 : Agent
{
    [SerializeField] GameObject goal;
    [SerializeField] float moveSpeed = 5f;

    float lastDistance = 0f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(-9f, 9f));
        do {
            goal.transform.localPosition = new Vector3(Random.Range(-9f, 9f), 0.25f, Random.Range(-9f, 9f));
        } while (Vector3.Distance(transform.localPosition, goal.transform.localPosition) < 3f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(goal.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveX = 0;
        int moveZ = 0;

        switch (actions.DiscreteActions[0])
        {
            case 0:
                moveX = 1;
                break;
            case 1:
                moveX = 0;
                break;
            case 2:
                moveX = -1;
                break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0:
                moveZ = 1;
                break;
            case 1:
                moveZ = 0;
                break;
            case 2:
                moveZ = -1;
                break;
        }

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

        float thisDistance = Vector3.Distance(transform.localPosition, goal.transform.localPosition);
        SetReward(lastDistance - thisDistance);
        lastDistance = thisDistance;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = -(int)Input.GetAxisRaw("Horizontal");
        discreteActions[1] = -(int)Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            AddReward(4f);
            EndEpisode();
        } else if (other.CompareTag("ToAvoid"))
        {
            AddReward(-4f);
            EndEpisode();
        }
    }
}
