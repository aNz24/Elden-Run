using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUICharacterMenuManager : PlayerUIMenu
{
    [System.Obsolete]
    public void QuitToMainMenu()
    {
        var bossHPBar = GameObject.FindObjectOfType<UI_Boss_HP_Bar>();
        if (bossHPBar != null)
        {
            Destroy(bossHPBar.gameObject);
        }

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }


        SceneManager.LoadScene("Scene_Main_Menu_01");
        CloseMenuAfterFixedFrame();
        WorldSFXManger.instance.StopBossMusic();

    }

    public void SaveGame()
    {
        WorldSaveGameManager.instance.SaveGame();
        CloseMenuAfterFixedFrame();
    }

    public void QuitToDesktop()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
