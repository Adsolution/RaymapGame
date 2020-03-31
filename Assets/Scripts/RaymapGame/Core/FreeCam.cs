//================================
//  By: Adsolution
//================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Input;
using static RaymapGame.InputEx;

namespace RaymapGame
{
    public class FreeCam : MonoBehaviour
    {
        Camera cam;
        Vector3 rot;

        void Start()
        {
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            // Look
            if (GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;

                var add = mouseDelta * 0.175f;
                rot = new Vector3(Mathf.Clamp(rot.x - add.y, -90, 90), rot.y + add.x, 0);
                transform.rotation = Quaternion.Euler(rot);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }



            // Movement
            if (lStick_s.magnitude > 0.1f)
            {
                float sprint = 1;
                if (GetKey(KeyCode.LeftShift)) sprint = 4;
                else if (GetKey(KeyCode.LeftControl)) sprint = 0.25f;

                transform.position += Matrix4x4.Rotate(Camera.main.transform.rotation).
                    MultiplyPoint3x4(lStick3D_s) * 45 * sprint * Time.deltaTime;
            }
        }
    }
}