using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionGuide : MonoBehaviour {

    private Vector3 hitNormal;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float maxPlaceDistance = 2f;
    [SerializeField] private PickUp pickUpScript;
    [SerializeField] Collider[] SphereCastExemptColliders;

    private void Start() {
        GetComponent<Renderer>().enabled = false;
    }

    void Update() {
        if (!pickUpScript.holding) {
            GetComponent<Renderer>().enabled = false;
            return;
        }
        if (Input.GetButtonDown("Interact")) GetComponent<Renderer>().enabled = true;
        if (Input.GetButtonUp("Interact")) GetComponent<Renderer>().enabled = false;

        var allHits = Physics.SphereCastAll(gameCamera.transform.position, .1f, gameCamera.transform.forward, maxPlaceDistance);
        gameObject.transform.rotation = Quaternion.identity;

        if (allHits != null && allHits.Length > 0)
        {
            allHits = allHits.OrderBy(h => h.distance).ToArray();
            var firstValidHit = allHits.FirstOrDefault(h => h.collider == null || !SphereCastExemptColliders.Contains(h.collider));
            gameObject.transform.position = gameCamera.transform.position;
            gameObject.transform.position = gameCamera.transform.position + gameCamera.transform.forward * firstValidHit.distance;
            hitNormal = firstValidHit.normal;
        } else
        {
            gameObject.transform.position = gameCamera.transform.position + gameCamera.transform.forward * maxPlaceDistance;
        }
    }

    public Vector3 GetHitNormal() {
        return hitNormal;
    }
}
