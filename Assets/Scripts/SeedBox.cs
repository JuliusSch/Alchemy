using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBox : MonoBehaviour, IInteractable {

    [SerializeField] SeedSO seed;
    [SerializeField] private int time, growthTime, fruitTime;
    private bool hasSeed, fruiting, grown;
    private float timer = 0f, growthProgress;
    private GameObject plantModel;

    public void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript) {
        if (heldObject == null) return;
        SeedItem seedBag = heldObject.GetComponent<SeedItem>();
        if (seedBag && !hasSeed) {
            addNewSeed(seedBag.seedType);
            pickUpScript.DeleteHeld();
        }
    }

    public GameObject getGroup() {
        return null;
    }

    private void addNewSeed(SeedSO newSeed) {
        seed = newSeed;
        hasSeed = true;
        plantModel = Instantiate(seed.plantModelGrowing, transform);
        plantModel.transform.Rotate(90f, 0, 0);
    }

    public bool Interact(string key) {
        if (key == "e") harvest(); 
        Debug.Log("Seed box interacted with.");
        return true;
    }

    private bool harvest() {
        if (fruiting) {
            //give player plant produce
            fruiting = false;
            Destroy(plantModel);
            plantModel = Instantiate(seed.plantModel, transform);
            return true;
        }
        return false;
    }

    void Update() {
        if (hasSeed) {
            timer += Time.deltaTime;
            if (fruiting) return;
            if (timer > 1) {
                timer = 0;
                time++;
                if (growthTime < seed.growthTimeSec) growthTime++;
                else fruitTime++;
            }
            if (growthTime >= seed.growthTimeSec && grown == false) {
                grown = true;
                Destroy(plantModel);
                plantModel = Instantiate(seed.plantModel, transform);
            }
            if (fruitTime > seed.growthTimeSec) {
                fruitTime = 0;
                fruiting = true;
                Destroy(plantModel);
                plantModel = Instantiate(seed.plantModelFruiting, transform);
            }
            growthProgress = (float) growthTime / (seed.growthTimeSec);
            plantModel.transform.localScale = new Vector3(growthProgress, growthProgress, growthProgress);
        }
    }

    public void MouseOver() {
        // tooltip for planting, caring, harvesting
    }
}
