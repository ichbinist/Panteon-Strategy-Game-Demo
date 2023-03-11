using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;

///INFO
///->Usage of GridRenderer script: 
///ENDINFO
public class GridRenderer : MonoBehaviour
{
    private int gridSizeX;
    private int gridSizeY;
    private float cellSize;
    public Color CellColor;
    public Color BackgroundColor;

    private Texture2D cellTexture;
    private RenderTexture renderTexture;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        cellSize = GridManager.Instance.Grid.CellSize;
        gridSizeX = GridManager.Instance.Grid.GridSizeX;
        gridSizeY = GridManager.Instance.Grid.GridSizeY;

        cellTexture = new Texture2D(1, 1);
        cellTexture.SetPixel(0, 0, CellColor);
        cellTexture.Apply();

        renderTexture = new RenderTexture(gridSizeX, gridSizeY, 0);
        renderTexture.Create();

        spriteRenderer = GetComponent<SpriteRenderer>();
        Texture2D texture = new Texture2D(gridSizeX, gridSizeY, TextureFormat.ARGB32, false);
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, gridSizeX, gridSizeY), Vector2.one * 0.5f);
        spriteRenderer.material.mainTexture = renderTexture;

        Graphics.SetRenderTarget(renderTexture);
        GL.Clear(false, true, BackgroundColor);
    }

    private void DrawCell(Vector2 position)
    {
        Vector2 pixelPosition = position * cellSize;
        Vector2 pixelSize = Vector2.one * cellSize;

        Graphics.SetRenderTarget(renderTexture);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, renderTexture.width, 0, renderTexture.height);
        Graphics.DrawTexture(new Rect(pixelPosition, pixelSize), cellTexture);
        GL.PopMatrix();
    }

    public void DrawGrid(List<Cell> cells)
    {
        Texture2D texture = new Texture2D(Mathf.RoundToInt(cellSize * gridSizeX), Mathf.RoundToInt(cellSize * gridSizeY), TextureFormat.RGBA32, false);

        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = BackgroundColor;
        }
        texture.SetPixels(pixels);

        foreach (var cell in cells)
        {
            int x = Mathf.RoundToInt(cell.CellPosition.x * 100);
            int y = Mathf.RoundToInt(cell.CellPosition.y * 100);
            int size = Mathf.RoundToInt(cellSize)-1;
            
            for (int i = x+1; i < x + size; i++)
            {
                for (int j = y+1; j < y + size; j++)
                {
                    texture.SetPixel(i, j, CellColor);
                }
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Mathf.RoundToInt(cellSize * gridSizeX), Mathf.RoundToInt(cellSize * gridSizeY)), Vector2.one * 0.5f);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}