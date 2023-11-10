using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public singleton class that manages the overall game
/// </summary>
public sealed class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameState gameState;

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        gameState = new GameState();

        List<string> availablePartyNames = new List<string>()
        {
            "Labour", "National", "Greens", "ACT", "NZ First", "The Māori Party", "The Opportunities Party"
        };

        foreach (string partyName in availablePartyNames)
        {
            Party partyToAdd = new Party(partyName);
            gameState.AvailableParties.Add(partyToAdd);

            // USED FOR TESTING
            if (partyName.Equals("The Opportunities Party"))
            {
                gameState.PlayerParty = partyToAdd;
            }
        }

        gameState.WeeksRemaining = 5;

    }


    public static GameManager Instance
    {
        get { return instance; }
    }


    public GameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }
}
