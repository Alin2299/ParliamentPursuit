using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using OpenAI.Models;
using OpenAI.Chat;

public class ElectorateController : MonoBehaviour
{


    void Start()
    {

    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.white;
        Debug.Log(name);
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.gray;
    }
}
