using System;
using System.Collections.Generic;
using System.Linq;
using RaymapGame.Rayman2.Persos;
using UnityEngine;

namespace RaymapGame {
    public class Timeline {
        public float length;
        public float time;
        public List<AnimEvent> events = new List<AnimEvent>();

        public Timeline(params AnimEvent[] events) {
            this.events = events.ToList();
            /*new Timeline(
                new AnimEvent(0, "Rayman", rayman.Anim.Yahoo),
                new MoveEvent(3, "Rayman", 0, 4, 0, 2),

                );*/
        }

        public abstract class Event {
            public float time;
        }

        public class AnimEvent : Event {
            public AnimEvent(float time, string perso, int anim) {
                this.time = time;
                this.perso = perso;
                this.anim = anim;
            }
            public string perso;
            public int anim;
        }

        public class MoveEvent : Event {
            public MoveEvent(float time, string perso, float x, float y, float z) {

            }
            //string perso
        }

        public class RotEvent : Event {
            public RotEvent(float time, string perso, float x, float y, float t) {

            }
        }
    }
}
