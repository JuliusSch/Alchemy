using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TurtleMerchant : MonoBehaviour, IGetTimeNotifications, ISaveable
{

    [SerializeField] private float time, moveSpeed = 0.05f, doorSpeed = .3f;
    [SerializeField] private GameObject startPoint, endPoint; // Obs
    [SerializeField] private GameObject[] EntryPath;
    [SerializeField] private int inventorySize;
    [SerializeField] private IngredientSO[] availableIngredients;
    [SerializeField] private SeedSO[] availableSeeds;
    [SerializeField] private ShopItemSlot[] inventorySlots;
    private Vector3 start, end;
    private IInteractable[] inventory;
    private MerchantState _currentState;
    //private Coroutine coroutine;
    //private Quaternion OpenLeft, ClosedLeft, OpenRight, ClosedRight, OpenLower, ClosedLower;

    public int playerMoney;

    public Transform LeftDoor, RightDoor, LowerDoor;
    //public Vector3 LeftAxis, RightAxis, LowerAxis;
    //public float LeftAngle, RightAngle, LowerAngle;
    public Vector3 LeftRotation = new Vector3(0, 150, 0), RightRotation = new Vector3(0, -150, 0), LowerRotation = new Vector3(0, 90, 0);

    private void Start() {
        start = startPoint.transform.position; // Obs
        end = endPoint.transform.position;  // Obs
        inventory = new IInteractable[inventorySize];

        //ClosedLeft = LeftDoor.localRotation;
        //ClosedRight = RightDoor.localRotation;
        //ClosedLower = LowerDoor.localRotation;
        //OpenLeft = Quaternion.AngleAxis(LeftAngle, LeftAxis);
        //OpenRight = Quaternion.AngleAxis(RightAngle, RightAxis);
        //OpenLower = Quaternion.AngleAxis(LowerAngle, LowerAxis);

        TimeManager.Instance.Subscribe(this);
    }

    public enum MerchantState
    {
        ARRIVING, OPEN, LEAVING, NOT_THERE
    }

    private void MoveIn() {
        transform.position = EntryPath[0].transform.position;
        transform.DOPath(EntryPath.Select(p => p.transform.position).ToArray(), 10f, PathType.CatmullRom).SetLookAt(0.001f).onComplete = OpenShop;
        //while (time < 1) {
        //    time += Time.deltaTime * moveSpeed;
        //    transform.position = Vector3.Lerp(start, end, time);
        //    yield return null;
        //}
        //time = 0;
        
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

    private void StockShop() {
        inventorySlots[0].AddContents(availableIngredients[0]);
        //shop must stock ingredients, seeds, perhaps empty bottles?
        //also contracts/requests and the ability to sell back concoctions to fulfil these.
    }

    private void OpenShop()
    {
        SetState(MerchantState.OPEN);
        StockShop();
        LeftDoor.DOLocalRotate(LeftRotation, 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce);
        RightDoor.DOLocalRotate(RightRotation, 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce);
        LowerDoor.DOLocalRotate(LowerRotation, 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce).SetDelay(1f);
        // move doors
        //StartCoroutine(OpenDoors());
    }

    private void OpenDoors()
    {
        //time = 0;
        //while (time < 1.5)
        //{
        //    time += Time.deltaTime * doorSpeed;
        //    if (time < 1)
        //    {
        //        LeftDoor.Rotate(LeftAxis, Time.deltaTime * doorSpeed * LeftAngle, Space.Self);
        //        RightDoor.Rotate(RightAxis, Time.deltaTime * doorSpeed * RightAngle, Space.Self);
        //    }
        //    if (time > 0.5)
        //        LowerDoor.Rotate(LowerAxis, Time.deltaTime * doorSpeed * LowerAngle, Space.Self);
        //    yield return null;
        //}
        ////coroutine = null;
        //time = 0;
    }

    private void SetState(MerchantState state)
    {
        switch (state)
        {
            case MerchantState.ARRIVING:
                _currentState = MerchantState.ARRIVING;
                break;
            case MerchantState.OPEN:
                _currentState = MerchantState.OPEN;
                //OpenShop();
                //StockShop();
                break;
            case MerchantState.LEAVING:
                _currentState = MerchantState.LEAVING;
                break;
            case MerchantState.NOT_THERE:
                _currentState = MerchantState.NOT_THERE;
                break;
            default: break;
        }
    }

    public void Notify(int time)
    {
        if (time == 110) {
            SetState(MerchantState.ARRIVING);
            MoveIn();
        }
        if (time == 450)
        {
            SetState(MerchantState.LEAVING);
        }
        if (time == 600)
        {
            SetState(MerchantState.NOT_THERE);
        }
    }

    public void Save(SaveData data)
    {
        data.MerchantSaveData = new MerchantSaveData {
            State = _currentState,
            StateInfo = _currentState switch {
                MerchantState.ARRIVING => string.Format("time|{0}", time),
                MerchantState.OPEN => "",
                MerchantState.LEAVING => string.Format("time|{0}", time),
                MerchantState.NOT_THERE => "",
                _ => "",
            }
        };
        if (_currentState == MerchantState.OPEN)
        {

        }
    }

    public void Load(SaveData data)
    {
        if (data.MerchantSaveData != null)
        {
            SetState(data.MerchantSaveData.State);
        }
    }
}
