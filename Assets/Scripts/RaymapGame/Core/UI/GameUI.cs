using UnityEngine;
using System.Collections;

namespace RaymapGame {
    public class GameUI : MonoBehaviour {
        public HealthBar rayHealth, rayAir, enemyHealth;
        CanvasGroup can;
        void Awake() {
            Main.ui = this;
            can = GetComponent<CanvasGroup>();
        }

        public void FadeIn() {
            StopAllCoroutines(); StartCoroutine(cr_FadeIn());
        }
        public void FadeOut() {
            StopAllCoroutines(); StartCoroutine(cr_FadeOut());
        }

        bool fadeIn;
        public void ToggleFade() {
            StopAllCoroutines(); if (fadeIn = !fadeIn) FadeIn(); else FadeOut();
        }


        Timer t_fadeOut = new Timer();
        IEnumerator cr_FadeIn() {
            for (float a = can.alpha; a < 1; a += Time.deltaTime * 5) {
                can.alpha = a;
                yield return new WaitForEndOfFrame();
            }
            can.alpha = 1;
            t_fadeOut.Start(4, FadeOut);
            yield return null;
        }

        IEnumerator cr_FadeOut() {
            t_fadeOut.Abort();
            for (float a = can.alpha; a > 0; a -= Time.deltaTime) {
                can.alpha = a;
                yield return new WaitForEndOfFrame();
            }
            can.alpha = 0;
            yield return null;
        }



        float healthPrev;
        void Update() {
            if (Main.mainActor != null) {
                if (healthPrev != Main.mainActor.hitPoints)
                    FadeIn();
                healthPrev = Main.mainActor.hitPoints;
                rayHealth.Set(Main.mainActor.hitPoints, Main.mainActor.maxHitPoints);
            }
        }
    }
}
