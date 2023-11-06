using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private int currentWeek;

    private Party playerParty;

    private List<Party> availableParties = new List<Party>();

    private GameObject selectedElectorate;

    public int CurrentWeek
    {
        get { return currentWeek; }
        set { currentWeek = value; }
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
