using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIToggelHud : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerUIManager.instance.playerUIHudManager.ToggleHud(false);
    }

    private void OnDisable()
    {
        PlayerUIManager.instance.playerUIHudManager.ToggleHud(true);
    }
}
