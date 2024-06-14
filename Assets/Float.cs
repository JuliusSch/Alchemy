using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Float : MonoBehaviour
{
    //  https://www.youtube.com/watch?v=v7ag-NeSMSQ

    public Rigidbody Rb;
    public float DepthBeforeSubmersion;
    public float DisplacementAmount;
    public int Floaters;

    public float WaterDrag;
    public float WaterAngularDrag;
    public WaterSurface Water;

    private WaterSearchParameters SearchParameters;
    private WaterSearchResult SearchResult;

    private void FixedUpdate()
    {
        Rb.AddForceAtPosition(Physics.gravity / Floaters, transform.position, ForceMode.Acceleration);
        SearchParameters.startPositionWS = transform.position;
        Water.ProjectPointOnWaterSurface(SearchParameters, out SearchResult);
        if (transform.position.y < SearchResult.projectedPositionWS.y)
        {
            float displacementMultiplier = Mathf.Clamp01((SearchResult.projectedPositionWS.y - transform.position.y) / DepthBeforeSubmersion) * DisplacementAmount;
            Rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            Rb.AddForce(displacementMultiplier * -Rb.velocity * WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            Rb.AddTorque(displacementMultiplier * -Rb.angularVelocity * WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
