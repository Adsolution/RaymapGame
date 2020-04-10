using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame {
    public class HealthBar : MonoBehaviour {
        public float curr, max;
        float _curr, _max;

        public void Set(float curr, float max) {
            this.curr = curr; this.max = max;
        }

        public static HealthBar rayHealth, rayAir, enemyHealth;
        void Awake() {
            switch (name) {
                case "RayHealth": rayHealth = this; break;
                case "RayAir": rayAir = this; break;
                case "EnemyHealth": enemyHealth = this; break;
            }
            empty = GetComponent<RectTransform>();
            full = transform.GetChild(0).GetComponent<RectTransform>();
        }

        RectTransform full, empty;

        void Update() {
            if (_curr > curr - 3 && _curr < curr + 3)
                _curr = curr;
            else if (_curr < curr)
                _curr += Time.deltaTime * 100;
            else if (_curr > curr)
                _curr -= Time.deltaTime * 100;

            empty.sizeDelta = new Vector2(max * 1.25f, empty.sizeDelta.y);
            full.anchorMax = new Vector2(_curr / max, 1);
        }
    }

}