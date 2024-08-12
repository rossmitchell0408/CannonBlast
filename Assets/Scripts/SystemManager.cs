using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static SystemManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    [SerializeField]
    GameObject titlePanel;
    [SerializeField]
    GameObject menuPanel;
    [SerializeField]
    GameObject playerSelectPanel;
    //[SerializeField]
    //GameObject instructionsPanel;
    //[SerializeField]
    //GameObject creditsPanel;
    //[SerializeField]
    //GameObject gameModePanel;
    [SerializeField]
    GameObject gamePanel;
    //[SerializeField]
    //GameObject gameOverPanel;
    [SerializeField]
    GameObject victoryPanel;

    //[SerializeField]
    //GameObject gridLevel1;
    //[SerializeField]
    //GameObject gridLevel2;
    //[SerializeField]
    //GameObject gridLevel3;

    //[SerializeField]
    //GameManager gameManager;

    //[SerializeField]
    //AudioManager audioManger;
    [SerializeField]
    GameObject playerManager;

    void Start()
    {
        RefreshState();
    }

    public void RefreshState()
    {
        titlePanel.SetActive(false);
        menuPanel.SetActive(false);
        playerSelectPanel.SetActive(false);
        //instructionsPanel.SetActive(false);
        //creditsPanel.SetActive(false);
        //gameModePanel.SetActive(false);
        gamePanel.SetActive(false);
        //gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        //gameManager.ClearLevel();
        //playerManager.SetActive(false);
        ItemSpawnManager.Instance.EndGame();

        switch (StateManager.gameState.Peek())
        {
            case StateManager.GameState.Title:
                titlePanel.SetActive(true);
                //audioManger.Play("MenuTheme");
                break;
            case StateManager.GameState.Menu:
                menuPanel.SetActive(true);
                //audioManger.Play("MenuTheme");
                break;
            case StateManager.GameState.PlayerSelect:
                playerSelectPanel.SetActive(true);
                break;
            //case StateManager.GameState.Instructions:
            //    instructionsPanel.SetActive(true);
            //    break;
            //case StateManager.GameState.Credits:
            //    creditsPanel.SetActive(true);
            //    break;
            //case StateManager.GameState.GameMode:
            //    gameModePanel.SetActive(true);
            //    gameModePanel.GetComponent<GameModeStateManager>().EnterState();
            //    break;
            case StateManager.GameState.Game:
                gamePanel.SetActive(true);
                GameManager.Instance.NewGame();
                //ItemSpawnManager.Instance.BeginGame();
                //playerManager.SetActive(true);
                //LoadLevel();
                //gameManager.BeginLevel();
                //audioManger.Play("GameTheme");
                break;
            //case StateManager.GameState.GameOver:
            //    gameOverPanel.SetActive(true);
            //    audioManger.Play("MenuTheme");
            //    break;
            case StateManager.GameState.Victory:
                victoryPanel.SetActive(true);
                //audioManger.Play("MenuTheme");
                break;
            default:
                break;
        }
    }

    private void LoadLevel()
    {
        //gridLevel1.SetActive(false);
        //gridLevel2.SetActive(false);
        //gridLevel3.SetActive(false);
        //switch (gameManager.GetLevel())
        //{
        //    case 1:
        //        gridLevel1.SetActive(true);
        //        break;
        //    case 2:
        //        gridLevel2.SetActive(true);
        //        break;
        //    case 3:
        //        gridLevel3.SetActive(true);
        //        break;
        //    default:
        //        gridLevel1.SetActive(true);
        //        break;
        //}
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}