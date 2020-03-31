//================================
//  By: Adsolution
//================================
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RaymapGame.PersoEditor {
    [System.Serializable]
    public struct ValueInput {
        public enum Type { Const, Func }

        public ParameterInfo pi;
        public ValueInput(ParameterInfo pi) {
            this.pi = pi;
            type = Type.Const;
            constVal = 0;
            _func = null;
            funcParams = null;
        }
        public System.Type valueType => pi.ParameterType;
        public Type type;
        public object constVal;

        MethodInfo _func;
        public MethodInfo func { get => _func; set {
                _func = value;
                var fp = value.GetParameters();
                funcParams = new ValueInput[fp.Length];
            } }
        public ValueInput[] funcParams;

        public object[] GetParamValuesOn(object perso) {
            var r = new List<object>();
            foreach (var v in funcParams)
                r.Add(v.InvokeAndGetOn(perso));
            return r.ToArray();
        }

        public object InvokeAndGetOn(object perso) {
            return func.Invoke(perso, GetParamValuesOn(perso));
        }
    }
}