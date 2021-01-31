using UnityEngine;

namespace GlobalGameJam2021
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TerrainBehaviour : Trigger
    {
        public Texture2D TerrainTexture;
        Texture2D cloneTexture;
        SpriteRenderer spriteRenderer;

        float widthWorld, heightWorld;
        int widthPixel, heightPixel;

        public float WidthWorld
        {
            get
            {
                if (widthWorld == 0)
                    widthWorld = spriteRenderer.bounds.size.x;
                return widthWorld;
            }

        }
        public float HeightWorld
        {
            get
            {
                if (heightWorld == 0)
                    heightWorld = spriteRenderer.bounds.size.y;
                return heightWorld;
            }

        }
        public int WidthPixel
        {
            get
            {
                if (widthPixel == 0)
                    widthPixel = spriteRenderer.sprite.texture.width;

                return widthPixel;
            }
        }
        public int HeightPixel
        {
            get
            {
                if (heightPixel == 0)
                    heightPixel = spriteRenderer.sprite.texture.height;

                return heightPixel;
            }
        }

        void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            cloneTexture = Instantiate(TerrainTexture);
            cloneTexture.alphaIsTransparency = true;
            UpdateTexture();
            gameObject.AddComponent<PolygonCollider2D>();
        }


        void MakeAHole(CircleCollider2D col)
        {
            Vector2 _c = WorldToPixel(col.bounds.center);
            int _r = Mathf.RoundToInt(col.bounds.size.x * WidthPixel / WidthWorld);

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
            cloneTexture.Apply();
            UpdateTexture();

            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>();
        }

        void UpdateTexture()
        {
            spriteRenderer.sprite = Sprite.Create(cloneTexture,
                                new Rect(0, 0, cloneTexture.width, cloneTexture.height),
                                new Vector2(0.5f, 0.5f),
                                50f
                                );
        }

        Vector2 WorldToPixel(Vector2 pos)
        {
            Vector2 v = Vector2.zero;

            float _dx = (pos.x - transform.position.x);
            float _dy = (pos.y - transform.position.y);

            v.x = Mathf.RoundToInt(0.5f * WidthPixel + _dx * (WidthPixel / WidthWorld));
            v.y = Mathf.RoundToInt(0.5f * HeightPixel + _dy * (HeightPixel / HeightWorld));

            return v;
        }

        public override bool OnEnter(Digger _digger)
        {
            MakeAHole(_digger.GetComponent<CircleCollider2D>());
            return false;
        }

        void Start()
        {
            Init();
        }
    }
}
