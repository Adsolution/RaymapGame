//================================
//  By: Adsolution
//================================

using UnityEngine;
using OpenSpace.Collide;

namespace RaymapGame {

    public struct CollideInfo {
        public GeometricObjectElementCollideTriangles collide;
        public RaycastHit hit;
        public PersoController hitPerso;

        public CollideMaterial mat => collide?.gameMaterial?.collideMaterial;
        public bool isValid;
        public CollideType collideType => collide.geo.type;
        public CollideMaterial.CollisionFlags_R2 type {
            get => (CollideMaterial.CollisionFlags_R2)mat?.type;
            set { mat.type = (ushort)type; }
        }
        
        public CollideInfo(RaycastHit hit) { 
            var c = hit.collider?.GetComponent<CollideComponent>();
            if (c != null && hit.normal != Vector3.zero) {
                isValid = true;
                collide = c.collide;
                this.hit = hit;
            }
            else if (Main.anyCollision && hit.normal != Vector3.zero) {
                isValid = true;
                collide = null;
                this.hit = hit;
            }
            else {
                isValid = false;
                collide = null;
                this.hit = new RaycastHit();
            }
            hitPerso = hit.collider?.GetComponentInParent<PersoController>();
        }
        public CollideInfo(RaycastHit hit, GeometricObjectElementCollideTriangles collide) {
            isValid = true;
            this.hit = hit;
            this.collide = collide;
            hitPerso = hit.collider?.GetComponentInParent<PersoController>();
        }


        bool Checks => Main.anyCollision || (isValid && collide?.gameMaterial?.collideMaterial != null);
        public bool None => !isValid;
        public bool Any => isValid;
        public bool Generic => isValid && collide?.gameMaterial?.collideMaterial == null;
        public bool AnyGround => Generic || GrabbableLedge || Trampoline || ClimbableWall;
        public bool AnyWall => Generic || GrabbableLedge || Slide || Trampoline || Wall || ClimbableWall || HangableCeiling;

        public bool Slide => Checks && (!Main.anyCollision && mat.Slide);
        public bool Trampoline => Checks && (!Main.anyCollision && mat.Trampoline);
        public bool GrabbableLedge => Checks && (!Main.anyCollision && mat.GrabbableLedge);
        public bool Wall => Checks && (!Main.anyCollision && mat.Wall);
        public bool FlagUnknown => Checks && (!Main.anyCollision && mat.FlagUnknown);
        public bool HangableCeiling => Checks && (!Main.anyCollision && mat.HangableCeiling);
        public bool ClimbableWall => Checks && (!Main.anyCollision && mat.ClimbableWall);
        public bool Electric => Checks && (!Main.anyCollision && mat.Electric);
        public bool LavaDeathWarp => Checks && (!Main.anyCollision && mat.LavaDeathWarp);
        public bool FallTrigger => Checks && (!Main.anyCollision && mat.FallTrigger);
        public bool HurtTrigger => Checks && (!Main.anyCollision && mat.HurtTrigger);
        public bool DeathWarp => Checks && (!Main.anyCollision && mat.DeathWarp);
        public bool FlagUnk2 => Checks && (!Main.anyCollision && mat.FlagUnk2);
        public bool FlagUnk3 => Checks && (!Main.anyCollision && mat.FlagUnk3);
        public bool Water => Checks && (!Main.anyCollision && mat.Water);
        public bool NoCollision => Checks && (!Main.anyCollision && mat.NoCollision);
    }
}