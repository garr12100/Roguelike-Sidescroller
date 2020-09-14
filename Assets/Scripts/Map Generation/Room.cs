using UnityEngine;
using System.Collections;

public class Room
{
    
    public RoomType roomType;

    public bool openRight, openLeft, openUp, openDown;
    public RoomDirections directions
    {
        get
        {
            if (openUp && openRight && openLeft && openDown)
                return RoomDirections.UpRightDownLeft;
            else if (openUp && openRight && openDown)
                return RoomDirections.UpRightDown;
            else if (openRight && openDown && openLeft)
                return RoomDirections.RightDownLeft;
            else if (openDown && openLeft && openUp)
                return RoomDirections.DownLeftUp;
            else if (openLeft && openUp && openRight)
                return RoomDirections.LeftUpRight;
            else if (openUp && openDown)
                return RoomDirections.UpDown;
            else if (openUp && openRight)
                return RoomDirections.UpRight;
            else if (openUp && openLeft)
                return RoomDirections.UpLeft;
            else if (openDown && openRight)
                return RoomDirections.DownRight;
            else if (openDown && openLeft)
                return RoomDirections.DownLeft;
            else if (openRight && openLeft)
                return RoomDirections.RightLeft;
            else if (openUp)
                return RoomDirections.Up;
            else if (openDown)
                return RoomDirections.Down;
            else if (openRight)
                return RoomDirections.Right;
            else if (openLeft)
                return RoomDirections.Left;
            else
                return RoomDirections.None;
        }
    }

    public Room()
    {
        this.roomType = RoomType.Default;
    }


    public Room(RoomType pt)
    {
        this.roomType = pt;
    }
}

public enum RoomDirections
{
    None = 0,
    Up,
    Right,
    Down,
    Left,
    UpRight,
    UpDown,
    UpLeft,
    DownRight,
    DownLeft,
    RightLeft,
    UpRightDown,
    RightDownLeft,
    DownLeftUp,
    LeftUpRight,
    UpRightDownLeft
}

public enum RoomType
{
    Default = 0,
    CriticalPath,
    TreasurePath,
    Ending,
    Starting,
    Treasure
}
