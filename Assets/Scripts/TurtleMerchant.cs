using DG.Tweening;
using UnityEngine;

public class TurtleMerchant : MonoBehaviour, ISaveable
{

    [SerializeField] private float time, moveSpeed = 0.05f, doorSpeed = .3f;
    //[SerializeField] private GameObject[] EntryPath;
    [SerializeField] private int inventorySize;
    [SerializeField] private IngredientSO[] availableIngredients;
    [SerializeField] private SeedSO[] availableSeeds;
    [SerializeField] private ShopItemSlot[] inventorySlots;
    private IInteractable[] inventory;
    private TurtleStateManager _stateManager;

    public int playerMoney;

    public Transform LeftDoor, RightDoor, LowerDoor;
    public Vector3 LeftRotation = new(0, 150, 0), RightRotation = new(0, -150, 0), LowerRotation = new(0, 90, 0);

    private void Start() {
        _stateManager = GetComponent<TurtleStateManager>();
        inventory = new IInteractable[inventorySize];
    }

    public bool CanBuy(IInteractable item) {
        // not working with seedbags or contracts, only ingredientbottles;
        IngredientBottle bottle = item.gameObject.GetComponent<IngredientBottle>();
        if (bottle.IngredientInst.data.shopValue < playerMoney) return true;
        return false;
    }

    public void Buy(ShopItemSlot slot) {
        IInteractable slotContents = slot.RemoveContents();
        IngredientBottle bottle = slotContents.gameObject.GetComponent<IngredientBottle>();
        playerMoney -= bottle.IngredientInst.data.shopValue;
        Debug.Log(slotContents.gameObject.name);
    }

    public void StockShop() {
        inventorySlots[0].AddContents(availableIngredients[0]);
        //shop must stock ingredients, seeds, perhaps empty bottles?
        //also contracts/requests and the ability to sell back concoctions to fulfil these.
    }

    public void OpenShop()
    {
        //SetState(MerchantState.OPEN);
        StockShop();
        LeftDoor.DOLocalRotate(LeftRotation, 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce);
        RightDoor.DOLocalRotate(RightRotation, 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce);
        LowerDoor.DOLocalRotate(LowerRotation, 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce).SetDelay(1f);
    }

    public void Save(SaveData data)
    {
        //data.MerchantSaveData = new MerchantSaveData {
        //    State = _currentState,
        //    StateInfo = _currentState switch {
        //        TurtleState.ARRIVING => string.Format("time|{0}", time),
        //        TurtleState.OPEN => "",
        //        TurtleState.LEAVING => string.Format("time|{0}", time),
        //        TurtleState.NOT_THERE => "",
        //        _ => "",
        //    }
        //};
        //if (_currentState == TurtleState.OPEN)
        //{

        //}
    }

    public void Load(SaveData data)
    {
        //secondary state manager? also handles saving?

        //if (data.MerchantSaveData != null)
        //{
        //    SetState(data.MerchantSaveData.State);
        //}
    }
}
