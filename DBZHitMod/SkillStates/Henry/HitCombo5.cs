using DBZHitMod.Modules;
using EntityStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.Chat;

namespace HenryMod.SkillStates
{
    public class HitCombo5 : BaseSkillState
    {
        public static float damageCoefficient = 1.5f;
        public static float buffDamageCoefficient = 1f;
        public float baseDuration = 0.25f;
        public static float attackRecoil = 0.5f;
        public static float hitHopVelocity = 5.5f;
        public static float baseEarlyExit = 0.25f;
        public int swingIndex;

        //public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactMercSwing");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/lunarneedledamageeffect");

        public GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFXSlashMerc");

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
            this.earlyExitDuration = HitCombo5.baseEarlyExit / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            //this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            //base.characterBody.isSprinting = false;

            //base.characterBody.healthComponent.AddBarrier(base.characterBody.damage);

            //Chat.SendBroadcastChat(new SimpleChatMessage { baseToken = "<color=#e5eefc>{0}</color>", paramTokens = new[] { "HitCombo5" } });

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "RHandHitBox");
            }

            //if (this.swingIndex == 0) base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //else base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //base.PlayAnimation("Kick", "Kick", "attackSpeed", this.duration);
            base.PlayAnimation("Attack", "UpperD", "attackSpeed", this.duration);




            float dmg = HitCombo4.damageCoefficient;
            //if (this.swordController && this.swordController.swordActive) dmg = Slash.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = (Util.CheckRoll(20f, base.characterBody.master) ? DamageType.Stun1s : DamageType.Generic);
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = HitCombo5.hitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 1f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void OnExit()
        {
            base.PlayAnimation("Attack", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                //Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);
                //Util.PlaySound(Sounds.zSlash1Voice, base.gameObject);
                //Util.PlaySound(Sounds.zSlash1SFX, base.gameObject);
                Util.PlaySound(Sounds.hit3, base.gameObject);

                //string muzzleString = null;
                // if (this.swingIndex == 0) muzzleString = "SwingLeft";
                //else muzzleString = "SwingRight";


                // EffectManager.SimpleMuzzleFlash(Modules.Assets.swordSwing, base.gameObject, muzzleString, true);
                EffectManager.SimpleMuzzleFlash(EntityStates.Merc.GroundLight.comboSwingEffectPrefab, base.gameObject, "SwingLeft", true);

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * HitCombo5.attackRecoil, -2f * HitCombo5.attackRecoil, -0.5f * HitCombo5.attackRecoil, 0.5f * HitCombo5.attackRecoil);

                    Ray aimRay = base.GetAimRay();

                    //if (this.swordController && this.swordController.swordActive)
                    //{
                    //    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeam, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                    // }

                    if (this.attack.Fire())
                    {
                        //Util.PlaySound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject);
                        //Util.PlaySound(MinerPlugin.Sounds.Hit, base.gameObject);
                        

                        if (!this.hasHopped)
                        {
                            if (base.characterMotor && !base.characterMotor.isGrounded)
                            {
                                base.SmallHop(base.characterMotor, HitCombo5.hitHopVelocity);
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
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("FireArrow.playbackRate", 1f);
            }

            if (this.stopwatch >= this.duration * 0.2f)
            {
                this.FireAttack();
            }

            if (base.fixedAge >= (this.duration) && base.isAuthority && base.inputBank.skill1.down)
            {

                //int index = this.swingIndex;
                // if (index == 0) index = 1;
                //else index = 0;
                HitCombo1 HC1 = new HitCombo1();
                this.outer.SetNextState(HC1);

            }

            if (base.fixedAge >= this.duration && base.isAuthority && !base.inputBank.skill1.down)
            {
                this.outer.SetNextStateToMain();
                return;
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