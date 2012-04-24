//Andraste v2.0 - Vico

using System;
using DOL.Events;
using DOL.GS;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;

namespace DOL.GS.Spells
{
    [SpellHandler("ArrowDamageTypes")]
    public class ArrowDamageTypes : SpellHandler
    {
        /// <summary>
        /// Does this spell break stealth on finish?
        /// </summary>
        public override bool UnstealthCasterOnFinish
        {
            get { return false; }
        }

        /// <summary>
        /// Does this spell break stealth on start?
        /// </summary>
        public override bool UnstealthCasterOnStart
        {
            get { return false; }
        }

        public ArrowDamageTypes(GameLiving caster, Spell spell, SpellLine line) : base(caster, spell, line) { }
    }
}