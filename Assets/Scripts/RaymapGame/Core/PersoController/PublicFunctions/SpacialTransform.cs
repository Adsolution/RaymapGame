//================================
//  By: Adsolution
//================================
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenSpace.Collide;
using static RaymapGame.InputEx;
using System.Reflection;
using System.Linq;

namespace RaymapGame {
    public partial class PersoController {
        //========================================
        //  Spacial
        //========================================
        public Vector3 worldForward => Vector3.forward;
        public Vector3 worldRight => Vector3.right;
        public Vector3 worldUpward => Vector3.up;
        public Matrix4x4 matRot => Matrix4x4.Rotate(Quaternion.Euler(rot.eulerAngles) * Quaternion.Euler(0, 180, 0));
        public Vector3 forward => matRot.MultiplyVector(Vector3.forward);
        public Vector3 right => matRot.MultiplyVector(Vector3.right);
        public Vector3 upward => matRot.MultiplyVector(Vector3.up);

        public Vector3 centerRel => new Vector3(0, (col.bottom + col.top) / 2, 0);
        public Vector3 center => pos + centerRel;
        public Vector3 GetVisCenter() {
            int n = 0;
            Vector3 cens = Vector3.zero;
            foreach (var mr in GetComponentsInChildren<MeshRenderer>()) {
                cens += transform.localToWorldMatrix.MultiplyPoint3x4(mr.bounds.center);
                n++;
            }
            return cens / n;
        }

        public int sector => Main.controller.sectorManager.sectors.IndexOf(!Main.isRom ? perso.sector : persoRom.sector);
        public static int activeSector => Main.controller.sectorManager.sectors.IndexOf(Main.controller.sectorManager.activeSector);
        public bool outOfSector => sector != activeSector;
        public bool outOfActiveRadius => Main.mainActor == null || DistTo(mainActor) > activeRadius;
        public Vector3 worldNullPos => new Vector3(0, -10000, 0);

        public float rndAngle => UnityEngine.Random.Range(-180, 180);

        public static Vector3 SwapYZ(Vector3 vec)
            => new Vector3(vec.x, vec.z, vec.y);

        public static float Dist(Target from, Target to)
            => Vector3.Distance(from, to);
        public float DistTo(Target point)
            => Dist(pos, point);
        public float DistTo2D(Target point)
            => Dist(new Vector3(point.x, 0, point.z), new Vector3(pos.x, 0, pos.z));
        public float DistTo(PersoController perso)
            => perso == null ? float.PositiveInfinity : DistTo(perso.pos);
        public float DistTo2D(PersoController perso)
            => perso == null ? float.PositiveInfinity : DistTo2D(perso.pos);

        public static float VecAngleY(Target origin, Target target)
            => -90 + Mathf.Rad2Deg * -Mathf.Atan2(origin.z - target.z, origin.x - target.x);
        public float VecAngleY(Target target)
            => VecAngleY(pos, target);
        public static float VecAngleX(Target origin, Target target)
            => 90 + Mathf.Rad2Deg * Mathf.Atan2(origin.y - target.y, Dist(origin, target));
        public float VecAngleX(Target target)
            => VecAngleX(pos, target);

        public bool IsInLevel(string lvlName)
            => lvlName.ToLowerInvariant() == Main.lvlName.ToLowerInvariant();
        public bool IsInActiveSector(int sectorIndex)
            => sector == Main.controller.sectorManager.sectors.IndexOf(mainActor.perso.sector);
        public bool IsInSector(int sectorIndex)
            => perso.sector == Main.controller.sectorManager.sectors[sectorIndex];
        public bool IsWithinCyl(Vector3 centre, float radius, float maxHeight)
            => DistTo2D(centre) < radius && pos.y < maxHeight;



