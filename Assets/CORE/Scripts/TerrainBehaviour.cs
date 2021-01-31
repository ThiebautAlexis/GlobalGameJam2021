// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    public class TerrainBehaviour : Trigger
    {
        [SerializeField, Required] private new Renderer renderer;
        private Texture2D cloneTexture;
        

        float widthWorld, heightWorld;
        int widthPixel, heightPixel;

        public float WidthWorld
        {
            get
            {
                if (widthWorld == 0)
                    widthWorld = renderer.bounds.size.x;
                return widthWorld;
            }

        }
        public float HeightWorld
        {
            get
            {
                if (heightWorld == 0)
                    heightWorld = renderer.bounds.size.y;
                return heightWorld;
            }

        }
        public int WidthPixel
        {
            get
            {
                if (widthPixel == 0)
                    widthPixel = renderer.material.mainTexture.width;

                return widthPixel;
            }
        }
        public int HeightPixel
        {
            get
            {
                if (heightPixel == 0)
                    heightPixel = renderer.material.mainTexture.height;

                return heightPixel;
            }
        }

        void MakeAHole(Collider2D col)
        {
            Vector2 _c = WorldToPixel(col.bounds.center);
            int _r = Mathf.RoundToInt(col.bounds.extents.x * WidthPixel / WidthWorld);

            int _px, _nx, _py, _ny, _d;
            for (int i = 0; i <= _r; i++)
            {
                _d = Mathf.RoundToInt(Mathf.Sqrt(_r * _r - i * i));
                for (int j = 0; j <= _d; j++)
                {
                    _px = (int)_c.x + i;
                    _nx = (int)_c.x - i;
                    _py = (int)_c.y + j;
                    _ny = (int)_c.y - j;

                    cloneTexture.SetPixel(_px, _py, Color.clear);
                    cloneTexture.SetPixel(_nx, _py, Color.clear);
                    cloneTexture.SetPixel(_px, _ny, Color.clear);
                    cloneTexture.SetPixel(_nx, _ny, Color.clear);
                }
            }

            cloneTexture.Apply(false);
        }

        Vector2 WorldToPixel(Vector2 pos)
        {
            pos = Quaternion.Inverse(transform.rotation) * pos;
            Vector2 v = Vector2.zero;

            float _dx = (pos.x - transform.position.x);
            float _dy = (pos.y - transform.position.y);

            v.x = Mathf.RoundToInt(0.5f * WidthPixel + _dx * (WidthPixel / WidthWorld));
            v.y = Mathf.RoundToInt(0.5f * HeightPixel + _dy * (HeightPixel / HeightWorld));

            return v;
        }

        public override bool OnEnter(Digger _digger) => false;

        public override void OnUpdate(Digger _digger) => MakeAHole(_digger.Collider);

        void Start()
        {
            cloneTexture = Instantiate(renderer.material.mainTexture) as Texture2D;
            cloneTexture.alphaIsTransparency = true;
            renderer.material.mainTexture = cloneTexture;
        }
    }
}
