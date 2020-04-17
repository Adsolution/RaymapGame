//================================
//  By: Adsolution
//================================
using System;
using UnityEngine;
using OpenSpace.Collide;

namespace RaymapGame {
    public partial class PersoController {
        //========================================
        //  Health & Combat
        //========================================
        public bool dead => hitPoints == 0;
        public void SetHealth(float max, bool healFull = true) {
            startHitPoints = maxHitPoints = max;
            if (healFull) HealFull();
        }
        public void HealFull()
            => hitPoints = maxHitPoints;
        public void Heal(float points) {
            hitPoints += points;
            if (hitPoints > maxHitPoints)
                hitPoints = maxHitPoints;
        }
        public void Kill()
            => Damage(hitPoints);
        public void Damage(float points) {
            if (dead) return;
            hitPoints -= points;
            if (hitPoints <= 0) {
                hitPoints = 0;
                SetRule("");
                OnDeath();
                foreach (var dtr in deathEvents)
                    dtr.Invoke();
            }
        }
        public PersoController ReceiveProjectiles(out PersoController projectile, float iFrame = 0)
            => projectile = ReceiveProjectiles(iFrame);
        public PersoController ReceiveProjectiles(float iFrame = 0) {
            if (dead || t_iFrame.active) return null;
            foreach (Rayman2.Persos.projectiles pr in GetPersos(typeof(Rayman2.Persos.projectiles)))
                if (pr != null && pr.creator != this && Dist(GetCollisionSphere(CollideType.ZDE).position, pr.pos) 
                    < pr.radius + GetCollisionSphere(CollideType.ZDE).radius) {

                    lastDmgSrc = pr;
                    Damage(pr.damage);
                    t_iFrame.Start(iFrame);
                    OnHit();
                    pr.Remove();
                    return pr;
                }
            return null;
        }
        Timer t_iFrame = new Timer();



        // Combat
        public void EnterCombat() {
            foreach (var c in combatEvents)
                c.Invoke();
            OnEnterCombat();
        }
        public bool hasTarget => target != null;
        public PersoController FindTarget(float maxDist, float maxAngle = 30)
            => FindTarget(typeof(PersoController), maxDist, maxAngle, true);
        public PersoController FindTarget(Type persoType, float maxDist, float maxAngle = 30)
            => FindTarget(persoType, maxDist, maxAngle, false);
        public P FindTarget<P>(float maxDist, float maxAngle = 30) where P : PersoController
            => (P)FindTarget(typeof(P), maxDist, maxAngle, false);
        public PersoController FindTarget(Type persoType, float maxDist, float maxAngle, bool onlyZDE)
            => target = GetClosestPerso(persoType, (p) =>
                    p != creator && !p.dead && DistTo(p) < maxDist
                    && Vector3.Angle(forward, new Vector3(p.pos.x - pos.x, 0, p.pos.z - pos.z)) < maxAngle
                    && Mathf.Abs(p.pos.y - pos.y) < DistTo(p) / 4
                    && (!onlyZDE || p.HasCollisionType(CollideType.ZDE)));
        public PersoController Shoot()
            => Shoot(projectileType, projectileVel);
        public PersoController Shoot(float vel)
            => Shoot(projectileType, vel);
        public PersoController Shoot(Vector3 target)
            => Shoot(projectileType, projectileVel, target);
        public PersoController Shoot(float vel, Vector3 target)
            => Shoot(projectileType, vel, target);
        public PersoController Shoot(Type projectileType, float vel)
            => Shoot(projectileType, vel, projectileSpawnPos + forward);
        public PersoController Shoot(bool atTarget)
            => Shoot(projectileType, projectileVel, atTarget && target != null
                ? target.GetCollisionSphere(CollideType.ZDE).position : projectileSpawnPos + forward);
        public PersoController Shoot(Type projectileType, float vel, Vector3 target) {
            Clone(projectileType, out PersoController p, projectileSpawnPos, rot);
            p.vel = (target - projectileSpawnPos).normalized * vel;
            p.SetRule("Shot");
            return p;
        }

        public Vector3 projectileSpawnPos => pos + matRot.MultiplyPoint3x4(projectileOffset);


        public Rayman2.Persos.Alw_Explosion_model CreateExplosion(Vector3 pos, float radius = 6)
            => CreateExplosion(this, pos, radius);
        public static Rayman2.Persos.Alw_Explosion_model CreateExplosion(PersoController spawner, Vector3 pos, float radius = 6) {
            var expl = (Rayman2.Persos.Alw_Explosion_model)Clone(typeof(Rayman2.Persos.Alw_Explosion_model), out var c, pos, Vector3.zero, spawner);
            expl.radius = radius;
            expl.Explode();
            return expl;
        }





        // Carrying / Throwing

        public PersoController CarryPerso(PersoController persoToHold)
            => CarryPerso(persoToHold, handChannel);
        public PersoController CarryPerso(PersoController persoToHold, int handChannel) {
            carryPerso = persoToHold;
            this.handChannel = handChannel;
            if (carryPerso != null) {
                carryPerso.SetCollision(false);
                carryPerso.vel = Vector3.zero;
            }
            return persoToHold;
        }

        Vector3 thrStart, thrTarg;
        public void ThrowCarriedFoward(Target target = null) {
            if (carryPerso != null) {
                carryPerso.SetCollision(true);
                if (target == null) {
                    carryPerso.velY = 10;
                    carryPerso.velXZ = forward * 25;
                    carryPerso.OnThrown();
                }
                else {
                    carryPerso.thrStart = carryPerso.pos;
                    carryPerso.thrTarg = target;
                    carryPerso.inThrowArc = true;
                }
                carryPerso = null;
                Timers("ThrowBlock").Start(1);
            }
        }
        public void ThrowCarriedUp() {
            if (carryPerso != null) {
                carryPerso.SetCollision(true);
                carryPerso.velY = 20;
                carryPerso.velXZ = Vector3.zero;
                carryPerso.OnThrown();
                carryPerso = null;
                Timers("ThrowBlock").Start(1);
            }
        }


        public void DropCarried() {
            carryPerso.SetCollision(true);
            carryPerso.OnThrown();
            carryPerso = null;
            Timers("ThrowBlock").Start(1);
        }
    }
}