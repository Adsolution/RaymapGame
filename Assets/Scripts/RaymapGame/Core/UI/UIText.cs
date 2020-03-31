using UnityEngine;
using System.Collections.Generic;

namespace RaymapGame {
    public class UIText : MonoBehaviour {
        public Color bottom, top;
        public string text => _text;
        string _text;
        public void SetText(string text) {
            _text = text;

            var verts = new List<Vector3>();
            var uvs = new List<Vector2>();
            var cols = new List<Color>();
            var inds = new List<int[]>();

            for (int c = 0; c < text.Length; c++) {
                verts.AddRange(new Vector3[] {
                    new Vector3(c + 0, 0),
                    new Vector3(c + 0, 1),
                    new Vector3(c + 1, 1),
                    new Vector3(c + 1, 0),
                });
                uvs.AddRange(new Vector2[] {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                });
                cols.AddRange(new Color[] {
                    bottom, top, top, bottom,
                });
                inds.Add(new int[] {
                    c * 4 + 0,
                    c * 4 + 1,
                    c * 4 + 2,
                    c * 4 + 3,
                });
            }
            mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetUVs(0, uvs.ToArray());
            mesh.SetColors(cols);
            for (int c = 0; c < text.Length; c++) {
                mesh.SetSubMesh(c, new UnityEngine.Rendering.SubMeshDescriptor(c * 4, 4, MeshTopology.Quads));
                mesh.SetIndices(inds[c], MeshTopology.Quads, c);
            }

            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshRenderer>().materials = new Material[text.Length];

            for (int c = 0; c < text.Length; c++)
                GetComponent<MeshRenderer>().materials[c] = new Material(Shader.Find("Custom/VColorTransparent")) {
                    mainTexture = ResManager.Get<Texture2D>($"textures_alphabet/m_{text[c].ToString().ToLower()}_aaa")
                };
        }

        Mesh mesh;



        void Start() {
            SetText("Da great escape");
        }

        void Update() {

        }
    }
}