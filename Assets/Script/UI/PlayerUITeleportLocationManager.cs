using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUITeleportLocationManager : PlayerUIMenu
{
    [SerializeField] GameObject[] teleportLocations;

    public override void OpenMenu()
    {
        base.OpenMenu();

        CheckForUnLockedTeleprots();
    }

    private void CheckForUnLockedTeleprots()
    {
        bool hasSelectedButton = false;

        for (int i = 0; i < teleportLocations.Length; i++)
        {
            for (int j = 0; j < WorldObjectManager.instance.siteOfGrace.Count; j++)
            {
                if (WorldObjectManager.instance.siteOfGrace[j].siteOfGraceID == i)
                {
                    if (WorldObjectManager.instance.siteOfGrace[j].isActivated.Value)
                    {
                        teleportLocations[i].SetActive(true);

                        if (!hasSelectedButton)
                        {
                            hasSelectedButton = true;
                            teleportLocations[i].GetComponent<Button>().Select(); 
                            teleportLocations[i].GetComponent<Button>().OnSelect(null); 
                        }
                    }
                    else
                    {
                        teleportLocations[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void TeleportToSiteOfGrace(int siteID)
    {
        CloseMenu();

        for (int i = 0; i < WorldObjectManager.instance.siteOfGrace.Count; i++)
        {
            if (WorldObjectManager.instance.siteOfGrace[i].siteOfGraceID == siteID)
            {
                //TELEPORT
                WorldObjectManager.instance.siteOfGrace[i].TeleportToSiteOfGrace();
                return;
            }
        }
    }
}
