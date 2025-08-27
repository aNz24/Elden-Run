using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Character Name")]
    public string characterName = "";

    [HideInInspector] public AICharacteNetworkManager aiCharacteNetworkManager;
    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
    [HideInInspector] public AICharacterInventoryManager aiCharacterInventoryManager;

    [Header("Navmesh Agent")]
    public NavMeshAgent navMeshAgent;


    [Header("Current State")]
    public AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;
    public AttackState attack;
    public InvestigateSoundState investigateSound;

    protected override void Awake()
    {
        base.Awake();

        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacteNetworkManager = GetComponent<AICharacteNetworkManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();

        animator.keepAnimatorStateOnDisable = true;
    }

    protected override void Update()
    {
        base.Update();

        aiCharacterCombatManager.HandleActionRecovery(this);

        if (navMeshAgent == null)
            return;

        if (IsOwner)
            ProcessStateMachine();

        if (!navMeshAgent.enabled)
            return;

        Vector3 positinDifference = navMeshAgent.transform.position - transform.position;

        if (positinDifference.magnitude > .2f)
            navMeshAgent.transform.localPosition = Vector3.zero;

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(characterUIManager.hasFloatingHPBar)
            characterNetWorkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (characterUIManager.hasFloatingHPBar)
            characterNetWorkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        if (IsOwner)
        {
            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);
            combatStance = Instantiate(combatStance); 
            attack = Instantiate(attack);
            investigateSound = Instantiate(investigateSound);
            currentState = idle;
        }

        aiCharacteNetworkManager.currentHealth.OnValueChanged += aiCharacteNetworkManager.OnHPChanged;

        if(!aiCharacteNetworkManager.isAwake.Value) 
            animator.Play(aiCharacteNetworkManager.sleepAnimation.Value.ToString());

        if (isDead.Value)
            animator.Play("Dead_01");

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        aiCharacteNetworkManager.currentHealth.OnValueChanged -= aiCharacteNetworkManager.OnHPChanged;

    }


    //OPTION 01
    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);

        if (nextState != null)
        {
            currentState = nextState;
        }

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if(aiCharacterCombatManager.currentTarget != null)
        {
            aiCharacterCombatManager.targetDirection =aiCharacterCombatManager.currentTarget.transform.position - transform.position;
            aiCharacterCombatManager.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetDirection);
            aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position); 
        }

        if (navMeshAgent.enabled)
        {
            Vector3  agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if(remainingDistance > navMeshAgent.stoppingDistance)
            {
                aiCharacteNetworkManager.isMoving.Value = true;
            }
            else
            {
                aiCharacteNetworkManager.isMoving.Value = false;
            }
        }
        else
        {
            aiCharacteNetworkManager.isMoving.Value = false;
        }
    }
}
