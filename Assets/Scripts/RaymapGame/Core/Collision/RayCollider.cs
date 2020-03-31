//================================
//  By: Adsolution
//================================

using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using OpenSpace.Collide;

namespace RaymapGame
{
    public class RayCollider {
        public MonoBehaviour controller;
        public bool groundEnabled = true;
        public bool wallEnabled;
        public float radius = 0.375f;
        public float bottom = 0.5f;
        public float top = 2;
        public float groundLevel;
        public float groundDepth = 0.25f;
        public float ceilingHeight = 0.5f;
        public float wallAngle = 0.707f;

        public static float waterShallowDepth = 1.5f;
        public bool waterAutoSurface = true;
        public float waterAutoSurfaceDepth = 4;
        public float waterRestOffset = 1.125f;

        
        public CollideInfo ground, groundFar, wall, ceiling, water;
        public bool atWaterSurface => water.hit.distance > 0 && water.hit.distance < 1.5f + waterRestOffset;
        public bool waterIsShallow => _waterIsShallow;
        bool _waterIsShallow;


        public PersoController perso => controller as PersoController;
        public Vector3 pos => perso != null ? perso.pos : controller.transform.position;

        public void ClearAll() {
            ground = new CollideInfo();
            groundFar = new CollideInfo();
            wall = new CollideInfo();
            ceiling = new CollideInfo();
            water = new CollideInfo();
        }


        //========================================
        //  Continuous Raycasting
        //========================================
        public static int raycastDepth = 5;

        public static CollideInfo RaycastMouse(bool collideVisual = false) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Raycast(ray.origin, ray.direction, 10000, collideVisual);
        }

        public static CollideInfo Raycast(Vector3 origin, Vector3 direction, float distance, bool collideVisual = false) {
            bool hit = Physics.Raycast(origin, direction, out var newhit, distance, 1 << (collideVisual ? 8 : 9), QueryTriggerInteraction.Ignore);
            /*
            RaycastHit newhit = new RaycastHit();
            int casts = 0;
            for (float dist = 0; dist < distance && casts < raycastDepth; dist += newhit.distance, casts++) {
                bool hit = Physics.Raycast(casts == 0 ? origin : newhit.point, direction, out newhit, distance - dist, 1 << 9, QueryTriggerInteraction.Ignore);
                if (hit) {
                    var colComp = newhit.collider.GetComponent<CollideComponent>();
                    if (colComp != null) {
                        if (((CollideMaterialType)colComp.col.type & types) == types)
                            return new CollideInfo(newhit, colComp.collide);
                    }
                } else break;
            }*/
            return new CollideInfo(newhit);
        }

        public CollideInfo RaycastGround(CollideMaterial.CollisionFlags_R2 types = CollideMaterial.CollisionFlags_R2.All)
            => Raycast(pos + Vector3.up * (1 + groundLevel), Vector3.down, 1 + groundDepth);
        public CollideInfo RaycastCeiling(CollideMaterial.CollisionFlags_R2 types = CollideMaterial.CollisionFlags_R2.All)
            => Raycast(pos + Vector3.up * (top - 0.5f), Vector3.up, ceilingHeight + 0.5f);



        //========================================
        //  Collision Actions
        //========================================
        public void BlockCollision(float seconds) => t_blockCollision.Start(seconds);
        Timer t_blockCollision = new Timer();

        public void ApplyZDRCollision() {
            if (perso == null || !perso.HasCollisionType(CollideType.ZDR)) return;

            foreach (var p in PersoController.GetPersos(typeof(PersoController)).Where((x) => x.HasCollisionType(CollideType.ZDR))) {
                var zdr1 = perso.GetCollisionSphere(CollideType.ZDR);
                var zdr2 = p.GetCollisionSphere(CollideType.ZDR);

                float dist = PersoController.Dist(zdr1.position, zdr2.position);
                float maxDist = zdr1.radius + zdr2.radius;

                if (dist < maxDist) {
                    perso.pos += (zdr1.position - zdr2.position).normalized * (maxDist - dist);
                }
            }
        }

        public void UpdateCollision()
        {
            UpdateGroundCollision();
            UpdateWallCollision();
        }

