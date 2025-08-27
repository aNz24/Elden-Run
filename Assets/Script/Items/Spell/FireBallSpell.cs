using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Spells/Fire Ball")]
public class FireBallSpell : SpellItem  
{
    [SerializeField] float upwardVelocity = 3f;
    [SerializeField] float forwardVelocity = 15f;

    public override void AttempToCastSpell(PlayerManager player)
    {
        base.AttempToCastSpell(player);

        if (!CanICastThisSpell(player))
            return;


        if (player.playerNetWorkManager.isUsingRightHand.Value)
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(mainHandSpellAnimation, true);
        }
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(offHandSpellAnimation, true);

        }
    }

    public override void SuccesfullyCastSpell(PlayerManager player)
    {
        base.SuccesfullyCastSpell(player);

        if(player. IsOwner)
            player.playerCombatManager.DestroyAllCurrentActionFX();

        SpellIntstantitationLocation spellIntstantitationLocation;
        GameObject instantiatedReleaseSpellFX = Instantiate(spellCastReleaseFX);

  

        if (player.playerNetWorkManager.isUsingRightHand.Value)
        {
            spellIntstantitationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellIntstantitationLocation>();

        }
        else
        {
            spellIntstantitationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellIntstantitationLocation>();
        }



        instantiatedReleaseSpellFX.transform.parent = spellIntstantitationLocation.transform;
        instantiatedReleaseSpellFX.transform.localPosition = Vector3.zero;
        instantiatedReleaseSpellFX.transform.localRotation = Quaternion.identity;
        instantiatedReleaseSpellFX.transform.parent = null;

        FireBallManager fireBallManger = instantiatedReleaseSpellFX.GetComponent<FireBallManager>();
        fireBallManger.InitializeFireBall(player);

        /*Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
         Collider chacracterCollisonCollider = player.GetComponent<Collider>();
         
         
        Physics.IgnoreCollision(chacracterCollisonCollider, fireBallManger.damageCollider.damageCollider , true);

        foreach (var collider in characterColliders)
        {
             Physics.IgnoreCollision(collider, fireBallManger.damageCollider.damageCollider , true);
        }*/


        if (player.playerNetWorkManager.isLockedOn.Value)
        {
            instantiatedReleaseSpellFX.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
        }
        else
        {
            Vector3 forwardDirection = player.transform.forward;
            instantiatedReleaseSpellFX.transform.forward = forwardDirection;
        }

        Rigidbody spellRigidbody = instantiatedReleaseSpellFX.GetComponent<Rigidbody>();
        Vector3 upwardVelocityVector = instantiatedReleaseSpellFX.transform.up * upwardVelocity;
        Vector3 forwardVelocityVector = instantiatedReleaseSpellFX.transform.forward * forwardVelocity;
        Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
        spellRigidbody.linearVelocity = totalVelocity;

    }

    public override void InstantiateWarmUpSpellFX(PlayerManager player)
    {
        base.InstantiateWarmUpSpellFX(player);

        SpellIntstantitationLocation spellIntstantitationLocation;
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellCastWarmUpFX);
        if (player.playerNetWorkManager.isUsingRightHand.Value)
        {
            spellIntstantitationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellIntstantitationLocation>();
      
        }
        else
        {
            spellIntstantitationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellIntstantitationLocation>();
        }

        instantiatedWarmUpSpellFX.transform.parent = spellIntstantitationLocation.transform;
        instantiatedWarmUpSpellFX.transform.localPosition = Vector3.zero;
        instantiatedWarmUpSpellFX.transform.localRotation = Quaternion.identity;
        player.playerEffectsManager.activeSpellWarnUpFX = instantiatedWarmUpSpellFX;
    }

}
