using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class AgentScriptLvl4 : Agent
{
    [SerializeField] GameObject goal;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 5f;

    Vector3 movement = Vector3.zero;

    int rotateY = 0;


    public override void OnEpisodeBegin()
    {
        transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        do
        {
            transform.localPosition = new Vector3(Random.Range(-14f, 14f), 0.5f, Random.Range(-14f, 14f));
        } while (transform.localPosition.x < 7f && transform.localPosition.x > -7f && transform.localPosition.z < 7f && transform.localPosition.z > -7f);
        do
        {
            goal.transform.localPosition = new Vector3(Random.Range(-13f, 13f), 0.25f, Random.Range(-13f, 13f));
        } while (Vector3.Distance(transform.localPosition, goal.transform.localPosition) < 5f || (goal.transform.localPosition.x < 8f && goal.transform.localPosition.x > -8f && goal.transform.localPosition.z < 8f && goal.transform.localPosition.z > -8f));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        movement = Vector3.zero;

        switch (actions.DiscreteActions[0])
        {
            case 0:
                movement += transform.forward;
                break;
            case 2:
                movement -= transform.forward;
                break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0:
                rotateY = 1;
                break;
            case 1:
                rotateY = 0;
                break;
            case 2:
                rotateY = -1;
                break;
        }
        transform.RotateAround(transform.position, Vector3.up, rotateY * rotateSpeed * Time.deltaTime);
        transform.localPosition += movement * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int)Input.GetAxisRaw("Vertical") + 1;
        discreteActions[1] = -(int)Input.GetAxisRaw("Horizontal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            AddReward(10f);
            AddReward((MaxStep - StepCount)/100f);
            EndEpisode();
        }
        else if (other.CompareTag("ToAvoid"))
        {
            AddReward(-5f);
            EndEpisode();
        }
    }
}
