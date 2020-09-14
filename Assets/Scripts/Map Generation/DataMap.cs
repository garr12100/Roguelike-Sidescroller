using UnityEngine;
using System.Collections.Generic;

public class DataMap
{
    int sizeX, sizeY;
    Room[,] map;
    List<Room> criticalPath = new List<Room>();
    List<Room> treasurePath = new List<Room>();

    public DataMap(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        map = new Room[sizeX, sizeY];
        //Create Critical Path:
        int yStart = Random.Range(0, sizeY); //pick random y position       
        while (criticalPath.Count == sizeX * sizeY || criticalPath.Count == 0)
        {
            ClearMap();
            map[0, yStart] = new Room(RoomType.Starting);
            criticalPath.Clear();
            MakeCriticalPathRecursive(0, yStart);
        }
        Vector2 treasurePos = PickTreasureRoom();
        map[(int)treasurePos.x, (int)treasurePos.y] = new Room(RoomType.Treasure);
        treasurePath.Clear();
        MakeTreasurePathRecursive((int)treasurePos.x, (int)treasurePos.y);
        MakeExtraRooms();
        OpenAdjacentRooms();
        CheckIfRoomsConnect();
    }

    public Room RoomAt(int x, int y)
    {
        if(x < 0 || y < 0 || x >= sizeX || y >= sizeY)
            return null;
        return map[x, y];
    }

    public bool InBounds(int x, int y)
    {
        if (x < 0 || y < 0 || x >= sizeX || y >= sizeY)
            return false;
        return true;
    }

