using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EulerAngleFunctions
{
    public static float GetXDegrees(Transform t)
    {
        // Get the angle about the world x axis in range -pi to +pi,
        // with 0 corresponding to a 180 degree rotation.
        var radians = Mathf.Atan2(t.forward.y, -t.forward.z);


        return 180 + radians * Mathf.Rad2Deg;
    }
}
