using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ValheimLegends
{
    public class SE_Ability3_CD : StatusEffect
    {
        public static Sprite AbilityIcon = ValheimLegends.Ability3_Sprite;
        public static GameObject GO_SEFX;

        public SE_Ability3_CD()
        {
            base.name = "SE_VL_Ability3_CD";
            m_icon = ValheimLegends.Ability3_Sprite;
            m_tooltip = ValheimLegends.Ability3_Name + " $Legends_cooldown";
            m_name = ValheimLegends.Ability3_Name + " $Legends_cooldown";
        }

        public override bool CanAdd(Character character)
        {
            return character.IsPlayer();
        }
    }
}
