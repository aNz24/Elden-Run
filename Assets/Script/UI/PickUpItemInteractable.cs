using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickUpItemInteractable : Interactable
{
    public ItemPickUpType pickUpType;


    [Header("Item")]
    [SerializeField] Item item;

    [Header("Creature Loot Pick Up")]
    public NetworkVariable<int> itemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<ulong> droppingCreatureID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public bool trackDroppingCreaturesPosition = true;

    [Header("World Spawn Pick Up")]
    [SerializeField] int worldSpawnInteractableId;
    [SerializeField] bool hasBeenLooted = false;

    [Header("Drop SFX")]
    [SerializeField] AudioClip itemDropSFX;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();

       if(pickUpType == ItemPickUpType.WorldSpawn)
            CheckIfWorldItemWasAlreadyLooted();

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        itemID.OnValueChanged += OnItemIDChanged;
        networkPosition.OnValueChanged += OnNetworkPositionChanged;
        droppingCreatureID.OnValueChanged += OnDroppingCreatureIDChanged;

        if (pickUpType == ItemPickUpType.CharacterDrop)
            audioSource.PlayOneShot(itemDropSFX);

        if (!IsOwner)
        {
            OnItemIDChanged(0, itemID.Value);
            OnNetworkPositionChanged(Vector3.zero, networkPosition.Value);
            OnDroppingCreatureIDChanged(0, droppingCreatureID.Value);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        itemID.OnValueChanged -= OnItemIDChanged;
        networkPosition.OnValueChanged -= OnNetworkPositionChanged;
        droppingCreatureID.OnValueChanged -= OnDroppingCreatureIDChanged;

    }

    private void CheckIfWorldItemWasAlreadyLooted()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            gameObject.SetActive(false);
            return;
        }

        if (!WorldSaveGameManager.instance.currentCharacterData.worldItemLooted.ContainsKey(worldSpawnInteractableId))
        {
            WorldSaveGameManager.instance.currentCharacterData.worldItemLooted.Add(worldSpawnInteractableId, false);
        }

        hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemLooted[worldSpawnInteractableId];

        if (hasBeenLooted)
            gameObject.SetActive(false);
    }

    public override void Interact(PlayerManager player)
    {
        if (player.isPerformingAction)
            return;

        if (player.playerCombatManager.isUsingItem)
            return;

        base.Interact(player);

        // 1.SFX
        player.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.pickUpItemSFX);

        //ANIMATION
        player.playerAnimatorManager.PlayTargetActionAnimation("Pick_Up_Item_01", true);

        // 2.ADD ITEM TO INVENTORY
        player.playerInventoryManager.AddItemToInventory(item);
        // 3.DISPLAY A UI POP UP SHOWING ITEM'S NAME AND PICURE
        PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item,1);

        // 4. SAVE LOOT STATUS IF IT'S A WORLD SPAWN
        if (pickUpType == ItemPickUpType.WorldSpawn)
        {
            if (WorldSaveGameManager.instance.currentCharacterData.worldItemLooted.ContainsKey((int)worldSpawnInteractableId))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemLooted.Remove(worldSpawnInteractableId);
            }

            WorldSaveGameManager.instance.currentCharacterData.worldItemLooted.Add(worldSpawnInteractableId, true);
        }

        // 5. HIDE OR DESTROY GAMEOBJECT
        DestroyThisNetworkObjectServerRpc();

    }

    protected void OnItemIDChanged(int oldValue , int newValue)
    {
        if (pickUpType != ItemPickUpType.CharacterDrop)
            return;

        item = WorldItemDatabase.instance.GetItemByID(itemID.Value);

    }

    protected void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        if (pickUpType != ItemPickUpType.CharacterDrop)
            return;

        transform.position = networkPosition.Value;
    }

    protected void OnDroppingCreatureIDChanged(ulong oldID, ulong newID)
    {
        if (pickUpType != ItemPickUpType.CharacterDrop)
            return;

        if (trackDroppingCreaturesPosition)
            StartCoroutine(TrackDroppingCreaturesPosition());
    }

    protected IEnumerator TrackDroppingCreaturesPosition()
    {
        AICharacterManager droppingCreature = NetworkManager.Singleton.SpawnManager.SpawnedObjects[droppingCreatureID.Value].gameObject.GetComponent<AICharacterManager>();
        bool trackCreature = false;


        if(droppingCreature != null)
            trackCreature = true;

        if (trackCreature)
        {
            while (gameObject.activeInHierarchy)
            {
                transform.position = droppingCreature.characterCombatManager.lockOnTranfrom.position;
                yield return null;
            }
        }
        yield return null;

    }

    [ServerRpc(RequireOwnership = false)]
    protected void DestroyThisNetworkObjectServerRpc()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        if (IsServer)
        {
            GetComponent<NetworkObject>().Despawn();
        }

        player.playerCombatManager.isUsingItem = false;
    }

}
