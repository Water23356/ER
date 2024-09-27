using UnityEngine;
using UnityEngine.Tilemaps;

public class FillTilemap : MonoBehaviour
{ 
    public Tilemap tilemap; // 需要填充的 Tilemap
    public TileBase tile; // 你要填充的 Tile 资源
    public Vector3Int startPosition; // 填充起始位置
    public int width = 5; // 填充的宽度
    public int height = 5; // 填充的高度

    void Start()
    { 
        FillTiles();
    }

    void FillTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + x, startPosition.y + y, startPosition.z);
                tilemap.SetTile(position, tile); // 在指定位置设置 Tile
            }
        }
    }


}