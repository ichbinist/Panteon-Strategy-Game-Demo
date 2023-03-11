using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of UnitConversions script: Holds the unit conversions for the grid system and A* pathfinding algorithm.
///ENDINFO

public class UnitConversions
{
    public static Vector2 UnityPositionToPixel(Vector2 unityPosition)
    {
        return unityPosition / 100f;
    }

    public static float UnityLengthToPixel(float length)
    {
        return length / 100f;
    }
}
