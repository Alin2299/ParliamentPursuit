using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        if (name.Contains("(Clone)"))
        {
            name = name.Replace("(Clone)", "");
            originalElectorate = GameObject.Find(name);
        }
        else
        {
            originalElectorate = GameObject.Find(name + "(Clone)");
        }

    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.white;

        if (originalElectorate != null)
        {
            originalElectorate.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.gray;

        if (originalElectorate != null)
        { 
            originalElectorate.GetComponent<Renderer>().material.color = Color.gray;
        }
    }
}
