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

        public readonly Vector3 pos;
        public float x => pos.x;
        public float y => pos.y;
        public float z => pos.z;

        public static implicit operator Vector3(Target v) => v == null ? new Vector3() : v.pos;

        public static implicit operator Target(Vector3 v) => new Target(v);
        public static implicit operator Target(CollideInfo v) => new Target(v.hit.point);
        public static implicit operator Target(PersoController v) => v == null ? null : new Target(v.pos);
        public static implicit operator Target(Channel v) => v == null ? null : new Target(v.pos);
    }
}
