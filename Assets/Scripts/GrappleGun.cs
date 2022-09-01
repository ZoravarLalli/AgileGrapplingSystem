using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    //private Transform transform; already can reference via implicit gameobject
    [SerializeField]
    private float reelRate;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private bool left;
    private Vector3 aimingAngle;

    // Corresponding latch object for this gun
    [SerializeField]
    private GrappleLatch grappleLatch;
    // Reference to the oculus device that controls the functions of this class in game
    private UnityEngine.XR.InputDevice controller ;

    // Fires a GrappleLatch object from the GrappleGun's transform position along the specified trajectory angle
    // Latch impact handling logic is in GrappleLatch.
    public void FireGrapple(Vector3 trajectoryAngle)
    {
        ///TODO FireGrapple logic

        /// Make an instance for a grapple that is to be fired
        grappleLatch = new GrappleLatch(this);
        grappleLatch.transform.position = transform.position; // put it at the same position as the gun it is fired from 
        /// TODO decide if we need to make a originPoint object to fire from for grapples or not
        
    }

    // Calculates the aiming angle of the hand/grapplegun according to the current orientation of quest sensors.
    private Vector3 CalculateAngle()
    {
        ///TODO Calculate Angle logic
        Vector3 rayAngle;
        // Only need to get references to input devices once, so if variable to hold it already has it assigned, skip
        // Order of how oculus hardware starts makes it so that in start() I can't detect controllers yet, they are only active once update loops begin
        if(!controller.isValid)
        {
            Debug.Log("STILL NULL");
            // Need to access XR Devices via unity engine xr package's inputdevices.getdevicesatxrnode function
            // it is a query type method to filter through all devices according to left handed... I guess in case
            // there are foot trackers on the left as well
            var handDevices = new List<UnityEngine.XR.InputDevice>();
            if (left == true)
            {
                UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, handDevices);
            }
            else
            {
                UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, handDevices);
            }

            // Checking for errors detecting devices

            if (handDevices.Count == 1) // Successfully detected one controller for given hand
            {
                controller = handDevices[0];
                ///Debug.Log(gameObject.name + " Reading device: " + controller.name + " with the role: " + controller.role.ToString());
            }
            else if (handDevices.Count > 1)
            {
                if (left == true)
                {
                    Debug.Log("DEVICE ISSUE: There is more than one controller detected for the left hand.");
                }
                else
                {
                    Debug.Log("DEVICE ISSUE: There is more than one controller detected for the right hand.");
                }
            }
            else
            {
                if (left == true)
                {
                    Debug.Log("DEVICE ISSUE: There is no controller connected for the left hand.");
                }
                else
                {
                    Debug.Log("DEVICE ISSUE: There is no controller connected for the right hand.");
                }
            }

            //Debug.Log("Device count: " + handDevices.Count);
        }

        // Use a raycast from gameObjects transform (instance of this script is attached to each grapple gun)
        // This function is part of fixedUpdate loop's calls so we can do ray casting here
        RaycastHit hit;
        // define which layers are collidable
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) // returns true if hits collider
        {
            Debug.Log("ON GRAPPLEABLE TARGET");
        }
        else
        {
            Debug.Log("NO VALID TARGET");
        }

        return Vector3.zero;
    }

    // Check if the trajectory is valid, realized this is probably unneccesary
    // Player should always be able to shoot if aiming at a grappleable surface
    // regardless of trajectory. Probably taking this out.
    private bool ValidTrajectory()
    {
        return true; // always valid for now as not sure what would constitute invalid trajectory.
    }

    // Start is called before the first frame update
    void Start()
    {
        CalculateAngle();
    }

    // Check for inputs from player and fire corresponding grapple action for detected inputs
    // I think it's safer to put all input calls in FixedUpdate as the actions are going to be manipulating forces on a rigidbody in game
    void FixedUpdate()
    {
        CalculateAngle();
        ///TODO Check for grapple firing input and call function if input is detected

    }
}
