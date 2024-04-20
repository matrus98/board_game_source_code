using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Pawn : MonoBehaviour {

    [SerializeField]
    public Material PawnMaterial;// { get; private set; }
    public string MyName = "Anonymous";
    public int FreezedTurns { get; private set; }
    public bool IsMyTurn = false;

    [SerializeField]
    GameObject ListOfPurchasedFieldUIPrefab;
    GameObject TemporaryListOfPurchasedFieldUI;
    private List<Field> MyFields = new List<Field>();
    public bool IsListShowed { get; private set; }
    public bool CanPurchaseHotelThisTurnRemotely { get; private set; }

    Vector3[] PawnAvaibleMovement;

    //Parametry do kart
    public bool HalfMoney = false;
    private int BlueFreedom = 0;
    private bool RedFreedom = false;

    public int myMoney = 3000;
    public int MyMoney
    {
        get { return myMoney; }
        set
        {
            myMoney = value;

            if (OnMoneyChanged != null)
                OnMoneyChanged.Invoke(MyMoney);
        }
    }
    public event Action<int> OnMoneyChanged;

    GameObject CrisisUI;
    public event Action<Field, Pawn> OnNotEnoughtMoneyToPay;
    public event Action<Pawn, int> OnNotEnoughtMoney;
    public event Action OnPlayerBroke;

    public int currentIndex = 0;
    public int CurrentIndex
    {
        get { return currentIndex; }
        set
        {
            currentIndex = value;

            if (currentIndex >= 36)
            {
                currentIndex -= 36;
            }

            if (currentIndex < 0)
            {
                currentIndex += 36;
            }
        }
    }
    public int indexOfFieldIAmStayingOn = 0;
    public int IndexOfFieldIAmStayingOn
    {
        get { return indexOfFieldIAmStayingOn; }
        set
        {
            indexOfFieldIAmStayingOn = value;

            if (indexOfFieldIAmStayingOn >= 36)
            {
                indexOfFieldIAmStayingOn -= 36;
            }

            if (indexOfFieldIAmStayingOn < 0)
            {
                indexOfFieldIAmStayingOn += 36;
            }
            //Debug.Log("Value indexOfFieldIAmStayingOn increased to: " + indexOfFieldIAmStayingOn);
        }
    }

    private void DisplayCrisisUI(Field field, Pawn pawn)
    {
        CrisisUI = Instantiate(FindObjectOfType<GameManager>().GetCrisisUI());
        CrisisUI.GetComponent<CrisisUI>().Configure(field, pawn);
    }

    private void DisplayCardCrisisUI(Pawn player, int Fine)
    {
        CrisisUI = Instantiate(FindObjectOfType<GameManager>().GetCardCrisisUI());
        CrisisUI.GetComponent<CrisisUIForCards>().Configure(player, Fine);
    }

    private void Awake()
    {
        OnNotEnoughtMoneyToPay += DisplayCrisisUI;
        OnPlayerBroke += FindObjectOfType<GameManager>().DestroyPawnIfPlayerIsBroke;
        OnNotEnoughtMoney += DisplayCardCrisisUI;
    }

    void Start()
    {
        PawnAvaibleMovement = GetComponent<PawnAvaibleMovement>().GetPawnAvaibleMovementArray();
        FindObjectOfType<GameManager>().AddPlayerPawn(this);
        StartCoroutine(CheckIfPlayerHasNoMoreMoney());
        //PawnMaterial = FindObjectOfType<Player>().PlayerMaterial;
        //PawnMaterial = FindObjectOfType<GlobalSettingsPlayerColor>().GetRandomPlayerPawnColor();
        //FindObjectOfType<GameManager>().CmdSetMaterialForEntirePawn(gameObject);
    }

    void Update()
    {
        if (IsMyTurn && Input.GetKeyDown(KeyCode.L))
        {
            if (!IsListShowed)
                DisplayListOfMyFields();

            else DestroyTemporaryListOfPurchasedFieldUI();
        }

        Movement();
    }

    public void DisplayListForGameManager()
    {
        if (IsMyTurn)
        {
            if (!IsListShowed)
                DisplayListOfMyFields();

            else DestroyTemporaryListOfPurchasedFieldUI();
        }
    }

    private void DisplayListOfMyFields()
    {
        TemporaryListOfPurchasedFieldUI = Instantiate(ListOfPurchasedFieldUIPrefab);
        TemporaryListOfPurchasedFieldUI.GetComponentInChildren<ListOfPurchasedFieldsUI>().Configure(MyFields, this);
        IsListShowed = true;
    }

    public void DisableRemotePurchasing()
    {
        CanPurchaseHotelThisTurnRemotely = false;
    }

    public void DestroyTemporaryListOfPurchasedFieldUI()
    {
        Destroy(TemporaryListOfPurchasedFieldUI);
        IsListShowed = false;
    }

    public void RefreshTemporaryListOfPurchasedFieldUI()
    {
        DestroyTemporaryListOfPurchasedFieldUI();
        DisplayListOfMyFields();
    }

    public void AddPurchasedField(Field field)
    {
        MyFields.Add(field);

        if (IsListShowed)
        {
            RefreshTemporaryListOfPurchasedFieldUI();
        }
    }

    public void RemoveSelledField(Field field)
    {
        MyFields.Remove(field);

        if (IsListShowed)
        {
            RefreshTemporaryListOfPurchasedFieldUI();
        }
    }

    IEnumerator CheckIfPlayerHasNoMoreMoney()
    {
        yield return new WaitForSeconds(1);
        if (MyMoney != 3000)
            MyMoney = 3000;
    }

    private void Movement()
    {
        transform.position = Vector3.Lerp(transform.position, PawnAvaibleMovement[CurrentIndex], Time.deltaTime * 25f);

        if (transform.position == PawnAvaibleMovement[CurrentIndex] && CurrentIndex != IndexOfFieldIAmStayingOn)
            CurrentIndex++;
    }

    public void IncreseIndex(int eyes)
    {
        IndexOfFieldIAmStayingOn += eyes;
    }

    public void DecreaseIndex()
    {
        IndexOfFieldIAmStayingOn -= 1;
    }

    public void SetTurnToPlayer()
    {
        IsMyTurn = true;

        if (FreezedTurns > 0)
        {
            FreezedTurns -= 1;
            FindObjectOfType<GameManager>().EndTurnOfPlayer();
        }

        else
        {
            CanPurchaseHotelThisTurnRemotely = true;
            FindObjectOfType<CreateDices>().CanRollDices = true;
        }
    }

    public void FreezeMeOnTurns(int freezeTime)
    {
        FreezedTurns += freezeTime;
    }

    public void SendMeToPrison(int FreezeDurationInTurns)
    {
        if(BlueFreedom > 0)
        {
            BlueFreedom--;
            FreezedTurns = 0;
        }

        else if (RedFreedom)
        {
            RedFreedom = false;
            FreezedTurns = 0;
        }

        else
        {
            FreezeMeOnTurns(FreezeDurationInTurns);
            SendMeOnIndex(9);
            FindObjectOfType<GameManager>().EndTurnOfPlayer();
        }
    }

    public void SendMeOnIndex(int Index)
    {
        IndexOfFieldIAmStayingOn = Index;
        CurrentIndex = Index;
        transform.position = PawnAvaibleMovement[CurrentIndex];
    }

    public bool HaveIEnoughtMoney(int Fine)
    {
        if (MyMoney >= Fine)
            return true;

        else
        {
            if (OnNotEnoughtMoney != null)
                OnNotEnoughtMoney.Invoke(this, Fine);

            return false;
        }
    }

    public void ICannotPayAtLeastNow(Field field, Pawn PlayerWhoPay)
    {
        if (OnNotEnoughtMoneyToPay != null)
            OnNotEnoughtMoneyToPay.Invoke(field, PlayerWhoPay);
    }

    public void ICannotPayAtAll()
    {
        foreach (Field F in MyFields)
        {
            F.SellFieldIfPlayerIsStoppedPlaying(this);
        }

        var cameramain = FindObjectOfType<Camera>();
        var cameraUI = FindObjectsOfType<CameraUI>();

        if (cameramain.IsThisPawnTracked(this.transform))
        {
            if (cameraUI.Count() <= 0)
                FindObjectOfType<Camera>().SetDefaultPosition();
            else
                FindObjectOfType<CameraUI>().SetDefaultPositionOfCamera();
        }

        if (OnPlayerBroke != null)
            OnPlayerBroke.Invoke();
    }

    public int GetCountOfOwnedBlueFreedomCards()
    {
        return BlueFreedom;
    }

    public int GetCountOfOwnedFields()
    {
        return MyFields.Count();
    }

    public void IncreseBlueFreedom()
    {
        BlueFreedom++;
    }

    public void GiveRedFreedom()
    {
        RedFreedom = true;
    }
}
