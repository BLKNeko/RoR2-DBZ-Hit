using DBZHitMod.Modules;
using EntityStates;
using RoR2;
using RoR2.Skills;
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
        private float CooldownResetTime = 0f;
        private bool reseted = false;


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

           

            if (CooldownTime >= 10f)
            {
                CooldownTime = 0f;
                this.outer.SetNextState(KSS);
            }

            if(CooldownResetTime >= 20f)
            {
                if(Util.CheckRoll((4 + base.characterBody.level), base.characterBody.master) && !reseted){
                    this.skillLocator.secondary.Reset();
                    reseted = true;
                    Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "SecReset" } });
                }

                if (Util.CheckRoll((4 + base.characterBody.level), base.characterBody.master) && !reseted)
                {
                    this.skillLocator.utility.Reset();
                    reseted = true;
                    Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "UtilReset" } });
                }

                if (Util.CheckRoll((4 + base.characterBody.level), base.characterBody.master) && !reseted)
                {
                    this.skillLocator.special.Reset();
                    reseted = true;
                    Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "SpecReset" } });
                }

                reseted = false;
                CooldownResetTime = 0f;

            }


            CooldownResetTime += Time.fixedDeltaTime;
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