using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    List<PlayerController> players = new List<PlayerController>();
    int scoreToWin = 3;

    private void Update()
    {
        CheckPlayerScores();
    }

    public void NewGame()
    {
        FindPlayers();
        ActivateAllPlayers(true);
        RespawnAllPlayers();
        ItemSpawnManager.Instance.BeginGame();
        FlagManager.Instance.BeginGame();
    }

    public void EndGame()
    {
        FindPlayers();
        ResetScores();
        ActivateAllPlayers(false);
        StateManager.Instance.PushState(StateManager.GameState.Victory);
        FlagManager.Instance.EndGame();
    }

    public void AddPlayer(PlayerController player)
    {
        if (players.Contains(player))
        {
            return;
        }
        players.Add(player);
    }

    void FindPlayers()
    {
        PlayerController[] plyrs = FindObjectsOfType<PlayerController>();
        
        foreach(PlayerController player in plyrs)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }
        }
    }

    void ActivateAllPlayers(bool active)
    {
        foreach(PlayerController player in players)
        {
            player.gameObject.SetActive(active);
        }
    }

    void RespawnAllPlayers()
    {
        foreach (PlayerController player in players)
        {
            SpawnManager.Instance.RespawnPlayer(player);
        }
    }

    void CheckPlayerScores()
    {
        foreach (PlayerController player in players)
        {
            if (player.score >= scoreToWin)
            {
                StateManager.Instance.PushState(StateManager.GameState.Victory);
                SetWinningPlayer(player);
                EndGame();
                return;
            }
        }
    }

    void ResetScores()
    {
        foreach (PlayerController player in players)
        {
            player.score = 0;
        }
    }

    void SetWinningPlayer(PlayerController player)
    {
        GameObject victoryState = GameObject.Find("VictoryPanel");
        TextMeshProUGUI victoryText = victoryState.transform.Find("Canvas/VictoryText").GetComponent<TextMeshProUGUI>();
        victoryText.text = "Player " + player.index + " Wins!";
    }
}
