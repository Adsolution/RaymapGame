//================================
//  By: Adsolution
//================================

using UnityEngine;
using static UnityEngine.Input;

namespace RaymapGame {
    [ExecuteAlways]
    public class InputEx : MonoBehaviour {
        public string
            jump = "A",
            shoot = "Space",
            strafe = "LControl";

        static InputEx inst;
        public static float deadZone = 0.15f;
        public static float smoothing = 0.05f;

        public static Vector2 lStick => new Vector2(GetAxisRaw("Horizontal"), GetAxisRaw("Vertical"));
        public static Vector2 rStick => new Vector2(GetAxisRaw("RHorizontal"), GetAxisRaw("RVertical"));
        public static bool lStickPress => lStick.magnitude > deadZone;
        public static bool rStickPress => rStick.magnitude > deadZone;
        public static float rStickAngle => Mathf.Atan2(rStick.x, rStick.y) * Mathf.Rad2Deg;
        public static Vector2 lStick_s => _lStick_s;
        static Vector2 _lStick_s;
        public static Vector2 rStick_s => _rStick_s;
        static Vector2 _rStick_s;
        public static Vector3 lStick3D => new Vector3(GetAxisRaw("Horizontal"), 0, GetAxisRaw("Vertical"));
        public static Vector3 rStick3D => new Vector3(GetAxisRaw("RHorizontal"), 0, GetAxisRaw("RVertical"));
        public static Vector3 lStick3D_s => new Vector3(lStick_s.x, 0, lStick_s.y);
        public static Vector3 rStick3D_s => new Vector3(rStick_s.x, 0, rStick_s.y);

        public static float lStickAngle => Mathf.Atan2(-lStick.x, lStick.y) * Mathf.Rad2Deg;
        public static float lStickAngleCam
            => Camera.main.transform.rotation.eulerAngles.y
            + Mathf.Atan2(-lStick_s.x, -lStick_s.y) * Mathf.Rad2Deg;
        public static Vector3 lStickCam_s
            => Matrix4x4.Rotate(Camera.main.transform.rotation).MultiplyVector(lStick3D_s);


        public static Vector2 mouseDelta;
        static Vector2 mousePosPrev;

        public static bool
            iJumpDown, iJumpHold, iJumpUp,
            iShootDown, iShootHold, iShootUp,
            iStrafeDown, iStrafeHold, iStrafeUp;


        void Update() {
            transform.hideFlags = HideFlags.HideInInspector;
            GetComponent<TimerHandler>().hideFlags = HideFlags.HideInInspector;
            GetComponent<MusicHandler>().hideFlags = HideFlags.HideInInspector;
            if (inst != this)
                inst = this;

            float sm = smoothing == 0 ? 1 : (1f / smoothing) * Time.deltaTime;
            _lStick_s = Vector3.ClampMagnitude(Vector2.Lerp(_lStick_s, lStick, sm), 1);
            _rStick_s = Vector3.ClampMagnitude(Vector2.Lerp(_rStick_s, rStick, sm), 1);
            mouseDelta = (Vector2)mousePosition - mousePosPrev;
            mousePosPrev = mousePosition;

            // Input mapping
            iJumpDown = GetKeyDown(inst.jump.ToLower()) || GetKeyDown(KeyCode.JoystickButton0);
            iJumpHold = GetKey(inst.jump.ToLower()) || GetKey(KeyCode.JoystickButton0);
            iJumpUp = GetKeyUp(inst.jump.ToLower()) || GetKeyUp(KeyCode.JoystickButton0);

            iShootDown = GetKeyDown(inst.shoot.ToLower()) || GetKeyDown(KeyCode.JoystickButton2);
            iShootHold = GetKey(inst.shoot.ToLower()) || GetKey(KeyCode.JoystickButton2);
            iShootUp = GetKeyUp(inst.shoot.ToLower()) || GetKeyUp(KeyCode.JoystickButton2);

            iStrafeDown = GetKeyDown(inst.strafe.ToLower()) || GetKeyDown(KeyCode.JoystickButton4);
            iStrafeHold = GetKey(inst.strafe.ToLower()) || GetKey(KeyCode.JoystickButton4);
            iStrafeUp = GetKeyUp(inst.strafe.ToLower()) || GetKeyUp(KeyCode.JoystickButton4);
        }
    }
}