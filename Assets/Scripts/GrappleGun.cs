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
    private float maxDistance;
    [SerializeField]
    private bool left;
    private Vector3 aimingAngle;
    private bool validTarget;
    private bool isFired;

    // Corresponding latch object for this gun
    [SerializeField]
    public GameObject grappleLatch;
    private GameObject grappleObj;
    public Transform grappleShotOrigin;
    // Reference to the oculus device that controls the functions of this class in game
    private UnityEngine.XR.InputDevice controller;
    private LineRenderer grappleLine;

    // Hooking refs to line renderers to draw grapple lines.
    private void Awake()
    {
        grappleLine = gameObject.GetComponent<LineRenderer>();
        isFired = false;
    }


    private void Start()
    {
        
    }

    // Fires a GrappleLatch object from the GrappleGun's transform position along the specified trajectory angle
    // Latch impact handling logic is in GrappleLatch.
    public void FireGrapple(Vector3 angle)
    {

        // First check if target is valid and shot not already fired
        if (validTarget && !isFired)
        {
            Debug.Log("SHOOTING GRAPPLE");
            // Instantiate the grappleLatch prefab and set a grappleObj to reference it so we can modify script values.
            grappleObj = Instantiate(grappleLatch, grappleShotOrigin.position, Quaternion.identity) as GameObject;
            GrappleLatch grappleScript = grappleObj.GetComponent<GrappleLatch>();
            // set a members in the GrappleLatch script for the gun it was shot from and corresponding controller
            grappleScript.SetGun(gameObject.GetComponent<GrappleGun>());
            // Set the parameters
            grappleScript.SetAngle(angle);
            grappleScript.SetSpeed(projectileSpeed);
            grappleScript.SetMaxDistance(maxDistance);


            isFired = true; // Set the shot to be true to it wont repeat
        }
        else
        {
            ///SOUND: No valid target sound
            if (!validTarget)
            {
                //Debug.Log("CAN'T GRAPPLE TO THAT"); // This will fire as long as input held...
                // Have grapple line ready at origin to fire.
            } else if (isFired)
            {
                // Already fired and attempting to fire again
                return;
            }
        }
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
            //Debug.Log("STILL NULL");
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
                    //Debug.Log("DEVICE ISSUE: There is more than one controller detected for the left hand.");
                }
                else
                {
                    //Debug.Log("DEVICE ISSUE: There is more than one controller detected for the right hand.");
                }
            }
            else
            {
                if (left == true)
                {
                    //Debug.Log("DEVICE ISSUE: There is no controller connected for the left hand.");
                }
                else
                {
                    //Debug.Log("DEVICE ISSUE: There is no controller connected for the right hand.");
                }
            }

            //Debug.Log("Device count: " + handDevices.Count);
        }

        // layer mask to define what raycasts can collide with (only 3rd layer which is 'grappleable')
        int layerMask = 1 << 3;

        // Have grapple line ready at origin to fire.
        //grappleLine.SetPosition(0, transform.position);

        // Use a raycast from gameObjects transform (instance of this script is attached to each grapple gun)
        // This function is part of fixedUpdate loop's calls so we can do ray casting here
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) // returns true if hits collider
        {
            //grappleLine.SetPosition(1, hit.point); // careful it will stay if not cleared.
            //grappleLine.SetColors(Color.green, Color.green);
            //Debug.Log("ON GRAPPLEABLE TARGET " + hit.collider.gameObject.name);
            validTarget = true;
        }
        else
        {
            //grappleLine.SetPosition(1, transform.TransformDirection(Vector3.forward) * 300);
            //grappleLine.SetColors(Color.red, Color.red);
            //Debug.Log("NO VALID TARGET");
            validTarget = false;
        }

        // Return the direction from grappleLatch origin to hit point
        return hit.point - grappleShotOrigin.transform.position;
    }

    // Check for inputs from player and fire corresponding grapple action for detected inputs
    // I think it's safer to put all input calls in FixedUpdate as the actions are going to be manipulating forces on a rigidbody in game
    void FixedUpdate()
    {
        aimingAngle = CalculateAngle(); // Want to update aimingAngle with physics loop as the angle is part of physics calc

        // Check for input for fire grapple
        bool triggerPressed;
        // This function will propel the latch originating from grappleGun position along the aimingAngle vector
        if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
        {
            // May need to make grapples fireable while one is still held instead of after destroying old one
            FireGrapple(aimingAngle);
        }
        else
        {
            if(isFired == true)
            {
                // Doing destruction here as destroying in the grappleObj scripts tries to destroy 
                // asset prefab instead of instance of prefab in game... Weird error
                Destroy(grappleObj);
                isFired = false;
            }
        }
    }

    // Using late update to draw grapple line so that it doesnt stutter
    // Having movement update done first before visual update of line position prevents stutter
    void LateUpdate()
    {

        // Check for input for fire grapple
        bool triggerPressed;
        // This function will propel the latch originating from grappleGun position along the aimingAngle vector
        if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
        {
            // Only ever draw the line if the grapple latch is currently existing
            if (grappleObj != null)
            {
                grappleLine.enabled = true;
                grappleLine.SetColors(Color.black, Color.black);
                grappleLine.SetPosition(0, grappleShotOrigin.position);
                grappleLine.SetPosition(1, grappleObj.transform.position);
            }
            else
            {
                grappleLine.enabled = false;
            }
        } 
        else if (grappleObj == null) // stop drawing grapple when release grapple control
        {
            grappleLine.enabled = false;
        }
    }

    private void Update()
    {
        

    }

    // Check if the trajectory is valid, realized this is probably unneccesary
    // Player should always be able to shoot if aiming at a grappleable surface
    // regardless of trajectory. Probably taking this out.
    private bool ValidTrajectory()
    {
        return true; // always valid for now as not sure what would constitute invalid trajectory.
    }
}


