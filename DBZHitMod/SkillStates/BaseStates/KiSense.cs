using DBZHitMod.Modules;
using EntityStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.Chat;

namespace HenryMod.SkillStates.BaseStates
{
    public class KiSense : GenericCharacterMain
    {
        public float baseDuration = 1f;
        private float duration;
        private Animator animator;
        private float CooldownTime = 0f;


        public override void OnEnter()
        {
            base.OnEnter();


        }

        public override void OnExit()
        {
            base.PlayAnimation("Attack", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

       
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            KiSenseSkill KSS = new KiSenseSkill();

            if(CooldownTime >= 10f)
            {
                CooldownTime = 0f;
                this.outer.SetNextState(KSS);
            }



            CooldownTime += Time.fixedDeltaTime;

           
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);

        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);

        }
    }
}