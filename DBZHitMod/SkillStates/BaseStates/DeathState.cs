using DBZHitMod.Modules;
using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.SkillStates.BaseStates
{
    public class DeathState : GenericCharacterDeath
    {

        private float duration;
        public float baseDuration = 1f;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            base.PlayAnimation("Body", "Death", "attackSpeed", this.duration);
            Util.PlaySound(Sounds.die, base.gameObject);
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
