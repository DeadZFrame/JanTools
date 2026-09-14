using System;

namespace Jan.Navigation
{
    [Flags]
    public enum NavmeshAreas
    {
        Walkable = 1 << 0,
        NotWalkable = 1 << 1,
        Jump = 1 << 2,
        Workshop = 1 << 3,
        AllAreas = Walkable | NotWalkable | Jump | Workshop
    }
}