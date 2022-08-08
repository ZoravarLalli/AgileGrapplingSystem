using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Defining a connection to the hardware sensors to retrieve their positions and update the rig accordingly.
public class QuestSensor 
{
    public Transform sensor;
    public Transform IKrig;
    public Vector3 pos;
    public Vector3 rot;

    // Update the rig position and rotation to correspond to the sensor.
    public void UpdateSensor()
    {
        // Transform point returns the world space position when given a local position
        IKrig.position = sensor.TransformPoint(pos);
        IKrig.rotation = sensor.rotation * Quaternion.Euler(rot);
    }
}


// Used to constrain the IK rig of chosen model to the head sensor data being supplied by the player head and hand position in reality.
// This script allows the rest of the body to follow the targeted head sensor as it moves.
public class QuestRig : MonoBehaviour
{
    // Define the relevant input sensors
    public QuestSensor head;
    public QuestSensor left;
    public QuestSensor right;
    public float turningSmoothness;

    public Transform headSensorTransform;
    private Vector3 headBodyDistance;

    // Start is called before the first frame update
    void Start()
    {
        headBodyDistance = transform.position - headSensorTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = headSensorTransform.position + headBodyDistance;
        // Using project on plane to ensure we only rotate the body along with the head on the x and y plane
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headSensorTransform.up, Vector3.up).normalized, Time.deltaTime * turningSmoothness);

        // Update the rig values according to sensor values using the created QuestSensor objects
        head.UpdateSensor();
        left.UpdateSensor();
        right.UpdateSensor();
    }
}
