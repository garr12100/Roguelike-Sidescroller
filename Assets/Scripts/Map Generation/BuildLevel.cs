using UnityEngine;
using System.Collections;

public class BuildLevel : MonoBehaviour
{

    public int tileSize = 1;
    public int roomWidth = 12;
    public int roomHeight = 8;
    public int sizeX;
    public int sizeY;
    //public float xScale, yScale;
    //public int tileResolution = 1;
    //public Texture2D terrainTiles;

    public GameObject[] roomDirections;
    public GameObject player;
    public GameObject treasure;
    public GameObject endPoint;

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
                    if (dataMap.RoomAt(x, y).roomType == RoomType.Starting)
                        player.transform.position = new Vector3(x * tileSize * roomWidth + 1.5f, y * tileSize * roomHeight + roomHeight/ 2, 0);
                    else if(dataMap.RoomAt(x,y).roomType == RoomType.Ending)
                        endPoint.transform.position = new Vector3(x * tileSize * roomWidth + 1.5f, y * tileSize * roomHeight + roomHeight / 2, 0);
                    else if (dataMap.RoomAt(x, y).roomType == RoomType.Treasure)
                        treasure.transform.position = new Vector3(x * tileSize * roomWidth + 1.5f, y * tileSize * roomHeight + roomHeight / 2, 0);
                    int dirIndex = (int)dataMap.RoomAt(x, y).directions;
                    GameObject room = Instantiate(roomDirections[dirIndex], new Vector3(x * tileSize * roomWidth, y * tileSize * roomHeight, 0), Quaternion.identity) as GameObject;
                    room.transform.SetParent(this.transform);
                }
            }
        }
    }
}
