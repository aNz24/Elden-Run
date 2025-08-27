using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.TextCore.Text;
using PsychoticLab;

public class CharacterManager : NetworkBehaviour
{


    [Header("Status")]
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);


    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterNetWorkManager characterNetWorkManager;
    [HideInInspector] public CharacterEffectManager characterEffectManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterSFXManager characterSFXManager;
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
    [HideInInspector] public CharacterUIManager characterUIManager;
    [HideInInspector] public CharacterStatManager characterStatManager;


    [Header("Mini Map")]
    public GameObject minimapIcon;

    [Header("Character Group")]
    public CharacterGroup characterGroup;


    [Header("Flags")] 
    public bool isPerformingAction = false;


    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetWorkManager = GetComponent<CharacterNetWorkManager>();
        characterEffectManager = GetComponent<CharacterEffectManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();   
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSFXManager = GetComponent<CharacterSFXManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        characterUIManager = GetComponent<CharacterUIManager>();
        characterStatManager = GetComponent<CharacterStatManager>();

    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded", characterLocomotionManager.isGrounded);
        if (IsOwner)
        {
            characterNetWorkManager.networkPosition.Value = transform.position;
            characterNetWorkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            //Position
            transform.position =  Vector3.SmoothDamp
                (transform.position, characterNetWorkManager.networkPosition.Value,
                ref characterNetWorkManager.networkPositionVelocity,
                characterNetWorkManager.networkPositionSmoothTime);

            //Rotation
            transform.rotation = Quaternion.Slerp
               (transform.rotation,
               characterNetWorkManager.networkRotation.Value,
               characterNetWorkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void Start()
    {
        IgnoreMyOwnColliders();
    }

    protected virtual void LateUpdate()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        animator.SetBool("isMoving", characterNetWorkManager.isMoving.Value);
        characterNetWorkManager.OnIsActiveChanged(false,characterNetWorkManager.isMoving.Value);

        isDead.OnValueChanged += characterNetWorkManager.OnIsDeadChanged;
        characterNetWorkManager.isMoving.OnValueChanged += characterNetWorkManager.OnIsMovingChanged;
        characterNetWorkManager.isActive.OnValueChanged += characterNetWorkManager.OnIsActiveChanged;
    }

    public override void OnNetworkDespawn()
    { 
        base.OnNetworkDespawn();
        isDead.OnValueChanged -= characterNetWorkManager.OnIsDeadChanged;
        characterNetWorkManager.isMoving.OnValueChanged -= characterNetWorkManager.OnIsMovingChanged;
        characterNetWorkManager.isActive.OnValueChanged -= characterNetWorkManager.OnIsActiveChanged;

    }

    public virtual IEnumerator ProcessDeathEvent(bool manaullySelectDeathAnimation  = false)
    {
        if(IsOwner)
        {
            characterNetWorkManager.currentHealth.Value= 0;
            isDead.Value = true;

            minimapIcon.SetActive(false);

            if (!manaullySelectDeathAnimation && !characterNetWorkManager.isBeingCriticallyDamaged.Value)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }
        }
 
        yield return new WaitForSeconds(5f);
    }

    public virtual void ReviveCharacter()
    {

    }

    protected virtual void IgnoreMyOwnColliders()
    {   
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

        List<Collider> ignoreColliders = new List<Collider>();

        foreach (var collider in damageableCharacterColliders)
        {
            ignoreColliders.Add(collider);   
        }

        ignoreColliders.Add(characterControllerCollider);

        foreach (var collider in ignoreColliders)
        {
            foreach (var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }

}
