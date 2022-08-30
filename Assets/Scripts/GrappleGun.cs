using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    private Transform transform;
    [SerializeField]
    private float reelRate;
    private float projectileSpeed;
    private Vector3 aimingAngle;
    // Corresponding latch object for this gun
    GameObject GrappleLatch;

    // Fires a GrappleLatch object from the GrappleGun's transform position along the specified trajectory angle
    // Latch impact handling logic is in GrappleLatch.
    public void FireGrapple(Vector3 trajectoryAngle)
    {
        //todo
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
