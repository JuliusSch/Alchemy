using UnityEngine;

public class TurtleStateManager : MonoBehaviour, IGetTimeNotifications
{
    public enum TurtleState
    {
        ARRIVING, OPEN, LEAVING, NOT_THERE
    }

    private TurtleState _currentState;
    public TurtleMovement Movement;
    public TurtleMerchant Merchant;

    public TurtleState CurrentState => _currentState;

    public void SetState(TurtleState state)
    {
        switch (state)
        {
            case TurtleState.ARRIVING:
                _currentState = TurtleState.ARRIVING;
                break;
            case TurtleState.OPEN:
                _currentState = TurtleState.OPEN;
                Merchant.OpenShop();
                Merchant.StockShop();
                break;
            case TurtleState.LEAVING:
                _currentState = TurtleState.LEAVING;
                break;
            case TurtleState.NOT_THERE:
                _currentState = TurtleState.NOT_THERE;
                break;
            default: break;
        }
    }

    public void Notify(int time)
    {
        if (time == 110)
        {
            SetState(TurtleState.ARRIVING);
            //_movement.MoveIn();             commented out for testing
        }
        if (time == 450)
        {
            SetState(TurtleState.LEAVING);
        }
        if (time == 600)
        {
            SetState(TurtleState.NOT_THERE);
        }
    }
}
