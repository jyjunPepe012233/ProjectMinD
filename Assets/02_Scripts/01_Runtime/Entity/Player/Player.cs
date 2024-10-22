using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MinD.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

#region RequireComponent
[RequireComponent(typeof(PlayerLocomotionHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerAttributeHandler))]
[RequireComponent(typeof(PlayerInventoryHandler))]
[RequireComponent(typeof(PlayerEquipmentHandler))]
[RequireComponent(typeof(PlayerInteractionHandler))]
#endregion

public class Player : BaseEntity {

    [FormerlySerializedAs("curHp")] [Header("[ Attributes ]")]
    public int curHP;
    public int curMp;
    public int curStamina;
    
    [Header("Flags")]
    public bool isPerformingAction;
    public bool isGrounded;
    public bool isMoving;
    public bool isJumping;
    public bool isLockOn;
    public bool canRotate;
    public bool canMove;
    
    [HideInInspector] public PlayerCamera camera;
    
    [HideInInspector] public PlayerLocomotionHandler locomotion;
    [HideInInspector] public PlayerAnimationHandler animation;
    [HideInInspector] public PlayerAttributeHandler attribute;
    [HideInInspector] public PlayerInventoryHandler inventory;
    [HideInInspector] public PlayerEquipmentHandler equipment;
    [HideInInspector] public PlayerInteractionHandler interaction;
    [HideInInspector] public PlayerCombatHandler combat;
    



    protected override void Awake() {

        base.Awake();

        camera = FindObjectOfType<PlayerCamera>();
        locomotion = GetComponent<PlayerLocomotionHandler>();
        animation = GetComponent<PlayerAnimationHandler>();
        attribute = GetComponent<PlayerAttributeHandler>();
        inventory = GetComponent<PlayerInventoryHandler>();
        equipment = GetComponent<PlayerEquipmentHandler>();
        interaction = GetComponent<PlayerInteractionHandler>();
        combat = GetComponent<PlayerCombatHandler>();


        camera.owner = this;
        locomotion.owner = this;
        animation.owner = this;
        attribute.owner = this;
        inventory.owner = this;
        equipment.owner = this;
        interaction.owner = this;
        combat.owner = this;

    }

    void OnEnable() {
        
        inventory.LoadItemData();
        
        PlayerHUDManager.Instance.RefreshAllStatusBar();
        
    }

    void Update() {
        
        camera.HandleCamera();
        locomotion.HandleAllLocomotion();
        inventory.HandleQuickSlotSwapping();
        interaction.HandleInteraction();
        combat.HandleAllCombatAction();

    }
}