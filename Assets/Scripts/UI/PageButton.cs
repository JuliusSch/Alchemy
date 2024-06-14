using UnityEngine;
using UnityEngine.Events;

public class PageButton : MonoBehaviour, IInteractableUI
{
    public UnityEvent OnClickAction, OnStartHover, OnEndHover;

    private bool _isHover;

    private void FixedUpdate()
    {
        if (_isHover)
        {
            _isHover = false;
            OnEndHover.Invoke();
        }
    }

    public void Hover()
    {
        if (!_isHover)
        {
            _isHover = true;
            OnStartHover.Invoke();
        }
    }

    public void Interact()
    {
        OnClickAction.Invoke();
    }
}
