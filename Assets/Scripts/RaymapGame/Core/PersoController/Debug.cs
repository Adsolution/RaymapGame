//================================
//  By: Adsolution
//================================

using UnityEngine;

namespace RaymapGame {
    public partial class PersoController {
        GUIStyle debugStyle = new GUIStyle();
        Rect debugRect;
        float offY;

        protected void DebugNewColumn() {
            debugRect.x += debugRect.width;
            debugRect.y = 8 + offY;
        }
        protected void DebugLabel(string name, object value = null) {
            string str = $"{name}{(value == null ? "" : ":")}   {value}";
            // Shadow
            var c = debugStyle.normal.textColor;
            debugStyle.normal.textColor = Color.black;
            GUI.Label(new Rect(debugRect.x + 1, debugRect.y + 1, debugRect.width, debugRect.height), str, debugStyle);
            // Label
            debugStyle.normal.textColor = c;
            GUI.Label(debugRect, str, debugStyle);
            debugRect.y += debugRect.height;
        }

        protected void OnGUI() {
            if (!(Main.showMainActorDebug && Main.mainActor == this)) return;
            offY = 0;
            debugRect = new Rect(8, 8, 225, 25);
            debugStyle.fontSize = 18;
            debugStyle.normal.textColor = Color.red;
            var p = Main.mainActor;

            DebugLabel("Pos", p.pos);
            DebugLabel("Rot", p.rot.eulerAngles);
            DebugLabel("Scale", p.scale);
            DebugLabel("Sector", p.sector);

            DebugNewColumn();

            DebugLabel("Vel", new Vector3(p.velXZ.x, p.velY, p.velXZ.z));
            DebugLabel("Horizontal Fric", p.fricXZ);
            DebugLabel("Vertical Fric", p.fricY);
            DebugLabel("Gravity", p.gravity);

            DebugNewColumn();

            DebugLabel("Rule", p.rule);
            if (p.anim != null)
                DebugLabel("Animation", p.anim.currAnim);
            DebugLabel("Move Speed", p.moveSpeed);

            DebugNewColumn();

            DebugLabel("Ground", p.col.ground.Any);
            DebugLabel("Wall", p.col.wall.Any);
            DebugLabel("Hit Points", $"{hitPoints} / {maxHitPoints}");

            // Custom subtype debug info
            if (GetType() != typeof(PersoController)) {
                offY = 120;
                DebugNewColumn();
                debugRect.x = 8;
                DebugLabel($"{GetType().Name} info:");
                offY = 160;
                DebugNewColumn();
                debugRect.x = 8;
                OnDebug();
            }
        }
    }
}