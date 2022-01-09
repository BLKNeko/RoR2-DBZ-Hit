using DBZHitMod.Modules;
using EntityStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.Chat;

namespace HenryMod.SkillStates
{
    public class KiSenseSkill : BaseSkillState
    {
        public static float damageCoefficient = 0.1f;
        public static float buffDamageCoefficient = 1f;
        public float baseDuration = 0.4f;
        public static float attackRecoil = 0.5f;
        public static float hitHopVelocity = 5.5f;
        public static float baseEarlyExit = 0.25f;
        public int swingIndex;

        //public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactMercSwing");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/lunarneedledamageeffect");

        //public GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFXSlashMerc");

        private float earlyExitDuration;
        private float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        //private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExitDuration = KiSenseSkill.baseEarlyExit / this.attackSpeedStat;
            this.hasFired = false;
            //this.animator = base.GetModelAnimator();
            //this.swordController = base.GetComponent<PaladinSwordController>();
            //base.StartAimMode(0.5f + this.duration, false);
            //base.characterBody.isSprinting = false;

            //base.characterBody.healthComponent.AddBarrier(base.characterBody.damage);


            //Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "KiSenseSkill" } });

            Util.PlaySound(Sounds.kiSense, base.gameObject);

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "KiSenseHitBox");
            }

            //if (this.swingIndex == 0) base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //else base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //base.PlayAnimation("Attack", "Punch1", "attackSpeed", this.duration);




            float dmg = KiSenseSkill.damageCoefficient;
            //if (this.swordController && this.swordController.swordActive) dmg = Slash.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.WeakOnHit;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = KiSenseSkill.hitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 1f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();


        }

        public override void OnExit()
        {
            //base.PlayAnimation("Attack", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                HitBoxGroup hitBoxGroup = null;
                Transform modelTransform = base.GetModelTransform();

                if (modelTransform)
                {
                    hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "KiSenseHitBox");
                }

                //if (this.swingIndex == 0) base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
                //else base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
                //base.PlayAnimation("Attack", "Punch1", "attackSpeed", this.duration);




                float dmg = KiSenseSkill.damageCoefficient;
                //if (this.swordController && this.swordController.swordActive) dmg = Slash.buffDamageCoefficient;

                this.attack = new OverlapAttack();
                this.attack.damageType = DamageType.WeakOnHit;
                this.attack.attacker = base.gameObject;
                this.attack.inflictor = base.gameObject;
                this.attack.teamIndex = base.GetTeam();
                this.attack.damage = dmg * this.damageStat;
                this.attack.procCoefficient = 1;
                this.attack.hitEffectPrefab = KiSenseSkill.hitEffectPrefab;
                this.attack.forceVector = Vector3.zero;
                this.attack.pushAwayForce = 1f;
                this.attack.hitBoxGroup = hitBoxGroup;
                this.attack.isCrit = base.RollCrit();

                if (this.attack.Fire())
                {
                    //Util.PlaySound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject);
                    //Util.PlaySound(MinerPlugin.Sounds.Hit, base.gameObject);


                    if (!this.hasHopped)
                    {
                        if (base.characterMotor && !base.characterMotor.isGrounded)
                        {
                            base.SmallHop(base.characterMotor, TimeBreaker.hitHopVelocity);
                        }

                        this.hasHopped = true;
                    }

                    if (!this.inHitPause)
                    {
                        this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "FireArrow.playbackRate");
                        this.hitPauseTimer = (0.6f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                        this.inHitPause = true;
                    }
                }

            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
          

            if (hasFired)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            else
                FireAttack();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
               
                this.outer.SetNextStateToMain();

            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}