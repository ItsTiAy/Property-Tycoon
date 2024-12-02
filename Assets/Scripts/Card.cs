using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all the cards
/// </summary>
[System.Serializable]
public class Card
{
    public string description;
    public Board board;
    //public string action;
    //public string type;

    //public CardGiveMoney[] gainMoney;
    //public CardFine[] loseMoney;

    /// <summary>
    /// The action performed by the card
    /// </summary>
    /// <param name="player"></param>
    public virtual void performCard(Player player){}

    /// <summary>
    /// Sets the board so certain cards can access methods
    /// </summary>
    /// <param name="board">The board to be set</param>
    public void SetBoard(Board board)
    {
        this.board = board;
    }
}

/// <summary>
/// Pays the player money
/// </summary>
[System.Serializable]
public class GainMoney : Card
{
    public int amount;

    public override void performCard(Player player)
    {
        player.IncreaseMoney(amount);
    }

}

/// <summary>
/// Fines the player money
/// </summary>
[System.Serializable]
public class LoseMoney : Card
{
    public int amount;
    private FreeParking freeParking;

    public override void performCard(Player player)
    {
        freeParking = (FreeParking) board.GetBoardTiles()[20];

        // If the card description contains the word "fine" adds the money to free parking
        if (description.ToLower().Contains("fine"))
        {
            freeParking.AddFineMoney(amount);
        }
        player.DecreaseMoney(amount);
    }
}
/*
//Fines the player money and puts it in free parking
public class CardFineToFreeParking : Card
{
    public int fineAmount;

    public override void performCard(Player player)
    {
        Bank.addToFreeParking(fineAmount); //change to only give the amount the player can afford? check spec
        player.DecreaseMoney(fineAmount); //probably do that by making finePlayer return 2 values, bool and maxPossibleMoney
    }
}

//Moves the player backwards (doesn't award money for passing go)
public class CardTeleport : Card
{ //tested
    public BoardTile destination;
    bool jail;
    public CardTeleport(BoardTile _destination, bool _jail = false)
    {
        destination = _destination;
        jail = _jail;
    }
    public override bool performCard(Player player)
    {
        player.inJail = jail;
        return player.teleportPlayer(destination);
    }
}
*/
/// <summary>
/// Moves the player forwards (does award money for passing go)
/// </summary>
[System.Serializable]
public class MoveForward : Card
{
    public int tilePosition;

    public override void performCard(Player player)
    {
        player.UpdatePosition(tilePosition - player.GetCurrentPosition() - 1 % 40); // probably not correct some tweeking required
        player.SetMoving(true);
    }
}

/// <summary>
/// Moves the player backwards (teleports them)
/// </summary>
[System.Serializable]
public class MoveBackward : Card
{
    public int tilePosition;

    public override void performCard(Player player)
    {
        // Sends the player to jail if the card has tile position of 11
        if(tilePosition == 11)
        {
            player.GoToJail();
        }
        else
        {
            player.SetPosition(tilePosition);
            player.SetMoving(true);
        }
    }
}

/// <summary>
/// Moves the player backwards the specified amount (teleports them)
/// </summary>
[System.Serializable]
public class MoveBackwardAmount : Card
{
    public int amount;
    private int newPosition;

    public override void performCard(Player player)
    {
        newPosition = player.GetCurrentPosition() - amount + 1;
        player.SetPosition(newPosition);
        player.SetMoving(true);
    }
}

/// <summary>
/// Gives the current player the amount from each other player
/// </summary>
[System.Serializable]
public class Birthday : Card
{
    public int amount;

    public override void performCard(Player player)
    {
        // Loops for each player in the game 
        foreach (GameObject p in board.GetPlayers())
        {
            // If the player does not equal the current player 
            if (player != p.GetComponent<Player>())
            {
                player.IncreaseMoney(amount);
                p.GetComponent<Player>().DecreaseMoney(amount);
            }
        }
    }
}

/// <summary>
/// Gives the player the choice to take an opportunity knocks card or pay a fine
/// </summary>
[System.Serializable]
public class Choice : Card
{
    public int amount;
    private FreeParking freeParking;

    public override void performCard(Player player)
    {
        freeParking = (FreeParking) board.GetBoardTiles()[20];
        freeParking.AddFineMoney(amount);
        player.DecreaseMoney(amount);
    }
}

/// <summary>
/// Charges the player an amount for the number of houses and hotels they own
/// </summary>
[System.Serializable]
public class Repairs : Card
{
    public int houseCost;
    public int hotelCost;

    private int numHouses = 0;
    private int numHotels = 0;

    /// <summary>
    /// Returns the total cost of the houses and hotels
    /// </summary>
    /// <param name="player">The player who's cost of houses and hotels is being found</param>
    /// <returns>The total cost of the houses and hotels</returns>
    public int GetTotalCost(Player player)
    {
        // Loops through all the properties a player owns
        foreach (BuyableTile buyableTile in player.properties)
        {
            // If of type property
            if (buyableTile.GetType() == typeof(Property))
            {
                // Adds number of hotels and houses to total
                Property property = (Property)buyableTile;
                if (property.GetNumHouses() == 5)
                {
                    numHotels++;
                }
                else
                {
                    numHouses += property.GetNumHouses();
                }
            }
        }
        int total = (hotelCost * numHotels) + (houseCost * numHouses);

        return total;
    }

    public override void performCard(Player player)
    {
        player.DecreaseMoney(GetTotalCost(player));
    }
}

/// <summary>
/// Represents the get out of jail free card
/// </summary>
[System.Serializable]
public class GetOutOfJailFree : Card
{
    public string cardType;

    public override void performCard(Player player)
    {
        /*
        if (cardType == "Opportunity Knocks")
        {
            player.setGetOutOfJailFreeOpportunityKnocks(true);
        }
        else
        {
            player.setGetOutOfJailFreePotluck(true);
        }
        */
    }
}

/*
//Gets every player that is still in the game to give the player money
[System.Serializable]
public class CardBirthday : Card
{ //tested
    public int amount;
    //public Board board;

    public override void performCard(Player player)
    {
        int money = 0;
        for (int i = 0; i < board.GetNumPlayers(); i++)
        {
            Player currentPlayer = board.GetPlayers()[i].GetComponent<Player>();
            if (currentPlayer != player)
            {
                //currentPlayer.stillPlaying = currentPlayer.DecreaseMoney(amount);
                if (currentPlayer.DecreaseMoney(amount))
                {
                    money += amount; //can change this so they still get paid if the person can't afford?
                }
            }
        }
        player.IncreaseMoney(money);
    }
}

//For community chest only
//Gives the player a choice between taking a fine or taking a chance
public class CardChoice : Card
{ //tested
    public int fineAmount;

    public override void performCard(Player player)
    {
        int choice = 1;
        if (choice == 0)
        { 
            //if you want to take the fine
            player.DecreaseMoney(fineAmount);
        }
        else
        { 
            //take a chance instead
            Card card = Bank.chances[0];
            Bank.chances.Remove(Bank.chances[0]);
            card.performCard(player);
            if (card.type != "GetOutOfJail")
            {
                Bank.chances.Add(card);
            }
        }
    }
}

//Gives get out of jail free card to the player
public class CardGetOutOfJail : Card
{ //tested
    string cardType;
    public CardGetOutOfJail(string _cardType)
    {
        cardType = _cardType;
    }
    public override void performCard(Player player)
    {
        if (cardType == "chance")
        {
            player.getOutOfJailChance = true;
        }
        else
        {
            player.getOutOfJailCommunity = true;
        }
        return true;
    }

}
*/
