using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Texture2D texture;
    public GameObject pixelPrefab;

    public float iterationsPerSecond;
    public float positionMult = 1f;

    private int _width, _height;
    private Color[] _colors;

    private int whites;

    void Start()
    {
        _width = texture.width; _height = texture.height;
        
        _colors = texture.GetPixels();

        for(int i = 0; i < _colors.Length; i++)
        {
            Color color = _colors[i];

            if(color == Color.white)
            {
                whites++;
                Instantiate(pixelPrefab, (Vector3)GetPixelPosition(_width, _height, i), Quaternion.identity);
            }
        }
    }

    Vector2 GetPixelPosition(int width, int height, int arrayPos)
    {
        Vector2 pixelPos = new Vector2((arrayPos % width) * positionMult, Mathf.FloorToInt(arrayPos / width) * positionMult);

        return pixelPos;
    }
}
