using System;

namespace Jan.Navigation
{
    [Flags]
    public enum NavmeshAreas
    {
        Walkable = 1 << 0,
        NotWalkable = 1 << 1,
        Jump = 1 << 2,
        Water = 1 << 3,
        ShoreLine = 1 << 4,
        Restaurant = 1 << 5,
        AnimalOne = 1 << 6,
        Pond = 1 << 7,
        CloseFishingArea = 1 << 8,
        AllAreas = Walkable | NotWalkable | Jump | Water | ShoreLine | Restaurant | AnimalOne | Pond | CloseFishingArea
    }
}