using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    enum State
    {
        TITLE = StateManager.GameState.Title,
        MAINMENU = StateManager.GameState.Menu,
        PLAYERSELECT = StateManager.GameState.PlayerSelect,
        GAME = StateManager.GameState.Game
    }

    [System.Serializable]
    class MenuButton
    {
        [SerializeField]
        public Button button;
        [SerializeField]
        public State state;
    }

    [SerializeField]
    List<MenuButton> buttons;

    // Start is called before the first frame update
    void Start()
    {
        foreach(MenuButton button in buttons)
        {
            button.button.onClick.RemoveAllListeners();
            button.button.onClick.AddListener(delegate { StateManager.Instance.PushState((int)button.state); });
        }
        //buttons[0].button.Select();
    }

    private void OnEnable()
    {
        buttons[0].button.Select();
    }
}
