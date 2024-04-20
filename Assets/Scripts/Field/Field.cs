using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // UI
    [SerializeField]
    private GameObject PurchaseUI;
    private GameObject TemporaryCanva;

    public FieldType FieldType { private set; get; }
    private Material DefaultMaterial;
    private Pawn OwnerOfThisField = null;
    public int MyIndex { private set; get; }

    [SerializeField]
    private GameObject ExclamationMortgageMarkPrefab;
    private GameObject TemporaryExclamationMortgageMarkPrefab;
    public bool IsUnderMortgage = false;
    public bool IsExhibited = false;

    //Parametry przy UI
    private bool CanPurchase = false;
    private bool CanShowInfo = false;
    private bool IsInfoShowed = false;
    private Pawn PlayerWhoHasEntered;
    private string OwnerName = "None";

    //Hotele
    [SerializeField]
    private GameObject HousePrefab;
    [SerializeField]
    private GameObject HotelPrefab;
    FieldCountryCounter CountryCountChecker;
    private const int PriceOfHouse = 100;
    private int CountOfHotels = 0;
    private bool CanPurchaseHotelThisTurn = true;
    private Vector3[] HotelsPositionsArray;

    //Eventy
    public event Action<string, string> OnFieldPurchase;
    public event Action<string,int,string> OnSomsiadBuli;
    public event Action<string, string, string, int> OnRepurchasedField;

    //Te rzeczy są potrzebne do konfiguracji w tym skrypcie z obiektu klasy FieldType
    //public int Price;
    //public bool HasOwner;
    //public Vector3 Position;

    //Te rzeczy są potrzebne do konfiguracji obiektów wizualnych dzieci
    //public string Description;
    //public Material CountryColor;
    //public Material BelongingColor;

    /// <summary>
    /// Konfiguruje pole (Ustawia cene, właściciela, pozycje, opis i materiały);
    /// </summary>
    /// <param name="fieldType"></param>
    public void Configure(FieldType fieldType, int index, int NumberOfRotations)
    {
        MyIndex = index + NumberOfRotations + 1; // Rotacja na 90 + 1 (  (obrót/90) + 1 )

        if (fieldType.IsQuestionMarkField)
        {
            Destroy(gameObject);
            GetComponentInParent<Board>().GenerateQuestionMark(fieldType.Position, MyIndex);
        } else
        {
            FieldType = fieldType;

            GetComponentInChildren<CountryColor>().Configure(FieldType.CountryColor);
            GetComponentInChildren<DescriptionOfTheField>().Configure(FieldType.Description, FieldType.Price);
            GetComponentInChildren<PlayerBelongingColor>().Configure(FieldType.BelongingColor);

            DefaultMaterial = FieldType.BelongingColor;

            transform.position = FieldType.Position;

            FindObjectOfType<FieldCountryCounter>().SetNumberOFCountryFields(FieldType.CountryColor);
        }
    }

    private void Awake()
    {
        OnFieldPurchase += FindObjectOfType<AnnounmentCanva>().OnFieldPurchaseFunction;
        OnSomsiadBuli += FindObjectOfType<AnnounmentCanva>().OnSomsiadBuliFunction;
        OnRepurchasedField += FindObjectOfType<AnnounmentCanva>().OnRepurchasedFieldFunction;
    }

    private void Start()
    {
        CountryCountChecker = FindObjectOfType<FieldCountryCounter>();
        HotelsPositionsArray = GetComponent<HousesAndHotelPosition>().GetHousesAndHotelPositionArray();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && CanShowInfo)
        {
            if (!IsInfoShowed)
            {
                CreateTemporaryCanvaFunction();
            }
            else
            {
                DestroyTemporaryCanvaFunction();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<ConstantInfoUserInterface>().AttachUserInterfaceDescriptionOfTheField(this);

        CanPurchaseHotelThisTurn = true;

        var obj = other.gameObject.GetComponent<Pawn>();

        if (obj == null)
            return;

        if(obj.IndexOfFieldIAmStayingOn == MyIndex)
        {
            PlayerWhoHasEntered = obj;

            if (!FieldType.HasOwner)
                CanPurchase = true;
            
            CanShowInfo = true;

            if(obj.IsMyTurn)
                StartCoroutine(PaySomsiadCoroutine());
        }

        if (obj != null && obj.IndexOfFieldIAmStayingOn == MyIndex)
            StartCoroutine(EnableEndTurButtonCoroutine());
    }

    private void OnTriggerStay(Collider other)
    {
        var player = other.gameObject.GetComponent<Pawn>();
        if (player == null)
            return;

        if (CheckWhetherCanPurchase())
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (player.MyMoney < FieldType.Price)
                {
                    //Debug.Log("Brak Piniendzy");
                }
                else
                {
                    PurchaseField(player);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            CreateHouse();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SellField();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(CanPurchase)
            CanPurchase = false;

        CanShowInfo = false;
        IsInfoShowed = false;
        CanPurchaseHotelThisTurn = false;

        Destroy(TemporaryCanva);
    }

    private bool CheckWhetherCanPurchase()
    {
        return CanPurchase && !FieldType.HasOwner && PlayerWhoHasEntered.IsMyTurn;
    }

    public bool CheckWhetherICanShowInfo()
    {
        return CanShowInfo && !IsInfoShowed;
    }
    
    public void CreateTemporaryCanvaFunction()
    {
        TemporaryCanva = Instantiate(PurchaseUI);

        TemporaryCanva.GetComponentInChildren<FieldInfoUserInterface>()
            .SetUserInterfaceDescriptionOfTheField(FieldType.Description, FieldType.Price, OwnerName, this);

        IsInfoShowed = true;
    }

    private void RefreshCanva()
    {
        TemporaryCanva.GetComponentInChildren<FieldInfoUserInterface>()
            .SetUserInterfaceDescriptionOfTheField(FieldType.Description, FieldType.Price, OwnerName, this);
    }

    public void DestroyTemporaryCanvaFunction()
    {
        Destroy(TemporaryCanva);
        IsInfoShowed = false;
    }

    public void PurchaseByButtonClick()
    {
        //if (PlayerWhoHasEntered.MyMoney >= FieldType.Price)
        if (PlayerWhoHasEntered.MyMoney < FieldType.Price)
            return;

        else PurchaseField(PlayerWhoHasEntered);
    }

    private void PurchaseField(Pawn player)
    {
        if (!PlayerWhoHasEntered.IsMyTurn || !CanPurchase)
            return;

        player.MyMoney -= FieldType.Price;

        FieldType.BelongingColor = PlayerWhoHasEntered.PawnMaterial;
        GetComponentInChildren<PlayerBelongingColor>().Configure(FieldType.BelongingColor);
        OwnerName = player.MyName;
        OwnerOfThisField = player;

        FieldType.HasOwner = true;

        player.AddPurchasedField(this);
        
        if (OnFieldPurchase != null)
            OnFieldPurchase.Invoke(player.MyName, FieldType.Description);

        CanPurchase = false;

        if (IsInfoShowed)
        {
            RefreshCanva();
        }
    }

    public void CreateHouseWhereEverIAm(Pawn player)
    {
        if (!player.IsMyTurn)
            return;

        if (!CountryCountChecker.DidIOwnedThemAll(FieldType, player.MyName))
            return;

        if (player.Equals(null) || player.MyName != OwnerName)
            return;

        if (!FieldType.HasOwner || !player.CanPurchaseHotelThisTurnRemotely)
            return;

        if (CountOfHotels >= 5)
            return;

        CanPurchaseHotelThisTurn = false;
        player.DisableRemotePurchasing();
        player.MyMoney -= PriceOfHouse;

        if (CountOfHotels >= 4)
        {
            DestroyAllHousesAndBuildHotel();
            CountOfHotels = 5;
        }
        else
        {
            var House = Instantiate(HousePrefab);
            House.transform.parent = transform;
            House.transform.localRotation = Quaternion.Euler(Vector3.zero);
            House.transform.localPosition = HotelsPositionsArray[CountOfHotels];

            CountOfHotels++;
        }

        //Debug.Log("CountOfHotels Of Hotels: " + CountOfHotels);
    }

    public void CreateHouse()
    {
        if (!PlayerWhoHasEntered.IsMyTurn)
            return;

        if (!CountryCountChecker.DidIOwnedThemAll(FieldType, PlayerWhoHasEntered.MyName))
            return;

        if (PlayerWhoHasEntered.Equals(null) || PlayerWhoHasEntered.MyName != OwnerName)
            return;

        if (!CanShowInfo || !FieldType.HasOwner || !CanPurchaseHotelThisTurn)
            return;

        if (CountOfHotels >= 5)
            return;
        
        CanPurchaseHotelThisTurn = false;
        PlayerWhoHasEntered.DisableRemotePurchasing();
        PlayerWhoHasEntered.MyMoney -= PriceOfHouse;

        if (CountOfHotels >= 4)
        {
            DestroyAllHousesAndBuildHotel();
            CountOfHotels = 5;
        }
        else
        {
            var House = Instantiate(HousePrefab);
            House.transform.parent = transform;
            House.transform.localRotation = Quaternion.Euler(Vector3.zero);
            House.transform.localPosition = HotelsPositionsArray[CountOfHotels];

            CountOfHotels++;
        }

        //Debug.Log("CountOfHotels Of Hotels: " + CountOfHotels);
    }

    private void DestroyAllHousesAndBuildHotel()
    {
        DestroyAllHousesThatFieldHas();

        var Hotel = Instantiate(HotelPrefab);
        Hotel.transform.parent = transform;
        Hotel.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Hotel.transform.localPosition = HotelsPositionsArray[4];
    }

    private void DestroyAllHousesThatFieldHas()
    {
        var Houses = GetComponentsInChildren<Transform>();

        foreach (Transform x in Houses)
        {
            if (x.name.Contains("House"))
            {
                Destroy(x.gameObject);
            }
        }

        CountOfHotels = 0;
    }

    public void DestroyAllHousesThatFieldHasV2(FieldCountryCounter fieldCountryCounter)
    {
        if(fieldCountryCounter != null)
        {
            DestroyAllHousesThatFieldHas();
        }
    }

    public void SellFieldByButtonClick()
    {
        SellField();
    }

    private void SellField()
    {
        if (!PlayerWhoHasEntered.IsMyTurn)
            return;

        if (PlayerWhoHasEntered.MyName != OwnerName)
            return;

        if (IsUnderMortgage)
        {
            PlayerWhoHasEntered.MyMoney += ((FieldType.Price / 2) - GetMortgageValue());
            ReedemMortgage();
        }
        else
        {
            PlayerWhoHasEntered.MyMoney += (FieldType.Price / 2);
        }
        
        PlayerWhoHasEntered.MyMoney += WorthOfHouses();
        
        CanPurchaseHotelThisTurn = true;
        DestroyAllHousesThatFieldHas();

        CountryCountChecker.DestroyHousesIfPlayerSellField(FieldType, OwnerName, PlayerWhoHasEntered);

        FieldType.HasOwner = false;
        OwnerName = "None";

        GetComponentInChildren<PlayerBelongingColor>().Configure(DefaultMaterial);

        OwnerOfThisField.RemoveSelledField(this);
        OwnerOfThisField = null;
        CanPurchase = true;

        if (IsExhibited)
            IsExhibited = false;

        FindObjectOfType<GameManager>().RemoveFieldFromAutionIfWasExhibited(this);

        if (IsInfoShowed)
            RefreshCanva();
    }

    public void SellFieldWhereEverAmI(Field field, Pawn pawn, int MortgageValue)
    {
        if (!field.Equals(this) || !pawn.Equals(OwnerOfThisField))
            return;
        
        if (!pawn.IsMyTurn || pawn.MyName != OwnerName)
            return;

        if (IsUnderMortgage)
        {
            pawn.MyMoney += ((FieldType.Price / 2) - MortgageValue);
            ReedemMortgage();
        }
        else
        {
            pawn.MyMoney += (FieldType.Price / 2);
        }
        pawn.MyMoney += WorthOfHouses();

        DestroyAllHousesThatFieldHas();

        CountryCountChecker.DestroyHousesIfPlayerSellField(FieldType, OwnerName, pawn);

        FieldType.HasOwner = false;
        OwnerName = "None";

        GetComponentInChildren<PlayerBelongingColor>().Configure(DefaultMaterial);

        OwnerOfThisField.RemoveSelledField(this);
        OwnerOfThisField = null;
        CanPurchase = true;

        if (IsExhibited)
            IsExhibited = false;

        FindObjectOfType<GameManager>().RemoveFieldFromAutionIfWasExhibited(this);

        if (IsInfoShowed)
            RefreshCanva();
    }

    public void SellFieldIfPlayerIsStoppedPlaying(Pawn pawn)
    {
        DestroyAllHousesThatFieldHas();

        CountryCountChecker.DestroyHousesIfPlayerSellField(FieldType, OwnerName, pawn);

        FieldType.HasOwner = false;
        OwnerName = "None";

        GetComponentInChildren<PlayerBelongingColor>().Configure(DefaultMaterial);
        
        OwnerOfThisField = null;

        if (IsUnderMortgage)
            ReedemMortgage();

        if (IsExhibited)
            FindObjectOfType<GameManager>().RemoveFromAuction(this, pawn);

        if (IsInfoShowed)
            RefreshCanva();
    }
    
    public string WhatIsTheOwnerName()
    {
        return OwnerName;
    }

    public int WorthOfHouses()
    {
        return CountOfHotels * PriceOfHouse / 2;
    }

    public int HowMuchSomsiadBuli()
    {
        return ((FieldType.Price / 10) + (CountOfHotels * 50)) * 2;
    }

    IEnumerator PaySomsiadCoroutine()
    {
        yield return new WaitForSeconds(1);

        if (!IsUnderMortgage &&
            FieldType.HasOwner && 
            !OwnerOfThisField.Equals(null) &&
            PlayerWhoHasEntered.IndexOfFieldIAmStayingOn == MyIndex &&
            !PlayerWhoHasEntered.Equals(OwnerOfThisField))
        {
            if (PlayerWhoHasEntered.MyMoney < HowMuchSomsiadBuli())
            {
                //Debug.Log("UUUUUU Somsiad nie ma kasy hehe ");
                PlayerWhoHasEntered.ICannotPayAtLeastNow(this, PlayerWhoHasEntered);
            }
            else
            {
                //Debug.Log("SOMSIAD PŁACI!!! " + HowMuchSomsiadBuli() + " $\nPole nr: " + MyIndex);
                SomsiadPayForEnterHere();
            }
        }
    }

    private void SomsiadPayForEnterHere()
    {
        PlayerWhoHasEntered.MyMoney -= HowMuchSomsiadBuli();
        OwnerOfThisField.MyMoney += HowMuchSomsiadBuli();

        if (OnSomsiadBuli != null)
            OnSomsiadBuli.Invoke(PlayerWhoHasEntered.MyName ,HowMuchSomsiadBuli(), OwnerOfThisField.MyName);         //  SOMSIAD DAWAJ KASE!
    }

    public void OnTurnEndDisableShowingInfo()
    {
        if (CanShowInfo)
        {
            CanShowInfo = false;
            DestroyTemporaryCanvaFunction();
        }
    }

    public void SetUnderMortage()
    {
        IsUnderMortgage = true;
        TemporaryExclamationMortgageMarkPrefab = Instantiate(ExclamationMortgageMarkPrefab);
        TemporaryExclamationMortgageMarkPrefab.transform.parent = transform;
        TemporaryExclamationMortgageMarkPrefab.transform.localPosition = new Vector3(0.35f, 0.01f, 0);
    }

    public void ReedemMortgage()
    {
        Destroy(TemporaryExclamationMortgageMarkPrefab);
        IsUnderMortgage = false;
    }

    public int GetMortgageValue()
    {
        return FieldType.Price / 5;
    }

    public void ChangeOwner(string newOwner)
    {
        OwnerName = newOwner;
    }

    public void ChangeOwnerObject(Pawn pawn)
    {
        OwnerOfThisField = pawn;
    }

    public Pawn GetOwnerOfThisField()
    {
        return OwnerOfThisField;
    }

    public void OnRepurchasingField(string PlayerWhoRepurchased, string FromWho, int price)
    {
        if (OnRepurchasedField != null)
            OnRepurchasedField.Invoke(PlayerWhoRepurchased, FromWho, FieldType.Description, price);

        if (IsInfoShowed)
        {
            RefreshCanva();
        }
    }

    IEnumerator EnableEndTurButtonCoroutine()
    {
        yield return new WaitForSeconds(1);

        if(PlayerWhoHasEntered != null && PlayerWhoHasEntered.IndexOfFieldIAmStayingOn == MyIndex)
        {
            FindObjectOfType<DisableEndTurnButton>().DisableButton(false);
        }
    }
}
