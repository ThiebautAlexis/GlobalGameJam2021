using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TerrainBehaviour : MonoBehaviour
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
        Vector2Int _c = WorldToPixel(col.bounds.center);
        int _r = Mathf.RoundToInt(col.bounds.size.x * WidthPixel / WidthWorld);

        int _px, _nx, _py, _ny, _d;
        for (int i = 0; i <= _r; i++)
        {
            _d = Mathf.RoundToInt(Mathf.Sqrt(_r * _r - i * i));
            for (int j = 0; j <= _d; j++)
            {
                _px = _c.x + i;
                _nx = _c.x - i;
                _py = _c.y + j;
                _ny = _c.y - j;

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

    Vector2Int WorldToPixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;

        float _dx = (pos.x - transform.position.x);
        float _dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * WidthPixel + _dx * (WidthPixel / WidthWorld));
        v.y = Mathf.RoundToInt(0.5f * HeightPixel + _dy * (HeightPixel / HeightWorld));

        return v;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Piercer"))
            return;
        if (!collision.GetComponent<CircleCollider2D>())
            return;
        MakeAHole(collision.GetComponent<CircleCollider2D>());
    }

    void Start()
    {
        Init();

    }
}