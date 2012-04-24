using System;
using DOL.Database;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;

namespace DOL.GS.RealmAbilities
{
    public class TheEmptyMindAbility : TimedRealmAbility
    {
        public TheEmptyMindAbility(DBAbility dba, int level) : base(dba, level) { }

        public const Int32 m_duration = 45000; //45 seconds

        public override void Execute(GameLiving living)
        {
            if (CheckPreconditions(living, DEAD | SITTING | MEZZED | STUNNED)) return;

            foreach (GamePlayer t_player in living.GetPlayersInRadius(WorldMgr.INFO_DISTANCE))
            {
                if (t_player == living && living is GamePlayer)
                {
                    (living as GamePlayer).Out.SendMessage("You clear your mind and become more resistant to magic damage!", eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
                }
                else
                {
                    t_player.Out.SendMessage(living.Name + " casts a spell!", eChatType.CT_Spell, eChatLoc.CL_SystemWindow);
                }
            }
            new TheEmptyMindEffect(this.Level).Start(living);
            DisableSkill(living);
        }

        public override int GetReUseDelay(int level)
        {
            return 600;
        }
    }
}
