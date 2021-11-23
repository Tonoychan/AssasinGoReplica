using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility 
{
    public static Vector3 Vector3Round(Vector3 InputVector)
    {
        return new Vector3(Mathf.Round(InputVector.x), Mathf.Round(InputVector.y), Mathf.Round(InputVector.z));
    }

    public static Vector2 Vector2Round(Vector2 InputVector)
    {
        return new Vector2(Mathf.Round(InputVector.x), Mathf.Round(InputVector.y));
    }
}