    void ClearMap()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                map[i, j] = null;
            }
        }
    }

    void MakeCriticalPathRecursive(int xPos, int yPos)
    {
        //Create a room
        //if (map[xPos, yPos] == null)
        //{
        //    if (criticalPath.Count == 0)
        //        map[xPos, yPos] = new Room(RoomType.Starting);
        //    else
        //        map[xPos, yPos] = new Room(RoomType.CriticalPath);
        //}
        if(!criticalPath.Contains(map[xPos, yPos]))
            criticalPath.Add(map[xPos, yPos]);
        //Choose random direction
        int directionInt = Random.Range(0, 3);
        switch (directionInt)
        {
            case 0: //Up
                if (InBounds(xPos, yPos +1) && map[xPos, yPos + 1] == null)
                {
                    map[xPos, yPos].openUp = true;
                    map[xPos, yPos + 1] = new Room(RoomType.CriticalPath);
                    map[xPos, yPos + 1].openDown = true;
                    MakeCriticalPathRecursive(xPos, yPos + 1);
                }
                else
                    MakeCriticalPathRecursive(xPos, yPos);
                break;
            case 1: //Right
                if (InBounds(xPos + 1, yPos))
                {
                    map[xPos, yPos].openRight = true;
                    map[xPos + 1, yPos] = new Room(RoomType.CriticalPath);
                    map[xPos + 1, yPos].openLeft = true;
                    MakeCriticalPathRecursive(xPos + 1, yPos);
                }
                else
                {
                    map[xPos, yPos].roomType = RoomType.Ending;
                    return;
                }
                break;
            case 2: //Down
                if (InBounds(xPos, yPos -1) && map[xPos, yPos - 1] == null)
                {
                    map[xPos, yPos].openDown = true;
                    map[xPos, yPos - 1] = new Room(RoomType.CriticalPath);
                    map[xPos, yPos - 1].openUp = true;
                    MakeCriticalPathRecursive(xPos, yPos - 1);
                }
                else
                    MakeCriticalPathRecursive(xPos, yPos);
                break;
        }
        return;
        
    }

    Vector2 PickTreasureRoom()
    {
        int minY = sizeY < 4 ? 0 : sizeY / 2 - sizeY / 4;
        int maxY = sizeY < 4 ? sizeY : sizeY / 2 + sizeY / 4;
        int x = Random.Range(0, sizeX);
        int y = Random.Range(minY, maxY);
        while (map[x, y] != null)
        {
            x = Random.Range(0, sizeX);
            y = Random.Range(minY, maxY);
        }
        return new Vector2(x, y);
    }

    void MakeTreasurePathRecursive(int xPos, int yPos)
    {
        if(!treasurePath.Contains(map[xPos, yPos]))
            treasurePath.Add(map[xPos, yPos]);
        int directionInt = Random.Range(0, 3);
        switch (directionInt)
        {
            case 0: //Up
                if (InBounds(xPos, yPos + 1) )
                {
                    if (map[xPos, yPos + 1] == null)
                    {
                        map[xPos, yPos].openUp = true;
                        map[xPos, yPos + 1] = new Room(RoomType.TreasurePath);
                        map[xPos, yPos + 1].openDown = true;
                        MakeTreasurePathRecursive(xPos, yPos + 1);
                    }
                    else if (map[xPos, yPos + 1].roomType == RoomType.TreasurePath || map[xPos, yPos + 1].roomType == RoomType.Treasure)
                    {
                        MakeTreasurePathRecursive(xPos, yPos + 1);
                    }
                    else if (map[xPos, yPos + 1].roomType != RoomType.Ending)
                    {
                        map[xPos, yPos].openUp = true;
                        map[xPos, yPos + 1].openDown = true;
                        return;
                    }
                    else
                        MakeTreasurePathRecursive(xPos, yPos);
                }
                else
                    MakeTreasurePathRecursive(xPos, yPos);
                break;
            case 1: //Left
                if (InBounds(xPos - 1, yPos))
                {
                    if (map[xPos - 1, yPos] == null)
                    {
                        map[xPos, yPos].openLeft = true;
                        map[xPos - 1, yPos] = new Room(RoomType.TreasurePath);
                        map[xPos - 1, yPos].openRight = true;
                        MakeTreasurePathRecursive(xPos - 1, yPos);
                    }
                    else if (map[xPos - 1, yPos].roomType == RoomType.TreasurePath || map[xPos - 1, yPos].roomType == RoomType.Treasure)
                    {
                        MakeTreasurePathRecursive(xPos - 1, yPos);
                    }
                    else
                    {
                        map[xPos, yPos].openLeft = true;
                        map[xPos - 1, yPos].openRight = true;
                        return;
                    }
                }
                else
                {
                    MakeTreasurePathRecursive(xPos, yPos);
                }
                break;
            case 2: //Down
                if (InBounds(xPos, yPos - 1))
                {
                    if (map[xPos, yPos - 1] == null)
                    {
                        map[xPos, yPos].openDown = true;
                        map[xPos, yPos - 1] = new Room(RoomType.TreasurePath);
                        map[xPos, yPos - 1].openUp = true;
                        MakeTreasurePathRecursive(xPos, yPos - 1);
                    }
                    else if (map[xPos, yPos - 1].roomType == RoomType.TreasurePath || map[xPos, yPos - 1].roomType == RoomType.Treasure)
                    {
                        MakeTreasurePathRecursive(xPos, yPos - 1);
                    }
                    else if (map[xPos, yPos - 1].roomType != RoomType.Ending)
                    {
                        map[xPos, yPos].openDown = true;
                        map[xPos, yPos - 1].openUp = true;
                        return;
                    }
                    else
                        MakeTreasurePathRecursive(xPos, yPos);
                }
                else
                    MakeTreasurePathRecursive(xPos, yPos);
                break;
        }
        return;
    }

    void MakeExtraRooms()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (map[x, y] == null || map[x, y].roomType == RoomType.Ending)
                    continue;
                PlaceAdjacentRooms(x, y, 35);
            }
        }
    }

    void TryPlaceRoom(int x, int y, int chance)
    {
        int roll = Random.Range(0, 100);
        if (!InBounds(x, y) || map[x, y] != null || roll > chance)
            return;
        //Otherwise, place a room
        map[x, y] = new Room(RoomType.Default);
        PlaceAdjacentRooms(x, y, chance / 2);
    }

    void PlaceAdjacentRooms(int x, int y, int chance)
    {
        //Try Up
        TryPlaceRoom(x, y + 1, chance);
        //Try Right
        TryPlaceRoom(x + 1, y, chance);
        //Try Down
        TryPlaceRoom(x, y - 1, chance);
        //Try Left
        TryPlaceRoom(x - 1, y, chance);
    }

    void OpenAdjacentRooms()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                //Try Up
                TryConnectRooms(x, y, x, y + 1, 10);
                //Try Right
                TryConnectRooms(x, y, x + 1, y, 10);
                //Try Down
                TryConnectRooms(x, y, x, y - 1, 10);
                //Try Left
                TryConnectRooms(x, y, x - 1, y, 10);
            }
        }
    }

    void TryConnectRooms(int x1, int y1, int x2, int y2, int chance)
    {
        int roll = Random.Range(0, 100);

        //Return if empty/out of bounds
        if (!InBounds(x1, y1) || map[x1, y1] == null || !InBounds(x2, y2) || map[x2, y2] == null)
            return;

        //Don't connect starting and ending rooms
        if ((map[x1, y1].roomType == RoomType.Starting && map[x2, y2].roomType == RoomType.Ending) ||
            (map[x1, y1].roomType == RoomType.Ending && map[x2, y2].roomType == RoomType.Starting))
            return;

        if ((!map[x1, y1].openUp && !map[x1, y1].openDown && !map[x1, y1].openLeft && !map[x1, y1].openRight)
            || (!map[x2, y2].openUp && !map[x2, y2].openDown && !map[x2, y2].openLeft && !map[x2, y2].openRight))
        {
            chance = 100; //A room is connected to nothing, connect it to something
        }
        if (roll > chance)
            return;
        //Otherwise, join rooms
        if (x1 < x2) //Room 1 to the left of room 2
        {
            map[x1, y1].openRight = true;
            map[x2, y2].openLeft = true;
        }
        else if (x1 > x2) //Room 1 to the right of room 2
        {
            map[x1, y1].openLeft = true;
            map[x2, y2].openRight = true;
        }
        else if (y1 < y2) //Room 1 below room 2
        {
            map[x1, y1].openUp = true;
            map[x2, y2].openDown = true;
        }
        else if (y1 > y2) //Room 1 above room 2
        {
            map[x1, y1].openDown = true;
            map[x2, y2].openUp = true;
        }
    }

    void CheckIfRoomsConnect()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (!IsRoomConnected(x, y))
                    DestroyRoom(x, y);
            }
        }
    }

    bool IsRoomConnected(int x, int y)
    {
        if (!InBounds(x, y) || map[x, y] == null)
            return false;
        if (map[x, y].roomType != RoomType.Default)
            return true;
        if (map[x, y].openUp)
        {
            if (IsRoomConnected(x, y + 1))
                return true;
        }
        if (map[x, y].openRight)
        {
            if (IsRoomConnected(x + 1, y))
                return true;
        }

        return false;
    }

    void DestroyRoom(int x, int y)
    {
        map[x, y] = null;
        if (InBounds(x, y + 1) && map[x, y + 1] != null)
            map[x, y + 1].openDown = false;
        if (InBounds(x + 1, y) && map[x + 1, y] != null)
            map[x + 1, y].openLeft = false;
        if (InBounds(x, y - 1) && map[x, y - 1] != null)
            map[x, y - 1].openUp = false;
        if (InBounds(x - 1, y) && map[x - 1, y] != null)
            map[x - 1, y].openRight = false;
    }

}
