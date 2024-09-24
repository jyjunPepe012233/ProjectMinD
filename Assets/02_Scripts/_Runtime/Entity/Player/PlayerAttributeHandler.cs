using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Combat;
using UnityEngine;

public class PlayerAttributeHandler : MonoBehaviour {

    private Player owner;

    [Header("[ Stats ]")]
    public int vitality;
    public int endurance;
    public int mind;
    public int intelligence;
    public int faith;

    [Header("[ Attributes ]")]
    public int maxHp;
    public int maxMp;
    public int maxStamina;
    [Space(7)]
    public float magicForceModifier;
    public float divine;
    [Space(7)]
    public DamageNegation damageNegation;

    #region Debug
    [Header("[ Debug ]")]
    public bool resetAttributes;
    
    void OnValidate() {
        if (resetAttributes) {
            SetBaseAttributesAsPerStats();
            owner.curHp = maxHp;
            resetAttributes = false;
        }
    }

    #endregion


    public void Awake() {
        owner = GetComponent<Player>();
    }


    void SetBaseAttributesAsPerStats() {

        // Vitality
        maxHp = 100 + (vitality * 15);
        damageNegation.fire = vitality * 0.4f / 100;
        
        // Endurance
        maxStamina = 45 + (endurance * 2);
        
        // Mind
        maxMp = 100 + (mind * 2);
        damageNegation.magic = (mind * 0.25f) / 100;
        
        // Intelligence
        magicForceModifier = 1 + (intelligence * 0.035f);
        
        // Faith
        divine = faith * 0.04f;
    }

    void ModifyAttributesAsPerEquipment() {
        
        
    }
}