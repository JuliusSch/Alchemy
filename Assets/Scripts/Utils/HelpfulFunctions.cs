using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class HelpfulFunctions
{
    public static List<T> FindAssetsByType<T>() where T : Object
    {
        List<T> assets = new List<T>();

        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (asset != null)
                assets.Add(asset);
        }
        return assets; 
    }

    // Vector - inspired by https://gist.github.com/modyari/e53cefad97aebeb9a290504206a7fc61
    public static Vector3 With(this Vector3 point, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(
            x == null ? point.x : x.Value,
            y == null ? point.y : y.Value,
            z == null ? point.z : z.Value
        );
    }

    public static Vector3 Add(this Vector3 point, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(
            point.x + (x == null ? 0 : x.Value),
            point.y + (y == null ? 0 : y.Value),
            point.z + (z == null ? 0 : z.Value)
        );
    }

    public static Vector3 Add(this Vector3 vec, Vector3 vec2)
    {
        return new Vector3(vec.x + vec2.x, vec.y + vec2.y, vec.z + vec2.z);
    }

    // Quaternion
    public static Quaternion With(this Quaternion rotation, float? x = null, float? y = null, float? z = null)
    {
        return Quaternion.Euler(rotation.eulerAngles.With(x, y, z));
    }

    public static Quaternion Add(this Quaternion rotation, float? x = null, float? y = null, float? z = null)
    {
        return Quaternion.Euler(rotation.eulerAngles.Add(x, y, z));
    }
}

// https://gist.github.com/kurtdekker/0da9a9721c15bd3af1d2ced0a367e24e?permalink_comment_id=3803990
public class CallAfterDelay : MonoBehaviour
{
    float delay, age;
    System.Action action;

    // Will never call this frame, always the next frame at the earliest
    public static CallAfterDelay Create(float delay, System.Action action)
    {
        CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
        cad.delay = delay;
        cad.action = action;
        return cad;
    }

    void Update()
    {
        if (age > delay)
        {
            action();
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        age += Time.deltaTime;
    }
}