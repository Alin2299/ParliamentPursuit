using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class that represents a political party in the game
/// </summary>
public class Party
{
    private string partyName;
    private Image logo;

    private int funds;
    private int members;

    public Party(string partyName)
    {
        this.partyName = partyName;
        //this.logo = logo;
    }
    public string PartyName
    {
        get { return partyName; }
    }

    public Image Logo
    {
        get { return logo; }
    }
    public int Funds
    {
        get { return funds; }
        set { funds = value; }
    }

    public int Members
    {
        get { return members; }
        set { members = value; }
    }
}