        //========================================
        //  Transform
        //========================================
        public void SetNullPos()
            => pos = worldNullPos;
        public void SetRotY(float angle, float t = -1)
            => rot = Quaternion.Slerp(rot, Quaternion.Euler(rot.eulerAngles.x, angle, rot.eulerAngles.z), tCheck(t));
        public void SetRotX(float angle, float t = -1)
            => rot = Quaternion.Slerp(rot, Quaternion.Euler(angle + 180, rot.eulerAngles.y, rot.eulerAngles.z), tCheck(t));
        public void RotateY(float angle, float t = -1)
            => rot.eulerAngles += new Vector3(0, angle, 0) * tCheck(t);
        public void RotateX(float angle, float t = -1)
            => rot.eulerAngles += new Vector3(angle, 0, 0) * tCheck(t);
        public void RotateLocalY(float angle, float t = -1)
            => rot = Quaternion.SlerpUnclamped(rot, rot * Quaternion.Euler(0, angle, 0), tCheck(t));
        public void RotateLocalX(float angle, float t = -1)
            => rot = Quaternion.SlerpUnclamped(rot, rot * Quaternion.Euler(angle, 0, 0), tCheck(t));
        public void LookAt(Target target, float t = -1)
            => rot = lookAt(target, 0, 0, t);
        public void LookAt2D(Target target, float t = -1)
            => LookAt(new Vector3(target.x, pos.y, target.z), t);
        public void LookAtX(Target target, float addDegrees, float t = -1)
            => rot.eulerAngles = new Vector3(lookAt(target, addDegrees, 0, t).eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
        public void LookAtY(Target target, float addDegrees, float t = -1)
            => rot.eulerAngles = new Vector3(rot.eulerAngles.x, lookAt(target, 0, addDegrees, t).eulerAngles.y, rot.eulerAngles.z);
        public void FaceDir3D(Vector3 dir, float t = -1)
            => LookAt(pos + dir, t);
        public void FaceDir2D(Vector3 dir, float t = -1)
            => LookAt2D(pos + dir, t);
        public void FaceVel3D(bool apparentVel, float t = -1)
            => LookAt(pos + (apparentVel ? apprVel : vel), t);
        public void FaceVel2D(bool apparentVel, float t = -1)
            => LookAt2D(pos + (apparentVel ? apprVel : vel), t);
        public void AlignY(float t = -1)
            => rot = Quaternion.Slerp(rot, Quaternion.Euler(0, rot.eulerAngles.y, 0), tCheck(t));
        public void Orbit(Target target, float dist, float angleY, float angleX, float t_v = -1, float t_h = -1) {
            var targ = (oTarget = target) +
                Matrix4x4.Rotate(Quaternion.Euler(oAngleX = angleX, oAngleY = angleY, 0))
                .MultiplyPoint3x4(Vector3.back * dist);
            pos.x = Mathf.Lerp(pos.x, targ.x, tCheck(t_h));
            pos.z = Mathf.Lerp(pos.z, targ.z, tCheck(oT_h = t_h));
            pos.y = Mathf.Lerp(pos.y, targ.y, tCheck(oT_v = t_v));
        }
        public void SetOrbit(Target target, float dist, float angleY, float angleX, float t_v = -1, float t_h = -1) {
            oTarget = target; oDist = dist; oAngleY = angleY; oAngleX = angleX; oT_v = t_v; oT_h = t_h;
        }
        public void SetOrbitRot(float angleY, float t = -1)
            => oAngleY = Quaternion.Slerp(Quaternion.Euler(0, oAngleY, 0), Quaternion.Euler(0, angleY, 0), tCheck(t)).eulerAngles.y;
        public void SetOrbitOffset(float dist, float angleX, float t = -1) {
            oDist = Mathf.Lerp(oDist, dist, tCheck(t)); oAngleX = Mathf.Lerp(oAngleX, angleX, tCheck(t));
        }
        public void Orbit()
            => Orbit(oTarget, oDist, oAngleY, oAngleX, oT_v, oT_h);

        protected Vector3 oTarget;
        public float oDist, oAngleY, oAngleX, oT_v, oT_h;
        protected float tCheck(float t) => t == -1 ? 1 : t * dt;
        Quaternion lookAt(Target target, float addDegreesX, float addDegreesY, float t)
            => Quaternion.Slerp(rot, Matrix4x4.LookAt(pos, target, Vector3.up).rotation * Quaternion.Euler(addDegreesX, addDegreesY - 180, 0), tCheck(t));

        public bool targIsRay => targ is Rayman2.Persos.YLT_RaymanModel;
        public PersoController targ;
        public Vector3 targPos;

        public static float defaultDist = 8.5f, defaultAngleX = 15, defaultOrbitSpeed = 0.135f;
        protected float xLook, orbSpd, orbVel = defaultOrbitSpeed;
    }
}