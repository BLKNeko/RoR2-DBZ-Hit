using DBZHitMod.Modules;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.SkillStates
{
    public class TimeSkip : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;

        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = TimeSkip.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            //base.characterBody.SetAimTimer(2f);
            //this.muzzleString = "Muzzle";

            Util.PlaySound(Sounds.timeSkip, base.gameObject);

            //base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            if (NetworkServer.active)
            {
                //base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, 3f * Roll.duration);
                base.characterBody.AddTimedBuff(Modules.Buffs.speedBuff, 5f * Roll.duration);
                base.characterBody.AddTimedBuff(Modules.Buffs.atkspeedBuff, 5f * Roll.duration);
                //base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f * Roll.duration);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 5f * Roll.duration);
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}