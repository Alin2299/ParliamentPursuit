using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candidate : MonoBehaviour
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
}
