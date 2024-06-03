﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace ValheimLegends
{
    public class SE_Berserk : SE_Stats
    {
        public static Sprite AbilityIcon;
        public static GameObject GO_SEFX;

        [Header("SE_VL_Berserk")]
        public static float m_baseTTL = 18f;
        public float speedModifier = Class_Berserker.berserk_speedbonus_base;
        public float damageModifier = Class_Berserker.berserk_damagebonus_base;
        public float healthAbsorbPercent = Class_Berserker.berserk_healthabsorb_base;
        private float m_timer = 0f;
        private float m_interval = 3f;

        private float savedStaminaRegenDelay = 1f;

        public SE_Berserk()
        {
            base.name = "SE_VL_Berserk";
            m_icon = AbilityIcon;
            m_tooltip = $"Drains hp per second to boost damage by {(int)((damageModifier - 1f) * 100f)}%, speed by {(int)((speedModifier - 1f) * 100f)}%.\n*Additionally absorbs {(int)((healthAbsorbPercent - 1f) * 100f)}% of damage dealt as stamina";
            m_name = "Berserk";
            m_ttl = m_baseTTL;
        }

        public override void ModifySpeed(float baseSpeed, ref float speed, Character character, Vector3 dir)
        {
            speed *= speedModifier;
            base.ModifySpeed(baseSpeed, ref speed, character, dir);
        }

        public override void Setup(Character character)
        {
            savedStaminaRegenDelay = Traverse.Create(root: (Player)character).Field(name: "m_staminaRegenDelay").GetValue<float>();
            Traverse.Create(root: (Player)character).Field(name: "m_staminaRegenDelay").SetValue(0f);
            base.Setup(character);
        }

        public override void UpdateStatusEffect(float dt)
        {
            base.UpdateStatusEffect(dt);
            m_timer -= dt;
            if (m_timer <= 0f)
            {
                m_timer = m_interval;
                HitData hitData = new HitData();
                hitData.m_damage.m_spirit = Mathf.Clamp(.05f * m_character.GetMaxHealth(), 1f, 15f);
                hitData.m_point = m_character.GetEyePoint();
                m_character.ApplyDamage(hitData, true, true, HitData.DamageModifier.Normal);
                UnityEngine.Object.Instantiate(ZNetScene.instance.GetPrefab("fx_deathsquito_hit"), m_character.GetCenterPoint(), Quaternion.identity);
            }
        }

        public override bool IsDone()
        {
            if (m_ttl > 0f && m_time > m_ttl)
            {
                Traverse.Create(root: (Player)m_character).Field(name: "m_staminaRegenDelay").SetValue(savedStaminaRegenDelay);
            }

            return base.IsDone();
        }

        public override bool CanAdd(Character character)
        {
            return character.IsPlayer();
        }
    }
}
