using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class that represents a political candidate in the game
/// </summary>
public class Candidate
{
    private string candidateName;
    private Party partyAffiliation;

    public string CandidateName
    { 
        get { return candidateName; } 
        set { candidateName = value; } 
    }

    public Party PartyAffiliation 
    { 
        get 
        { return partyAffiliation;} 
        set { partyAffiliation = value; } 
    }

    public Candidate(string name, Party party)
    {
        candidateName = name;
        partyAffiliation = party;
    }
}
