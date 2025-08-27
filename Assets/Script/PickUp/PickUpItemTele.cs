using UnityEngine;

public class PickUpItemTele : Interactable
{

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

       PlayerUIManager.instance.playerUITeleportLocationManager.TeleportToSiteOfGrace(1);

        WorldSaveGameManager.instance.SaveGame();

        Destroy(gameObject);
    }
}
