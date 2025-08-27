using UnityEngine;

public class PickUpLootRunes : Interactable
{
    [SerializeField] int minValue;
    [SerializeField] int maxValue;
    [SerializeField] GameObject VFX;


    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        int runesCount = Random.Range(minValue, maxValue);

        player.playerStatManager.AddRunes(runesCount);

        GameObject bloodSplatter = Instantiate(VFX, gameObject.transform.position, Quaternion.identity);

        // PlayerUIManager.instance.playerUITeleportLocationManager.TeleportToSiteOfGrace(1);

        player.playerNetWorkManager.remainingHealthFlasks.Value += 2;
        player.playerNetWorkManager.remainingFocusPointFlasks.Value += 2;

        WorldSaveGameManager.instance.SaveGame();

        Destroy(gameObject);
    }
}
