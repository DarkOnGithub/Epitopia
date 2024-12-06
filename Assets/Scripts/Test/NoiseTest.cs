using System;
using UnityEngine;
using World.WorldGeneration.Noise;
using World.WorldGeneration.WorldDataParser;

public class NoiseTest : MonoBehaviour
{
    [Header("Noise Generation Settings")]
    public int width = 256;
    public int height = 256;
    public float scale = 1f;
    public float offsetX = 0f;
    public float offsetY = 0f;

    [Header("Dragging Settings")]
    public float dragSpeed = 2f;
    private Vector3 lastMousePosition;
    private Vector3 _position = new();
    private Texture2D noiseTexture;
    private SpriteRenderer spriteRenderer;
    private FastNoise _noise;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        var source = new FastNoise("OpenSimplex2");
        var fractal = new FastNoise("Fractal FBm");
        fractal.Set("Source", source);

        var pos = new FastNoise("Position Output");
        pos.Set("Multiplier Y", 3f);
        _noise = new FastNoise("Add");
        _noise.Set("LHS", fractal);
        _noise.Set("RHS", pos);

        WorldDataParser.ParseData<NoiseGenerator>(Resources.Load<TextAsset>("WorldGeneration/Noises/Erosion").text);
    }

    public void GeneratePerlinNoiseTexture(float[,] customNoiseArray)
    {

        noiseTexture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var normalizedValue = (customNoiseArray[x, y] + 1)/ 2f;
                pixels[x + y * width] = new Color(normalizedValue, normalizedValue, normalizedValue);
            }
        }
        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();
        Sprite noiseSprite = Sprite.Create(noiseTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite = noiseSprite;
    }

    void Update()
    {
        HandleDragging();
    }

    void HandleDragging()
    {
        // Left mouse button dragging
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            
            // Move camera or noise texture based on drag
            //transform.position -= mainCamera.ScreenToWorldPoint(delta) - mainCamera.ScreenToWorldPoint(Vector3.zero);
            _position += delta * dragSpeed;
            Debug.Log(_position);
            float[] noiseArray = new float[width * height];
            _noise.GenUniformGrid2D(noiseArray, (int)(_position.x), (int)(_position.y), width, height, scale, 1);
            float[,] noise2DArray = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noise2DArray[x, y] = noiseArray[x + y * width];
                }
            }
            GeneratePerlinNoiseTexture(noise2DArray);
            lastMousePosition = Input.mousePosition;
        }
    }

  
}