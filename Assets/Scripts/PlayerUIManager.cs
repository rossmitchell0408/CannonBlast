using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    List<PlayerUI> playerUI = new List<PlayerUI>();

    public void SetupUI(PlayerController player)
    {
        for(int i = 0; i < playerUI.Count; i++)
        {
            if (playerUI[i].GetPlayer() == null)
            {
                playerUI[i].SetPlayer(player, i);
                break;
            }
        }
    }
}
