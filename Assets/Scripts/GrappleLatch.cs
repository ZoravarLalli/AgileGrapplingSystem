using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLatch : MonoBehaviour
{
    private GameObject self; // Can use this for the physical object representing grapple in game
    private GrappleGun gun; // the gun that shot the grapple
    private bool latched;
    private ContactPoint latchPoint; // The point the latch is attached to the collided surface at
    private Vector3 angle;
    private float projectileSpeed;
    private float maxDistance;

    void Start()
    {
        Debug.Log("Grapple LIVE");
        latched = false; // always starts unlatched as it is initialized when shot from GrappleGun.
    }

    // Check for collisions with Grappleable objects and apply appropriate function depending on latched status
    void Update()
    {
        if (latched) 
        {
            // bind transform to latchPoint transform so it is stuck
            transform.position = latchPoint.point;
            Debug.Log("Grapple Pos: " + transform.position);
        }
        else
        {
            transform.Translate(angle * Time.deltaTime * projectileSpeed);
            
            // Destroy grapple latch if we don't hit a target within the allowed range
            if(distanceToGun() > maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    // Latch the grapple to any Grappleable object it is in collision with currently.
    public void Latch()
    {

        /// TODO latching logic
        latched = true;
        // Latch the grapple by binding this transform to the transform of the collided grappleable surface
        // do it in an update loop according to boolean status
    }

    // Unlatch the grapple from the currently latched state, if it isn't latched this call is void
    public void Unlatch()
    {
        // Unlatch the grapple by unbinding this transform to the transform of the currently collided grappleable surface
        latched = false;
        // Not sure what to do with unlatch yet... Destruction of latch takes place in grappleGun
    }

    // Returns distance from this grapple to the gun that fired it, call this function from grapple gun through the member instance of grappleLatch.
    public float distanceToGun()
    {
        // TODO Consider changing origin point for grapple after shot to player instead of gun transform
        // Maybe it should be distance to player instead as if the distances vary slightly between both grapples
        // it could mess with physics calculations, might be better to treat both grapples as originating from player's
        // transform after initial shot rather than from the two guns themselves.
        return Vector3.Distance(gun.transform.position, transform.position);
    }

    // Define latching functionality for collisions
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT A WALL");
        // get the contact point to latch to
        latchPoint = collision.GetContact(0);
        Latch();
    }

    // Setters
    public void SetGun(GrappleGun grappleGun)
    {
        gun = grappleGun;
    }
    public void SetAngle(Vector3 ang)
    {
        angle = ang;
    }
    public void SetSpeed(float speed)
    {
        projectileSpeed = speed;
    }
    public void SetMaxDistance(float distance)
    {
        maxDistance = distance;
    }
}
