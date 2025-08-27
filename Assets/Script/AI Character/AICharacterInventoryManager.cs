using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AICharacterInventoryManager : CharacterInventoryManager
{
    AICharacterManager aICharacter;
    [Header("Loot Chance")]
    public int dropItemChance = 10;
    [SerializeField] Item[] droppableItems;

    protected override void Awake()
    {
        base.Awake();

        aICharacter = GetComponent<AICharacterManager>();
    }

    public void DropItem()
    {
        if(!aICharacter.IsOwner)
            return;

        bool willDropItem = false;
        int itemChanceRoll = Random.Range(0, 100);

        if (itemChanceRoll <= dropItemChance)
            willDropItem = true; 

        if (!willDropItem )
            return;

        Item generatedItem = droppableItems[Random.Range(0, droppableItems.Length)];

        GameObject itemPickUpInteractableObject = Instantiate(WorldItemDatabase.instance.pickUpItemPrefab);
        PickUpItemInteractable pickUpInteractable = itemPickUpInteractableObject.GetComponent<PickUpItemInteractable>();
        itemPickUpInteractableObject.GetComponent<NetworkObject>().Spawn();
        pickUpInteractable.itemID.Value = generatedItem.itemID;
        pickUpInteractable.networkPosition.Value= transform.position;
        pickUpInteractable.droppingCreatureID.Value = aICharacter.NetworkObjectId;

    }
}
