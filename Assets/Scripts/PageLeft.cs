using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PageLeft : MonoBehaviour, IInteractable
{
    public AlchemistBook book;

    public GameObject PageFlipL2RPrefab;
    private List<GameObject> _currentLeftFlips = new List<GameObject>();

    public void PrimaryInteraction(Transform heldObject, PickUp pickUpScript)
    {
        bool beginningReached = !book.PrevPage();
        if (!beginningReached)
        {
            GetComponent<AudioSource>().Play();
            var flipInstance = Instantiate(PageFlipL2RPrefab, transform.parent);
            _currentLeftFlips.Add(flipInstance);
        }
    }

    void Update()
    {
        var toBeDestroyed = new List<GameObject>();
        foreach (var flip in _currentLeftFlips)
        {
            if (flip.GetComponent<PlayableDirector>().state == PlayState.Paused)
                toBeDestroyed.Add(flip);
        }
        foreach (var flip in toBeDestroyed)
        {
            _currentLeftFlips.Remove(flip);
            Destroy(flip);
        }
    }

    public GameObject getGroup()
    {
        throw new NotImplementedException();
    }

    public bool Interact(string key)
    {
        return false;
    }

    public void MouseOver()
    {
    }
}
