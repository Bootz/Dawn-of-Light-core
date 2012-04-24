using System;
using DOL.Events;
using DOL.GS.PlayerTitles;
using DOL.Language;

namespace DOL.GS.Scripts
{
    public class CraftTitle : SimplePlayerTitle
    {
        public override string GetDescription(GamePlayer player)
        {
            return player.CraftTitle;
        }

        public override string GetValue(GamePlayer player)
        {
            return player.CraftTitle;
        }

        public override bool IsSuitable(GamePlayer player)
        {
            if (player.CraftingPrimarySkill != eCraftingSkill.NoCrafting)
            {
                return true;
            }
            return false;
        }
    }
}