        public void UpdateGroundCollision()
        {
            var p = pos;
            if (groundEnabled) {
                var groundHold = RaycastGround();
                if (groundHold.hit.normal.y > wallAngle)
                    ground = groundHold;
                else ground = new CollideInfo();

                platformPerso = ground.hit.collider?.GetComponentInParent<PersoController>();
                groundFar = Raycast(p + Vector3.up * (1 - groundLevel), Vector3.down, 1 + 10);
            }
            else {
                ground = new CollideInfo();
                groundFar = new CollideInfo();
            }
        }

        public PersoController platformPerso;
        Vector3 platPosPrev;
        Quaternion platRotPrev;
        public void StickToGround() => StickToGround(ref perso.pos);
        public void StickToGround(ref Vector3 pos) {
            //if (!ground.AnyGround) return;
            if (platformPerso != null) {
                if (perso.newRule) {
                    platPosPrev = ground.hit.collider.transform.position;
                    platRotPrev = ground.hit.collider.transform.rotation;
                }
                pos += platformPerso.deltaPos;
                perso.rot *= platformPerso.deltaRot;
                pos -= /*(ground.hit.collider.transform.position - platPosPrev)
                    + */(Matrix4x4.Rotate(
                        Quaternion.Inverse(platRotPrev) * ground.hit.collider.transform.rotation)
                    .MultiplyPoint3x4(platformPerso.pos - pos)
                    - (platformPerso.pos - pos));

                platPosPrev = ground.hit.collider.transform.position;
                platRotPrev = ground.hit.collider.transform.rotation;
            }
            pos.y = ground.hit.point.y - groundLevel;
            perso.velY = 0;
        }

        public void StickToPlatformCentre() {
            if (!ground.AnyGround) return;
            if (platformPerso != null) {
                perso.pos = platformPerso.pos;
            }
            perso.pos.y = ground.hit.point.y;
        }

        Vector3 wallPush, ceilPush;
        public void UpdateWallCollision()
        {
            if (!wallEnabled) return;
            wallPush = new Vector3();
            wall = new CollideInfo();
            var most = new Vector3();
            RaycastHit shortest = new RaycastHit { distance = radius };
            for (float h = bottom; h <= top; h += 0.25f)
                for (float a = 0; a < Mathf.PI * 2; a += Mathf.PI * 2 / 16)
                {
                    var col = Raycast(pos + h * Vector3.up, new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)), radius);
                    if (col.AnyWall && col.hit.distance < shortest.distance)
                    {
                        most = col.hit.normal * (-col.hit.distance + radius);
                        shortest = col.hit;
                        wall = col;
                    }
                }
            wallPush = new Vector3(most.x, 0, most.z);

            ceilPush = new Vector3();
            ceiling = RaycastCeiling();
            if (ceiling.AnyWall)
                ceilPush = ceiling.hit.normal * (-ceiling.hit.distance + ceilingHeight);
        }

        public void ApplyWallCollision() => ApplyWallCollision(ref perso.pos);
        public void ApplyWallCollision(ref Vector3 pos)
        {
            pos += wallEnabled ? wallPush : Vector3.zero
                + ceilPush;
        }


        public void UpdateWaterCollision()
        {
            water = Raycast(pos, Vector3.up, 4);
            //var w = waterInfo.hit;
            /*
            _waterIsShallow = w.distance < waterShallowDepth
                && Raycast(w.point + Vector3.down * 0.1f, Vector3.down)
                && GetCollision(shalGround, waterShallowDepth - 0.1f).AnyGround;*/
        }


        public void ApplyWaterCollision() => ApplyWaterCollision(ref perso.pos, ref perso.velY);
        public void ApplyWaterCollision(ref Vector3 pos)
        {
            if (water.hit.distance < 1 + waterRestOffset)
                pos.y = water.hit.point.y - waterRestOffset;
        }
        public void ApplyWaterCollision(ref Vector3 pos, ref float velY)
        {
            ApplyWaterCollision(ref pos);
            if (waterAutoSurface && water.hit.distance < waterAutoSurfaceDepth)
                velY += 4 * (controller is IInterpolate ? Time.fixedDeltaTime : Time.deltaTime);
        }
    }
}