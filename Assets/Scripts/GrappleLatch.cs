using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLatch : MonoBehaviour
{
    private GameObject self; // Can use this for the physical object representing grapple in game
    private GrappleGun gun; // the gun that shot the grapple
    private bool latched;

    // When initializing a grapple latch object we pass the grapple gun that is initializing it always.
    public GrappleLatch(GrappleGun originGun)
    {
        gun = originGun;
    }

    void Start()
    {
        latched = false; // always starts unlatched as it is initialized when shot from GrappleGun.
    }

    // Check for collisions with Grappleable objects and apply appropriate function depending on latched status
    void Update()
    {
        ///TODO Grapple Latch collision update check
    }

    // Latch the grapple to any Grappleable object it is in collision with currently.
    public void Latch()
    {
        // Latch the grapple by binding this transform to the transform of the collided grappleable surface
        /// TODO latching logic
        latched = true;
    }

    // Unlatch the grapple from the currently latched state, if it isn't latched this call is void
    public void Unlatch()
    {
        // Unlatch the grapple by unbinding this transform to the transform of the currently collided grappleable surface
        /// TODO unlatching logic
        latched = false;
    }

    // Returns distance from this grapple to the gun that fired it, call this function from grapple gun through the member instance of grappleLatch.
    public float distanceToGun(GameObject grappleGun)
    {
        // TODO Consider changing origin point for grapple after shot to player instead of gun transform
        // Maybe it should be distance to player instead as if the distances vary slightly between both grapples
        // it could mess with physics calculations, might be better to treat both grapples as originating from player's
        // transform after initial shot rather than from the two guns themselves.
        return Vector3.Distance(grappleGun.transform.position, transform.position);
    }
}
