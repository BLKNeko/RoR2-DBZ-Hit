using DBZHitMod.Modules;
using EntityStates;
using RoR2;
using UnityEngine;

namespace HenryMod.SkillStates
{
    public class TimeHit : BaseSkillState
    {
        public static float damageCoefficient = 1f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/lunarneedleexplosioneffect");

        private float duration;
        private float fireTime;
        private int repeat = 0;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = TimeHit.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";
            //Util.PlaySound("HenryShootPistol", base.gameObject);
            Util.PlaySound(Sounds.timeHit, base.gameObject);

            base.PlayAnimation("Body", "Idle", "TimeHit.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
                //this.hasFired = true;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                
                    Ray aimRay = base.GetAimRay();
                    //base.AddRecoil(-1f * TimeHit.recoil, -2f * TimeHit.recoil, -0.5f * TimeHit.recoil, 0.5f * TimeHit.recoil);

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = TimeHit.damageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = TimeHit.range,
                        force = TimeHit.force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        //tracerEffectPrefab = TimeHit.tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = TimeHit.hitEffectPrefab,
                    }.Fire();

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                //this.Fire();
            }


            
                if (this.repeat <= 3)
                {
                    this.Fire();
                    repeat++;
                }
                    
                else
                this.outer.SetNextStateToMain();

            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}