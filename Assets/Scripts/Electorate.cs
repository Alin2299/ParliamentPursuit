using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class that represents an electorate in the game
/// </summary>
public class Electorate : MonoBehaviour
{
    private string electorateName;
    private GameObject originalElectorate;

    private List<Candidate> candidates= new List<Candidate>();

    public List<Candidate> Candidates
    {
        get { return candidates; }
        set { candidates = value; }
    }

    public GameObject OriginalElectorate
    {
        get { return originalElectorate; }
        set { originalElectorate = value; }
    }

    void OnMouseEnter()
    {
        MapManager.Instance.HighlightElectorate(originalElectorate, this.gameObject);
    }

    void OnMouseExit()
    {
        MapManager.Instance.UnhighlightElectorate(originalElectorate, this.gameObject);
    }
}
