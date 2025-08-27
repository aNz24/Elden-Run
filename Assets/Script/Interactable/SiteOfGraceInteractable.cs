using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SiteOfGraceInteractable : Interactable
{
    [Header("Site Of Grace Info")]
    public int siteOfGraceID;

    [Header("Activated")]
    public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("VFX")]
    [SerializeField] GameObject activatedParticles;

    [Header("Interaction Text")]
    [SerializeField] string unactivatedInteractionText = "Restore Site Of Grace";
    [SerializeField] string activatedInteractionText = "Rest";

    [Header("Teleport Tranform")]
    [SerializeField] Transform teleportTranform;

    protected override void Start()
    {
        base.Start();

        if (IsOwner)
        {
            if (WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.ContainsKey(siteOfGraceID))
            {
                isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.siteOfGrace[siteOfGraceID];
            }
            else
            {
                isActivated.Value = false;
            }

            if(isActivated.Value)
            {
                interactableText =activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;

            }
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
            OnIsActivatedChanged(false, isActivated.Value);

        isActivated.OnValueChanged += OnIsActivatedChanged;

        WorldObjectManager.instance.AddSiteOfGraceToList(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isActivated.OnValueChanged -= OnIsActivatedChanged;

    }

    private void RestoreSiteOfGrace(PlayerManager player)
    {
        isActivated.Value = true;

        if(WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.ContainsKey(siteOfGraceID))
            WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.Remove(siteOfGraceID);

        WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.Add(siteOfGraceID,true);

        player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);


        PlayerUIManager.instance.playerUIPopUpManager.SendGraceRestoredPopUp("SITE OF GRACE RESTORED");

        StartCoroutine(WaitForAnimationAndPopUpThenRestoredCollider());

    }

    private void RestAtSiteOfGrace(PlayerManager player)
    {
        PlayerUIManager.instance.playerUISiteOfGarceManager.OpenMenu();

        interactableCollider.enabled = true;
        player.playerNetWorkManager.currentHealth.Value = player.playerNetWorkManager.maxHealth.Value;
        player.playerNetWorkManager.currentStamina.Value = player.playerNetWorkManager.maxStamina.Value;
        player.playerNetWorkManager.currentFocusPoints.Value = player.playerNetWorkManager.maxFocusPoints.Value;
        WorldAIManager.instance.ResetAllCharacters();

    }

    private IEnumerator WaitForAnimationAndPopUpThenRestoredCollider()
    {
        yield return new WaitForSeconds(3);

        interactableCollider.enabled = true;
    }

    private void OnIsActivatedChanged(bool oldStatus , bool newStatus)
    {
        if(isActivated.Value)
        {
            activatedParticles.SetActive(true);
            interactableText = activatedInteractionText;

        }
        else
        {
            interactableText = unactivatedInteractionText;

        }
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        if (player.isPerformingAction)
            return;

        if (player.playerCombatManager.isUsingItem)
            return;

        WorldSaveGameManager.instance.currentCharacterData.lastSiteOfGraceRestedAt =siteOfGraceID;

        if (!isActivated.Value)
        {
            RestoreSiteOfGrace(player);
        }
        else
        {
            RestAtSiteOfGrace(player);
        }
    }

    public void TeleportToSiteOfGrace()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        // ENABLE LOADING SCREEN
        PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

        // TELEPORT PLAYER
        player.transform.position = teleportTranform.position;


        PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreene(1);
    }
}
