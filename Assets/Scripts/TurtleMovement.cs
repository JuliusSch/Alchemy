using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static TurtleStateManager;

public class TurtleMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] EntryPath;
    private TurtleStateManager _stateManager;

    public Transform FloatParent;

    private void Awake()
    {
        _stateManager = GetComponent<TurtleStateManager>();

        MoveIn();
    }

    private void MoveIn()
    {
        FloatParent.DOPath(EntryPath.Select(p => p.transform.position).ToArray(), 30f, PathType.CatmullRom).SetEase(Ease.Flash).SetLookAt(-0.001f).onComplete = SetOpenShop;
    }

    private void SetOpenShop() => _stateManager.SetState(TurtleState.OPEN);
}
