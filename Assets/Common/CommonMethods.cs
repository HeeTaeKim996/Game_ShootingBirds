
using UnityEngine;

public static class CommonMethods
{
    public static Vector3 GetClosestPointOnLine(Vector3 point, Vector3 firstPivot, Vector3 secondPivot)
    {
        Vector3 direction = secondPivot - firstPivot;
        Vector3 directionNormalized = direction.normalized;

        return firstPivot + Mathf.Clamp(Vector3.Dot(point - firstPivot, directionNormalized), 0, direction.magnitude) * directionNormalized;
    }
}
