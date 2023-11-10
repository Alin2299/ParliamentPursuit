using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class that stores info related to the current game state
/// </summary>
public class GameState
{
    private int weeksRemaining;

    private Party playerParty;

    private List<Party> availableParties = new List<Party>();

    private GameObject selectedElectorate;

    public int WeeksRemaining
    {
        get { return weeksRemaining; }
        set { weeksRemaining = value; }
    }

    public Party PlayerParty
    {
        get { return playerParty; }
        set { playerParty = value; }
    }

    public List<Party> AvailableParties
    {
        get { return availableParties; }
        set { availableParties = value; }
    }

    public GameObject SelectedElectorate
    {
        get { return selectedElectorate; }
        set { selectedElectorate = value; }
    }
}
