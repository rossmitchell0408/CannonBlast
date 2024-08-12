using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static StateManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        gameState.Push(GameState.Game);
    }
    /*******************************************/

    //[SerializeField]
    //SystemManager systemManager;

    static public class GameState
    {
        public const int Title = 0;
        public const int Menu = 1;
        public const int PlayerSelect = 2;
        //public const int Instructions = 2;
        //public const int Credits = 3;
        //public const int GameMode = 4;
        public const int Game = 5;
        //public const int GameOver = 6;
        public const int Victory = 7;
    }

    static public Stack gameState = new Stack();


    //void Start()
    //{
    //    gameState.Push(GameState.Title);
    //}

    public void PushState(int state)
    {
        gameState.Push(state);
        SystemManager.Instance.RefreshState();
    }

    public void PopState()
    {
        gameState.Pop();
        SystemManager.Instance.RefreshState();
    }

    public int CheckState()
    {
        return (int)gameState.Peek();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
