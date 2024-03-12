using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ValheimLegends
{
    public class SE_PowerShot : StatusEffect
    {
        public static Sprite AbilityIcon;
        public static GameObject GO_SEFX;

        [Header("SE_VL_PowerShot")]
        public static float m_baseTTL = 3f;
        public float damageBonus = Class_Ranger.powershot_damagebonus_base;
        public int hitCount = Class_Ranger.powershot_charges_base;
        public bool shouldActivate = true;

        public SE_PowerShot()
        {
            base.name = "SE_VL_PowerShot";
            m_icon = AbilityIcon;
            m_tooltip = $"Increases damage of next {hitCount} projectiles by {(int)((damageBonus - 1f) * 100f)}%";
            m_name = "PowerShot";
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
