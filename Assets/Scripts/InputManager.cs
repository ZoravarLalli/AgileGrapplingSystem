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
    private float leftSwingInputAmount;
    private float rightSwingInputAmount;
    private Vector3 headEulerRotation;
    private Vector2 joystickValue;
    // variables to hold references to the oculus sensors
    private UnityEngine.XR.InputDevice leftController;
    private UnityEngine.XR.InputDevice rightController;
    private UnityEngine.XR.InputDevice headset;
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
        leftSwingInputAmount = 0.0f;
        rightSwingInputAmount = 0.0f;
        headEulerRotation = Vector3.zero;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // get refs to sensor, keep trying until controllers are valid, only valid once a valid sensor assigned to variable
        if (!leftController.isValid || !rightController.isValid || !headset.isValid)
        {
            // Need to use lists to get reference to sensors first since using XRNode returns queried list result
            var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
            var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
            var headDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.Head, headDevices);
            // Now can assign first item of each list to XR.InputDevice variables to reference sensors
            leftController = leftHandDevices[0];
            rightController = rightHandDevices[0];
            headset = headDevices[0];
        }
        else // Only process inputs once all sensors are ready
        {
            // Update shot input booleans according to if trigger to shoot is pressed or not
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftShotInput);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightShotInput);

            // Update reel input booleans according to if trigger is pressed past reel threshold or not
            float leftReelTemp;
            float rightReelTemp;
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out leftReelTemp);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out rightReelTemp);

            // Check if past threshold and update boolean
            if (leftReelTemp > reelInputThreshold)
            {
                leftReelInput = true;
            }
            else
            {
                leftReelInput = false;
            }
            if (rightReelTemp > reelInputThreshold)
            {
                rightReelInput = true;
            }
            else
            {
                rightReelInput = false;
            }

            // Update swing input booleans according to if side grip button is pressed or not
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out leftSwingInput);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out rightSwingInput);

            // Update swing amount floats according to how much the side grip is pressed
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out leftSwingInputAmount);
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out rightSwingInputAmount);

            // Using this value for force manipulation via joystick to give extra control over motion path
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystickValue);
        }
    }

    // Getters
    public bool GetLeftShotInput()
    {
        return leftShotInput;
    }
    public bool GetRightShotInput()
    {
        return rightShotInput;
    }
    public bool GetLeftReelInput()
    {
        return leftReelInput;
    }
    public bool GetRightReelInput()
    {
        return rightReelInput;
    }
    public bool GetLeftSwingInput()
    {
        return leftSwingInput;
    }
    public bool GetRightSwingInput()
    {
        return rightSwingInput;
    }
    public float GetLeftSwingInputAmount()
    {
        return leftSwingInputAmount;
    }
    public float GetRightSwingInputAmount()
    {
        return rightSwingInputAmount;
    }
    public Vector3 GetHeadEulerRotation()
    {
        return headEulerRotation;
    }
    public Vector2 GetJoystickValue()
    {
        return joystickValue;
    }
}
