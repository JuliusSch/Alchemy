using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PageRight : MonoBehaviour, IInteractable
{
    public AlchemistBook book;
    public GameObject PageFlipR2LPrefab;
    private List<GameObject> _currentRightFlips = new List<GameObject>();

    public void PrimaryInteraction(Transform heldObject, PickUp pickUpScript)
    {
        bool endReached = !book.NextPage();
        if (!endReached)
        {
            GetComponent<AudioSource>().Play();
            var flipInstance = Instantiate(PageFlipR2LPrefab, transform.parent);
            _currentRightFlips.Add(flipInstance);
        }
    }

    void Update()
    {
        var toBeDestroyed = new List<GameObject>();
        foreach (var flip in _currentRightFlips)
        {
            if (flip.GetComponent<PlayableDirector>().state == PlayState.Paused)
                toBeDestroyed.Add(flip);
        }
        foreach (var flip in toBeDestroyed)
        {
            _currentRightFlips.Remove(flip);
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
