using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class WinUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text victoryText;

    public void ShowWinDialog(string player, string method)
    {
        victoryText.text = player + " wins by " + method + "!";
        victoryText.color = (player == "Red") ? new Color(0.7f, 0, 0) : new Color(0, 0, 0);
        panel.SetActive(true);
    }

    public void returnToTitle()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("TitleScene");
    }
}
