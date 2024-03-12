using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ValheimLegends
{
    public class SE_Execute : StatusEffect
    {
        public static Sprite AbilityIcon;
        public static GameObject GO_SEFX;

        [Header("SE_VL_Execute")]
        public static float m_baseTTL = 3f;
        public float staggerForce = Class_Berserker.execute_staggerbonus;
        public float damageBonus = Class_Berserker.execute_damagebonus;
        public int hitCount = (int)Class_Berserker.execute_charges_base;

        public SE_Execute()
        {
            base.name = "SE_VL_Execute";
            m_icon = AbilityIcon;
            m_tooltip = $"Increases damage of next {hitCount} hits by {(int)((damageBonus - 1f) * 100f)}%";
            m_name = "Execute";
            m_ttl = m_baseTTL;
        }

        public override void UpdateStatusEffect(float dt)
        {
            m_ttl = hitCount;
            m_time = 0;
            base.UpdateStatusEffect(dt);
        }

        public override bool CanAdd(Character character)
        {
            return character.IsPlayer();
        }
    }
}
