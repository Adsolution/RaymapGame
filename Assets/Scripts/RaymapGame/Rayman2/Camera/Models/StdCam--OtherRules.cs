using UnityEngine;
using static UnityEngine.Input;
using static RaymapGame.InputEx;

namespace RaymapGame.Rayman2.Persos {
    public partial class StdCam {
        protected void Rule_AxeForce(Vector3 focus, float tRotY) {
            col.wallEnabled = false;
            var focXZ = new Vector3(focus.x, 0, focus.z);
            var targXZ = new Vector3(targ.pos.x, 0, targ.pos.z);

            var vec = targXZ - focXZ;
            SetOrbit(new Vector3(focus.x, targ.pos.y, focus.z), vec.magnitude + 10, -Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg - 90, 10, 3, 4);
            LookAtY(targ.pos, 0, tRotY);
            LookAtX(targ.pos, 5, 2);
            Orbit();
        }

        float dist_s;
        protected void Rule_Oriente(float yRot, float xRot, float dist, float xAddDeg) {
            if (newRule) dist_s = oDist;
            dist_s = Mathf.Lerp(dist_s, dist, dt * 2);

            col.wallEnabled = false;
            Orbit(targ.center, dist_s, Quaternion.Lerp(
                    Quaternion.Euler(0, oAngleY, 0), Quaternion.Euler(0, yRot, 0),
                    dt * 1f).eulerAngles.y, xRot + 5, 1, 6);

            LookAtX(targ.center, 0, 6);
            LookAtY(targ.center, 0, 10);
        }

        protected void Rule_PosCam(Vector3 pos, float t) {
            if (newRule && t == -1) {
                LookAtX(targ.center, 0);
                LookAtY(targ.center, 0);
            }
            col.wallEnabled = false;
            this.pos = Vector3.Lerp(this.pos, pos + Vector3.up, tCheck(t));
            SetOrbitRot(rot.eulerAngles.y + 180);
            LookAtX(targ.center, 0, 10);
            LookAtY(targ.center, 0, 10);
        }


        Vector3 freeRot;
        protected void Rule_Free() {
            col.wallEnabled = false;
            if (newRule) freeRot = rot.eulerAngles;
            SetFriction(1, 1);

            // Look
            if (GetMouseButton(1)) {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;

                var add = mouseDelta * 0.175f;
                freeRot = new Vector3(Mathf.Clamp(freeRot.x - add.y, -90, 90), freeRot.y + add.x, 0);
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }


            // Movement
            if (lStick_s.magnitude > deadZone) { 
                float sprint = 1;
                if (GetKey(KeyCode.LeftShift)) sprint = 4;
                else if (GetKey(KeyCode.LeftControl)) sprint = 0.25f;

                moveSpeed = 45 * sprint * dt;

                NavDirectionCam(lStick3D_s);
            }

            rot.eulerAngles = freeRot;
        }
    }
}