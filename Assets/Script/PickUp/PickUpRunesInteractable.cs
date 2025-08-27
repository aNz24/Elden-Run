using UnityEngine;

public class PickUpRunesInteractable : Interactable
{
    public int runesCount = 0;  

    public override void Interact(PlayerManager player)
    {
        WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = false;
        player.playerStatManager.AddRunes(runesCount);

        Destroy(gameObject);
    }
}
