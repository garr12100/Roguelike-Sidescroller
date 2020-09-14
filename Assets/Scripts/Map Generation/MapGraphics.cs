using UnityEngine;
using System.Collections;

public class MapGraphics : MonoBehaviour
{
    public int tileSize = 1;
    public int sizeX;
    public int sizeY;
    //public float xScale, yScale;
    //public int tileResolution = 1;
    //public Texture2D terrainTiles;
    public GameObject roomPrefab;
    [SerializeField]
    public TileColors tileColors;
    public Sprite[] directionSprites;

    [System.Serializable]
    public class TileColors
    {
        public Color none = Color.black;
        public Color critical = Color.green;
        public Color other = Color.grey;
        public Color treasure = Color.yellow;
        public Color ending = Color.blue;
        public Color starting = Color.white;
        public Color treasurePath = Color.yellow;
    }
    // Use this for initialization
    void Start()
    {
        //BuildMesh();
        BuildMap();

	}

    public void BuildMap()
    {
        DataMap dataMap = new DataMap(sizeX, sizeY);
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (dataMap.RoomAt(x, y) != null)
                {
                    GameObject room = Instantiate(roomPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    room.transform.SetParent(this.transform);
                    SpriteRenderer roomSprite = room.GetComponent<SpriteRenderer>();
                    int dirIndex = (int)dataMap.RoomAt(x, y).directions;
                    roomSprite.sprite = directionSprites[dirIndex];
                    Color col = tileColors.other;
                    switch (dataMap.RoomAt(x, y).roomType)
                    {
                        case RoomType.CriticalPath:
                            col = tileColors.critical;
                            break;
                        case RoomType.TreasurePath:
                            col = tileColors.treasurePath;
                            break;
                        case RoomType.Ending:
                            col = tileColors.ending;
                            break;
                        case RoomType.Starting:
                            col = tileColors.starting;
                            break;
                        case RoomType.Treasure:
                            col = tileColors.treasure;
                            break;
                    }
                    roomSprite.color = col;
                }
            }
        }
    }

    //public void BuildMesh()
    //{
    //    int numTiles = sizeX * sizeY;
    //    int numTris = numTiles * 2;
    //    int vsizeX = sizeX + 1;
    //    int vsizeY = sizeY + 1;
    //    int numVerts = vsizeX * vsizeY;

    //    //Generate the mesh data
    //    Vector3[] vertices = new Vector3[numVerts];
    //    Vector3[] normals = new Vector3[numVerts];
    //    Vector2[] uv = new Vector2[numVerts];

    //    int[] triangles = new int[numTris * 3];

    //    int x, y;
    //    for (y = 0; y < vsizeY; y++)
    //    {
    //        for (x = 0; x < vsizeX; x++)
    //        {
    //            vertices[y * vsizeX + x] = new Vector3(x * tileSize, -y * tileSize, 0);
    //            normals[y * vsizeX + x] = Vector3.forward;
    //            uv[y * vsizeX + x] = new Vector2((float)x / sizeX, (float)y / sizeY);
    //        }
    //    }
    //    for (y = 0; y < sizeY; y++)
    //    {
    //        for (x = 0; x < sizeX; x++)
    //        {
    //            int squareIndex = y * sizeX + x;
    //            int triOffset = squareIndex * 6;
    //            triangles[triOffset + 0] = y * vsizeX + x + 0;
    //            triangles[triOffset + 2] = y * vsizeX + x + vsizeX + 0;
    //            triangles[triOffset + 1] = y * vsizeX + x + vsizeX + 1;

    //            triangles[triOffset + 3] = y * vsizeX + x + 0;
    //            triangles[triOffset + 5] = y * vsizeX + x + vsizeX + 1;
    //            triangles[triOffset + 4] = y * vsizeX + x + 1;
    //        }
    //    }

    //    //Create new mesh based on data
    //    Mesh mesh = new Mesh();
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.uv = uv;

    //    //Assign mesh
    //    MeshFilter mf = GetComponent<MeshFilter>();
    //    MeshCollider mc = GetComponent<MeshCollider>();
    //    mf.mesh = mesh;
    //    mc.sharedMesh = mesh;
    //    BuildTexture();
    //}

    //void BuildTexture()
    //{
    //    DataMap dataMap = new DataMap(sizeX, sizeY);
    //    int texW = sizeX * tileResolution;
    //    int texH = sizeY * tileResolution;
    //    Texture2D texture = new Texture2D(texW, texH);
    //    //Color[][] tiles = ChopUpTiles();
    //    for (int x = 0; x < sizeX; x++)
    //    {
    //        for (int y = 0; y < sizeY; y++)
    //        {
    //            Color[] p;
    //            switch (dataMap.RoomAt(x, y))
    //            {
    //                case TileType.None:
    //                    p = SetColorArray(tileColors.none);
    //                    break;
    //                case TileType.Room:
    //                    p = SetColorArray(tileColors.room);
    //                    break;
    //                case TileType.Corridor:
    //                    p = SetColorArray(tileColors.corridor);
    //                    break;
    //                case TileType.Border:
    //                    p = SetColorArray(tileColors.border);
    //                    break;
    //                default:
    //                    p = SetColorArray(Color.magenta);
    //                    break;
    //            }
    //            texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
    //        }
    //    }

    //    texture.filterMode = FilterMode.Point;
    //    texture.wrapMode = TextureWrapMode.Clamp;
    //    texture.Apply();
    //    MeshRenderer mr = GetComponent<MeshRenderer>();
    //    mr.sharedMaterials[0].mainTexture = texture;
    //}

    //public Color[] SetColorArray(Color col)
    //{
    //    Color[] colors = new Color[tileResolution * tileResolution];
    //    for (int i = 0; i < colors.Length; i++)
    //    {
    //        colors[i] = col;
    //    }
    //    return colors;
    //}

    //Color[][] ChopUpTiles()
    //{
    //    int numTilesPerRow = terrainTiles.width / tileResolution;
    //    int numRows = terrainTiles.height / tileResolution;
    //    Color[][] tiles = new Color[numTilesPerRow * numRows][];
    //    for (int y = 0; y < numRows; y++)
    //    {
    //        for (int x = 0; x < numTilesPerRow; x++)
    //        {
    //            tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
    //        }
    //    }
    //    return tiles;
    //}

}
