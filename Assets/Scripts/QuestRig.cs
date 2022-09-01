using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ORIGINALLY NAMED VRInputHandler
// Used to constrain the IK rig of chosen model to the head sensor data being supplied by
// the player head and hand position inputs in reality. This script allows the rest of the
// virtual body to follow the targeted head/hand sensors within the defined constraints.
public class QuestRig : MonoBehaviour
{
    // Define the available input sensors
    public QuestSensor head;
    public QuestSensor left;
    public QuestSensor right;
    public float turningSmoothness;

    public Transform headSensorTransform;
    private Vector3 headBodyDistance;

    // Start is called before the first frame update
    void Start()
    {
        // Storing the offset distance from headset position to center of gameobject
        // Have to do this everytime as height of user is variable.
        headBodyDistance = transform.position - headSensorTransform.position;
    }


    void FixedUpdate()
    {
        // Use the offset to ensure updating position accounts for player's height in real world as well as gameobject position in game.
        transform.position = headSensorTransform.position + headBodyDistance;
        // Using project on plane to ensure we only rotate the body along with the head on the x and y plane, rotation on other axis for torso is unnatural
        // Also using a timed delay for smoothness of movements
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headSensorTransform.up, Vector3.up).normalized, Time.deltaTime * turningSmoothness);

        // Update the rig values according to sensor values using the created QuestSensor instances
        head.UpdateSensor();
        left.UpdateSensor();
        right.UpdateSensor();
    }
}

[System.Serializable]
// Helper class for defining a connection to the hardware sensors to retrieve their positions and update the rig accordingly.
// Defining in questrig since instances of this class are only used here to help with input handling in the update loop.
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
