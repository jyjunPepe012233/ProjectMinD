using System;
using UnityEngine;
using UnityEngine.AI;
using MinD.Runtime.Managers;
using MinD.SO.EnemySO;
using MinD.SO.EnemySO.State;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyCombatHandler))]
[RequireComponent(typeof(EnemyAnimationHandler))]
[RequireComponent(typeof(EnemyUtilityHandler))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : BaseEntity {
	
	[HideInInspector] public NavMeshAgent navAgent;
	
	[HideInInspector] public EnemyStateMachine stateMachine;
	[HideInInspector] public EnemyCombatHandler combat;
	[HideInInspector] public EnemyAnimationHandler animation;
	[HideInInspector] public EnemyUtilityHandler utility;
	
	[HideInInspector] public Vector3 worldPlacedPosition;
	[HideInInspector] public Quaternion worldPlacedRotation;
	
	
	[Space(15)]
	public EnemyState currentState;
	public EnemyState previousState;
	[Space(3)]
	public EnemyState globalState;
	
	[HideInInspector] public EnemyState[] states;
	[HideInInspector] public EnemyState[] globalStates;
	

	[Header("[ Attributes ]")]
	[SerializeField] private int curHp;
	public int CurHp {
		get => curHp;
		set => curHp = Mathf.Clamp(value, 0, attribute.maxHp);
	}
	public EnemyAttribute attribute;
	
	
	
	[Header("[ Flags ]")]
	public bool isPerformingAction;
	public bool isInCombat;
	
	public Action getHitAction = new Action(() => {});
	
	

	// SETUP A OBJECT SETTINGS
	protected override void Awake() {

		base.Awake();

		navAgent = GetComponent<NavMeshAgent>();

		stateMachine = GetComponent<EnemyStateMachine>();
		combat = GetComponent<EnemyCombatHandler>();
		animation = GetComponent<EnemyAnimationHandler>();
		utility = GetComponent<EnemyUtilityHandler>();
		
		stateMachine.owner = this;
		combat.owner = this;
		animation.owner = this;
		utility.owner = this;
		

		WorldEnemyManager.Instance.RegisteringEnemyOnWorld(this);
		worldPlacedPosition = transform.position;
		worldPlacedRotation = transform.rotation;
		utility.AllCollisionIgnoreSetup();
		
		
		SetupStatesArray();
		Reload();
	} 
	
	// CALL SETUP
	protected override void Update() {
		
		base.Update();

		stateMachine.ExecuteStateTick();

	}
	
	
	
	
	// ASSIGN STATE ARRAY AND 
	protected abstract void SetupStatesArray();
	
	
	// SETUP START STATE AND RUNTIME ATTRIBUTE SETTING
	public virtual void Reload() {

		curHp = attribute.maxHp;
		transform.position = worldPlacedPosition;
		transform.position = worldPlacedPosition;
		
		// NEED TO SET THE START STATE IN OVERRIDE
	}
}

}