using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public static class ColliderExtensions
{
    public static bool Contains(this BoxCollider box, Vector3 point)
    {
        point = box.transform.InverseTransformPoint(point) - box.center;
        float halfX = (box.size.x * 0.5f);
        float halfY = (box.size.y * 0.5f);
        float halfZ = (box.size.z * 0.5f);
        if (point.x < halfX && point.x > -halfX &&
        point.y < halfY && point.y > -halfY &&
           point.z < halfZ && point.z > -halfZ)
            return true;
        else
            return false;
    }
}