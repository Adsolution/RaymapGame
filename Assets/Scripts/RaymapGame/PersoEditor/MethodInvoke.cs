//================================
//  By: Adsolution
//================================

using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RaymapGame.PersoEditor {
    [System.Serializable]
    public struct MethodInvoke {
        public MethodInfo method;
        public ValueInput[] methodParams;

        public MethodInvoke(MethodInfo method) {
            this.method = method;

            var pars = new List<ValueInput>();
            var parsR = new List<object>();
            foreach (var pi in method.GetParameters()) {
                var vi = new ValueInput(pi);
                pars.Add(vi);
            }
            methodParams = pars.ToArray();
        }

        public void InvokeOn(PersoController perso) {
            var pars = new List<object>();
            foreach (var p in methodParams) {
                if (p.type == ValueInput.Type.Const)
                    pars.Add(p.constVal);
                else pars.Add(p.InvokeAndGetOn(perso));
            }
        }
    }
}