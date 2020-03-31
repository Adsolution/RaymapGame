//================================
//  By: Adsolution
//================================
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenSpace.Collide;

namespace RaymapGame {
    public partial class PersoController {
        //========================================
        //  Physics & Collision
        //========================================
        public Vector3 apprVelXZ => new Vector3(apprVel.x, 0, apprVel.z);
        public float fricMag => (fricXZ + fricY) * globalFricMult;
        public void SetFriction(float horizontal, float vertical) {
            fricXZ = horizontal; fricY = vertical;
        }
        public void SetFriction(float horizontal, float vertical, float angular) {
            fricXZ = horizontal; fricY = vertical; rotFric = angular;
        }
        public void SetVelH(float x, float z, float t = -1)
            => velXZ = Vector3.Lerp(velXZ, new Vector3(x, 0, z), tCheck(t));
        public void SetVelV(float y, float t = -1)
            => velY = Mathf.Lerp(velY, y, tCheck(t));
        public void GiveImpulse(Vector3 impulse)
            => vel += impulse;
        public void GiveForce(Vector3 force)
            => vel += force * dt;
        public void BounceY(Vector3 surfNrm, float factor = 1)
            => velY = Vector3.Reflect(vel, surfNrm).y * factor;
        public void Bounce3D(Vector3 surfNrm, float factor = 1)
            => vel = Vector3.Reflect(vel, surfNrm) * factor;
        public void BounceXZ(Vector3 surfNrm, float factor = 1)
            => velXZ = Vector3.Reflect(vel, surfNrm) * factor;
        public void ApplyGravity()
            => velY = Mathf.Clamp(velY + gravity * globalGravityMult * dt, -80, 80);
        public CollideInfo Raycast(Vector3 dir, float dist)
            => RayCollider.Raycast(pos, dir, dist);
        public CollideInfo Raycast(Vector3 offset, Vector3 dir, float dist)
            => RayCollider.Raycast(pos + offset, dir, dist);


        // Collision
        public bool StoodOnBy(PersoController perso) {
            if (perso == this || perso.velY != 0) return false;
            foreach (var c in GetComponentsInChildren<Collider>()) {
                if (perso.col.ground.hit.collider == c) {
                    return true;
                }
            }
            return false;
        }


        Dictionary<CollideType, BoundingSphere> col_s = new Dictionary<CollideType, BoundingSphere>();
        Dictionary<CollideType, Bounds> col_b = new Dictionary<CollideType, Bounds>();
        public bool HasCollisionType(CollideType collideType) {
            foreach (Transform child in transform)
                if (child.name.Contains($"Collide Set {collideType}"))
                    return true;
            return false;
        }
        public BoundingSphere GetCollisionSphere(CollideType collideType) {
            if (col_s.ContainsKey(collideType)) return new BoundingSphere(
                transform.localToWorldMatrix.MultiplyPoint(col_s[collideType].position), col_s[collideType].radius);
            foreach (Transform child in transform)
                if (child.name.Contains($"Collide Set {collideType}")) {
                    var ch = child.GetChild(0).GetChild(0);
                    if (ch.name.Contains("Spheres")) {
                        col_s.Add(collideType, new BoundingSphere(ch.transform.localPosition, ch.localScale.x / 2));
                        return col_s[collideType];
                    }
                }
            col_s.Add(collideType, new BoundingSphere());
            return col_s[collideType];
        }
        public Bounds GetCollisionBox(CollideType collideType) {
            if (col_b.ContainsKey(collideType)) return new Bounds(
                transform.localToWorldMatrix.MultiplyPoint(col_b[collideType].center), col_b[collideType].size);
            foreach (Transform child in transform)
                if (child.name.Contains($"Collide Set {collideType}")) {
                    var ch = child.GetChild(0).GetChild(0);
                    if (ch.name.Contains("Aligned Boxes")) {
                        col_b.Add(collideType, new Bounds(ch.transform.localPosition, ch.transform.localScale));
                        return col_b[collideType];
                    }
                }
            col_b.Add(collideType, new Bounds());
            return col_b[collideType];
        }
        public bool CheckCollisionZone(PersoController perso, CollideType collideType) {
            return perso.DistTo(GetCollisionSphere(collideType).position) < GetCollisionSphere(collideType).radius
                + perso.GetCollisionSphere(collideType).radius
                || GetCollisionBox(collideType).Contains(perso.pos);
        }
    }
}