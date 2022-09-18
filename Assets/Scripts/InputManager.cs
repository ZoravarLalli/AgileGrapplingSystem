using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not all input is managed here the grapple shot inputs are read and handled in GrappleGun for example.
/// This is actually just an easy access way to read input values from anywhere else in project via an instance
/// of this class on the player gameobject.
/// </summary>
public class InputManager : MonoBehaviour
{
    private bool leftReelInput;
    private bool leftSwingInput;
    private bool leftShotInput;
    private bool rightReelInput;
    private bool rightSwingInput;
    private bool rightShotInput;
    private Vector3 headOrientation;
    // variables to hold references to the oculus controllers
    private UnityEngine.XR.InputDevice leftController;
    private UnityEngine.XR.InputDevice rightController;
    [SerializeField]
    private float reelInputThreshold;    

    void Awake()
    {
        leftReelInput = false;
        leftSwingInput = false;
        leftShotInput = false;
        rightReelInput = false;
        rightSwingInput = false;
        rightShotInput = false;
        headOrientation = Vector3.zero;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get refs to controller, keep trying until controllers are valid, only valid once a valid controller assigned to variable
        if(!leftController.isValid || !rightController.isValid)
        {
            // Need to use lists to get reference to controllers first since using XRNode returns queried list result
            var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
            var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
            // Now can assign first index of each list to variable to reference controllers
            leftController = leftHandDevices[0];
            rightController = rightHandDevices[0];
        }
        else // Only process inputs once both controllers are set
        {
            // Update shot booleans according to if trigger to shoot is pressed or not
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftShotInput);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightShotInput);

            // Update reel booleans according to if trigger is depressed past reel threshold or not
            float leftTest;
            float rightTest;
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out leftTest);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out rightTest);
            Debug.Log("LEFT TRIGGER: " + leftTest + " RIGHT TRIGGER: " + rightTest);
        }
    }

    // Getters
    public bool GetLeftReelInput()
    {
        return leftReelInput;
    }
    public bool GetLeftSwingInput()
    {
        return leftSwingInput;
    }
    public bool GetLeftShotInput()
    {
        return leftShotInput;
    }
    public bool GetRightReelInput()
    {
        return rightReelInput;
    }
    public bool GetRightSwingInput()
    {
        return rightSwingInput;
    }
    public bool GetRightShotInput()
    {
        return rightShotInput;
    }
    public Vector3 GetHeadOrientation()
    {
        return headOrientation;
    }
}
