//================================
//  By: Adsolution
//================================
using UnityEngine;

namespace RaymapGame {
    /// <summary>
    /// Container that retrieves a position from various input types.
    /// </summary>
    public class Target {
        public Target(Vector3 pos)
            => this.pos = pos;

        PersoController perso;
        public Vector3 pos;
        public float x => pos.x;
        public float y => pos.y;
        public float z => pos.z;

        public static implicit operator Vector3(Target v) => v.pos;

        public static implicit operator Target(PersoController v) => v == null ? null : new Target(v.pos);
        public static implicit operator Target(Vector3 v) => new Target(v);
    }
}
