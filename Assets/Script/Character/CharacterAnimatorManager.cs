using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animation")]
    public string lastDamageAnimationPlayed;

    [Header("Flags")]
    public bool applyRootMotion = false;
    //Ping
    [SerializeField] string hit_Forward_Ping_01 = "hit_Forward_Ping_01";
    [SerializeField] string hit_Forward_Ping_02 = "hit_Forward_Ping_02";

    [SerializeField] string hit_Backward_Ping_01 = "hit_Backward_Ping_01";
    [SerializeField] string hit_Backward_Ping_02 = "hit_Backward_Ping_02";

    [SerializeField] string hit_Left_Ping_01 = "hit_Left_Ping_01";
    [SerializeField] string hit_Left_Ping_02 = "hit_Left_Ping_02";

    [SerializeField] string hit_Right_Ping_01 = "hit_Right_Ping_01";
    [SerializeField] string hit_Right_Ping_02 = "hit_Right_Ping_02";

    public List<string> forward_Ping_Damage = new List<string>();
    public List<string> backward_Ping_Damage = new List<string>();
    public List<string> left_Ping_Damage = new List<string>();
    public List<string> right_Ping_Damage = new List<string>();

    // 
    [SerializeField] string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
    [SerializeField] string hit_Forward_Medium_02 = "hit_Forward_Medium_02";

    [SerializeField] string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
    [SerializeField] string hit_Backward_Medium_02 = "hit_Backward_Medium_02";

    [SerializeField] string hit_Left_Medium_01 = "hit_Left_Medium_01";
    [SerializeField] string hit_Left_Medium_02 = "hit_Left_Medium_02";

    [SerializeField] string hit_Right_Medium_01 = "hit_Right_Medium_01";
    [SerializeField] string hit_Right_Medium_02 = "hit_Right_Medium_02";

    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> backward_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start()
    {
        //Ping
        forward_Ping_Damage.Add(hit_Forward_Ping_01);
        forward_Ping_Damage.Add(hit_Forward_Ping_02);

        backward_Ping_Damage.Add(hit_Backward_Ping_01);
        backward_Ping_Damage.Add(hit_Backward_Ping_02);

        left_Ping_Damage.Add(hit_Left_Ping_01);
        left_Ping_Damage.Add(hit_Left_Ping_02);

        right_Ping_Damage.Add(hit_Right_Ping_01);
        right_Ping_Damage.Add(hit_Right_Ping_02);

        //
        forward_Medium_Damage.Add(hit_Forward_Medium_01);
        forward_Medium_Damage.Add(hit_Forward_Medium_02);

        backward_Medium_Damage.Add(hit_Backward_Medium_01);
        backward_Medium_Damage.Add(hit_Backward_Medium_02);

        left_Medium_Damage.Add(hit_Left_Medium_01);
        left_Medium_Damage.Add(hit_Left_Medium_02);

        right_Medium_Damage.Add(hit_Right_Medium_01);
        right_Medium_Damage.Add(hit_Right_Medium_02);
    }

    public string GetRandomAnimationFromList(List<string> animationList)
    {
        List<string> finalList = new List<string>();

        foreach (var item in animationList)
        {
            finalList.Add(item);
        }

        finalList.Remove(lastDamageAnimationPlayed);

        for (int i = finalList.Count - 1; i > -1; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement , bool isSprinting ) 
    {

        float snappedHorizontal = horizontalMovement;
        float snappedVertical = verticalMovement;  


        if(horizontalMovement > 0 && horizontalMovement <=0.5f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
        {
            snappedHorizontal = 1;

        }
        else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
        {
            snappedHorizontal =-0.5f;

        }
        else if (horizontalMovement < -0.5f && horizontalMovement >= -1)
        {
            snappedHorizontal = -1;

        }
        else
        {
            snappedHorizontal = 0;

        }



        if (verticalMovement > 0 && verticalMovement <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.5f && verticalMovement <= 1)
        {
            snappedVertical = 1;

        }
        else if (verticalMovement < 0 && verticalMovement >= -0.5f)
        {
            snappedVertical = -0.5f;

        }
        else if (verticalMovement < -0.5f && verticalMovement >= -1)
        {
            snappedVertical = -1;

        }
        else
        {
            snappedVertical = 0;

        }




        if ( isSprinting )
        {
            snappedVertical = 2;
        }

        character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targerAnimation,
        bool isPerformingAction ,
        bool aplyRootMotion = true,
        bool canRotate = false, 
        bool canMove = false,
        bool canRun= true,
        bool canRoll = false)
    {
        this.applyRootMotion = aplyRootMotion;
        character.animator.CrossFade(targerAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove;
        character.characterLocomotionManager.canRun = canRun;
        character.characterLocomotionManager.canRoll = canRoll;


        character.characterNetWorkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targerAnimation, aplyRootMotion);
    }

    public virtual void PlayTargetActionAnimationInstantly(string targerAnimation,
        bool isPerformingAction,
        bool aplyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        bool canRun = true,
        bool canRoll = false)
    {
        this.applyRootMotion = aplyRootMotion;
        character.animator.Play(targerAnimation);
        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove; 
        character.characterLocomotionManager.canRun = canRun;
        character.characterLocomotionManager.canRoll = canRoll;



        character.characterNetWorkManager.NotifyTheServerOfInstantActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targerAnimation, aplyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation( WeaponItem weapon,
        AttackType attackType,
        string targerAnimation, 
        bool isPerformingAction, 
        bool aplyRootMotion = true, 
        bool canRotate = false, 
        bool canMove = false,
        bool canRoll = false
        )
    {

        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targerAnimation;
        UpdateAnimatorController(weapon.weaponAnimator);
        character.characterAnimatorManager.applyRootMotion = aplyRootMotion;
        character.animator.CrossFade(targerAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove;
        character.characterNetWorkManager.isAttacking.Value = true;
        character.characterLocomotionManager.canRoll = canRoll;
        

        character.characterNetWorkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targerAnimation, aplyRootMotion);
    }

    public void UpdateAnimatorController(AnimatorOverrideController weaponController)
    {
        character.animator.runtimeAnimatorController = weaponController;
    }
}
