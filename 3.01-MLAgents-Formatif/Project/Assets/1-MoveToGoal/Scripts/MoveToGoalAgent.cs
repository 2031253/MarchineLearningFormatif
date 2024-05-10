
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class MoveToGoalAgent : Agent
{
   [SerializeField] private Transform targetTransform;
    [SerializeField] private Material sucessMaterial;
    [SerializeField] private Material failureMaterial;
    [SerializeField] private Renderer floor;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float speed = 10;
        transform.Translate(new UnityEngine.Vector3(moveX, 0, moveY) * Time.deltaTime * speed);
    }

    //le reward (la récompense ou la punition)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("goal"))
        {
            SetReward(1f);
            floor.material = sucessMaterial;
            EndEpisode();
        }
        else if (other.CompareTag("wall"))
        {
            SetReward(-1f);
            floor.material = failureMaterial;   
            EndEpisode();
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contActions = actionsOut.ContinuousActions;
        contActions[0] = Input.GetAxisRaw("Horizontal");
        contActions[1] = Input.GetAxisRaw("Vertical");
    }


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 1, 0);
    }

}
