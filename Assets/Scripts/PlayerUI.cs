using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI damageText;

    PlayerController player;

    int index;

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerInfo();
    }

    public void SetPlayer(PlayerController player, int index)
    {
        this.player = player;
        //nameText.text = player.gameObject.name;
        nameText.text = "Player " + (index + 1);
        gameObject.SetActive(true);

        player.index = index + 1;
    }   
    
    public PlayerController GetPlayer()
    {
        return player;
    }

    void UpdatePlayerInfo()
    {
        if (player == null)
        {
            return;
        }

        damageText.text = "DMG: " + player.damage.ToString();
        scoreText.text = "Score: " + player.score.ToString();
    }
}
