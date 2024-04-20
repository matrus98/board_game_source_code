using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardNotes : MonoBehaviour
{
    private string[] DescriptionsArrayBlue = new string[]
    {
        "You are considered as the\nbest participant in capitalistic\ncompetition where the main\naward is 200$.",               // 1

        "A cost of loans has risen\nunfortunately you had taken\none. You lose 300$.",                                           // 2

        "Congrats, Your exceptional\n skills in work have led You\nstraight on seat of employee\nof the month! 500$",            // 3

        "How dare You! Not paying\ntaxes is highly illegal.\nYou have to bear a fine 500$.\nPrison welcome on 1 turn!",          // 4

        "Cannot be! Is it a joke\nOr You have just introduced\nfees on free parking?\nYou earn 50$ as Players\nstay there. Rip-off!", // 5

        "Nothing. Or wait\nnothing also costs. 50$ Please.",                                                                     // 6

        "High time to go on\nlittle business trip.\nToward Washington.",                                                          // 7

        "You go back on Start.",                                                                                                  // 8

        "Move 3 steps\nforward.",                                                                                                // 9

        "You can go out of prison. These\ncards you can endlessly collect\nbut with every subsequent\nYou pay 100$ for each owned\nbefore!" +
        " Lose if can't instantly.", // 10

        "Did You know that\nChicago thought is a USA's city\nthis is the most ihabitated\ncity by Poles in the world?",

        "Within a program\nof student exchange you go\n to Dublin"
    };
    private string[] DescriptionsArrayRed = new string[]
    {
        "You are considered as the\nbest participant in communistic\ncompetition where the main\naward is 200$.",       // 1

        "You are under suspicious\nof being a western spy.\nYou have to bear a costs\nof the controll 200$.",           // 2

        "You have earn a free trip\nto the capital city\nof Russia Federation.\n Toward Moscow!",                       // 3

        "You have been always\ninto building and it is high time\nto realise the dream.\nYou go on trip to Legoland!",  // 4

        "You are long time\nin journey so it is time\nto make a break.\nGo on free parking.",                           // 5

        "Hmmmm... Nothing?",                                                                                            // 6

        "Cyprus a warm island\nLet's visit it!\nToward Paphos." ,                                                      // 7

        "Return on start. Great\nyou have more money\nnow. We will take only\nhalf of it for that\nfavor. Thank You!", // 8

        "You go backward\nas many times as count\nof fields you have.",                                                // 9

        "You can go out of\nprison if you get there.\nAs you earn another lose it.",                                   // 10

        "Wonderful capital city\nof Hungary Budapest.\nA place truely worth to see.",

        "Oslo - Capital city of Norway\nand country of beautiful fjords\nLet's take a look!"
    };
    private int Index = 0;

    public string GetRandomNote(QuestionMarkColor questionMarkColor)
    {
        if(questionMarkColor == QuestionMarkColor.Blue)
        {
            Index = Random.Range(0, DescriptionsArrayBlue.Length);

            if (CheckForSpecialEvent(Index))
                return SpecialNote(Index, questionMarkColor);

            else return DescriptionsArrayBlue[Index];
        }

        else
        {
            Index = Random.Range(0, DescriptionsArrayRed.Length);

            if (CheckForSpecialEvent(Index))
                return SpecialNote(Index, questionMarkColor);

            return DescriptionsArrayRed[Index];
        }
    }

    public void PerformAction(Pawn Player, QuestionMarkColor questionMarkColor)
    {
        if(questionMarkColor == QuestionMarkColor.Blue)
        {
            switch (Index)
            {
                case 0: Blue1NumberActionToCard(Player); break;
                case 1: Blue2NumberActionToCard(Player); break;
                case 2: Blue3NumberActionToCard(Player); break;
                case 3: Blue4NumberActionToCard(Player); break;
                case 4: Blue5NumberActionToCard(Player); break;
                case 5: Blue6NumberActionToCard(Player); break;
                case 6: Blue7NumberActionToCard(Player); break;
                case 7: Blue8NumberActionToCard(Player); break;
                case 8: Blue9NumberActionToCard(Player); break;
                case 9: Blue10NumberActionToCard(Player); break;
                case 10: Blue11NumberActionToCard(Player); break;
                case 11: Blue12NumberActionToCard(Player); break;
            }
        }

        else
        {
            switch (Index)
            {
                case 0: Red1NumberActionToCard(Player); break;
                case 1: Red2NumberActionToCard(Player); break;
                case 2: Red3NumberActionToCard(Player); break;
                case 3: Red4NumberActionToCard(Player); break;
                case 4: Red5NumberActionToCard(Player); break;
                case 5: break;
                case 6: Red6NumberActionToCard(Player); break;
                case 7: Red7NumberActionToCard(Player); break;
                case 8: Red8NumberActionToCard(Player); break;
                case 9: Red9NumberActionToCard(Player); break;
                case 10: Red10NumberActionToCard(Player); break;
                case 11: Red11NumberActionToCard(Player); break;
            }
        }
    }

    #region Particular Actions

    #region BlueActions
    private void Blue1NumberActionToCard(Pawn Player)
    {
        Player.MyMoney += 200;
        ActiveEndTurnButton();
    }

    private void Blue2NumberActionToCard(Pawn Player)
    {
        var fineToPay = 300;

        if (CheckIfPlayerHasEnoughtMoney(Player, fineToPay))
        {
            Player.MyMoney -= fineToPay;
            ActiveEndTurnButton();
        }

        else DisplayCrisisUIForCard(Player, fineToPay);
    }

    private void Blue3NumberActionToCard(Pawn Player)
    {
        Player.MyMoney += 500;
        ActiveEndTurnButton();
    }

    private void Blue4NumberActionToCard(Pawn Player)
    {
        var fineToPay = 500;

        if (CheckIfPlayerHasEnoughtMoney(Player, fineToPay))
        {
            Player.MyMoney -= fineToPay;
            
            Player.SendMeToPrison(1);

            ActiveEndTurnButton();
        }

        else
        {
            Player.FreezeMeOnTurns(1);
            DisplayCrisisUIForCard(Player, fineToPay);
        }
    }

    private void Blue5NumberActionToCard(Pawn Player)
    {
        FindObjectOfType<FreeParkingRipOff>().SetRipOff(Player, 50);
        ActiveEndTurnButton();
    }

    private void Blue6NumberActionToCard(Pawn Player)
    {
        var fineToPay = 50;

        if (CheckIfPlayerHasEnoughtMoney(Player, fineToPay))
        {
            Player.MyMoney -= fineToPay;
            ActiveEndTurnButton();
        }

        else DisplayCrisisUIForCard(Player, fineToPay);
    }

    private void Blue7NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Washington"));
        ActiveEndTurnButton();
    }

    private void Blue8NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(0);
        ActiveEndTurnButton();
    }

    private void Blue9NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex( (Player.IndexOfFieldIAmStayingOn + 3) );
        ActiveEndTurnButton();
    }

    private void Blue10NumberActionToCard(Pawn Player)
    {
        int fineToPay = Player.GetCountOfOwnedBlueFreedomCards() * 50;

        if (CheckIfPlayerHasEnoughtMoney(Player, fineToPay))
        {
            Player.MyMoney -= fineToPay;
            Player.IncreseBlueFreedom();
            ActiveEndTurnButton();
        }

        else DisplayCrisisUIForCard(Player, fineToPay);
    }

    private void Blue11NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Chicago"));
        ActiveEndTurnButton();
    }

    private void Blue12NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Dublin"));
        ActiveEndTurnButton();
    }
    #endregion

    #region RedActions
    private void Red1NumberActionToCard(Pawn Player)
    {
        Player.MyMoney += 200;
        ActiveEndTurnButton();
    }

    private void Red2NumberActionToCard(Pawn Player)
    {
        var fineToPay = 200;

        if (CheckIfPlayerHasEnoughtMoney(Player, fineToPay))
        {
            Player.MyMoney -= fineToPay;
            ActiveEndTurnButton();
        }

        else DisplayCrisisUIForCard(Player, fineToPay);
    }

    private void Red3NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Moscow"));
        ActiveEndTurnButton();
    }

    private void Red4NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Billund Legoland"));
        ActiveEndTurnButton();
    }

    private void Red5NumberActionToCard(Pawn Player)
    {
        var rp = FindObjectOfType<FreeParkingRipOff>();

        if (rp.IsRipOffSet())
            rp.UnsetRipOff();

        Player.SendMeOnIndex(18);
        ActiveEndTurnButton();
    }

    private void Red6NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Paphos"));
        ActiveEndTurnButton();
    }

    private void Red7NumberActionToCard(Pawn Player)
    {
        Player.HalfMoney = true;
        Player.SendMeOnIndex(0);
        ActiveEndTurnButton();
    }

    private void Red8NumberActionToCard(Pawn Player)
    {
        var DestinyIndex = Player.IndexOfFieldIAmStayingOn - Player.GetCountOfOwnedFields();

        Player.SendMeOnIndex(DestinyIndex);
        ActiveEndTurnButton();
    }

    private void Red9NumberActionToCard(Pawn Player)
    {
        Player.GiveRedFreedom();
        ActiveEndTurnButton();
    }

    private void Red10NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Budapest"));
        ActiveEndTurnButton();
    }

    private void Red11NumberActionToCard(Pawn Player)
    {
        Player.SendMeOnIndex(GetIndexOfFieldThatNameIs("Oslo"));
        ActiveEndTurnButton();
    }
    #endregion

    private void ActiveEndTurnButton()
    {
        FindObjectOfType<DisableEndTurnButton>().DisableButton(false);
    }

    private bool CheckIfPlayerHasEnoughtMoney(Pawn player, int fine)
    {
        if (player.MyMoney >= fine)
            return true;

        else return false;
    }

    private void DisplayCrisisUIForCard(Pawn player, int fine)
    {
        player.HaveIEnoughtMoney(fine);
    }

    private int GetIndexOfFieldThatNameIs(string name)
    {
        foreach (Field F in FindObjectsOfType<Field>())
        {
            if (F.FieldType.Description == name)
                return F.MyIndex;
        };

        return -1;
    }

    private bool CheckForSpecialEvent(int index)
    {
        if (index == 4 && FindObjectOfType<FreeParkingRipOff>().IsRipOffSet())
            return true;

        return false;
    }

    private string SpecialNote(int index, QuestionMarkColor questionMarkColor)
    {
        if(questionMarkColor == QuestionMarkColor.Blue)
        {
            if (index == 4)
                return "This is ridiculous! You\nhave taken over f(r)ee parking\non your own. May the\nripping-off never ends! *Pigs*";
        }

        else
        {
            if (index == 4)
                return "It is done with f(r)ee parking!\nYou releases it from stupid fees\nand go there to enjoy now\ntruely free parking.";
        }

        return null;
    }
    #endregion
}
