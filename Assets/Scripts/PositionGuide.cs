using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionGuide : MonoBehaviour {

    private Vector3 hitNormal;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float maxPlaceDistance = 2f;
    [SerializeField] private ItemInteraction ObjInteractScript;
    [SerializeField] private Collider[] SphereCastExemptColliders;
    [SerializeField] private Transform _carryGuide;

    private void Start() {
        GetComponent<Renderer>().enabled = false;
    }

    void Update() {
        if (!ObjInteractScript.holding) {
            GetComponent<Renderer>().enabled = false;
            return;
        }
        if (Input.GetButtonDown("Interact")) GetComponent<Renderer>().enabled = true;
        if (Input.GetButtonUp("Interact")) GetComponent<Renderer>().enabled = false;

        var allHits = Physics.SphereCastAll(gameCamera.transform.position, .1f, gameCamera.transform.forward, maxPlaceDistance);
        gameObject.transform.rotation = Quaternion.identity;

        IEnumerable<RaycastHit> validHits = null;
        if (allHits != null && allHits.Length > 0)
        {
            allHits = allHits.OrderBy(h => h.distance).ToArray();
            validHits = allHits.Where(h => (h.collider == null
                || !SphereCastExemptColliders.Contains(h.collider))
                && !h.transform.IsChildOf(_carryGuide));
        }
        if (validHits != null && validHits.Any())
        {
            var firstValidHit = validHits.First();
            gameObject.transform.position = gameCamera.transform.position + gameCamera.transform.forward * firstValidHit.distance;
            hitNormal = firstValidHit.normal;
        } else
        {
            gameObject.transform.position = gameCamera.transform.position + gameCamera.transform.forward * maxPlaceDistance;
        }
        // Consolidate this and PickUp.cs raycast code
    }

    public Vector3 GetHitNormal() {
        return hitNormal;
    }
}
