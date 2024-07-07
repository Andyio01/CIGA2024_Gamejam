using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2EqualityComparer : IEqualityComparer<Vector2>
{
    private HashSet<Vector2> visitedPoints = new HashSet<Vector2>(new Vector2EqualityComparer());
    public float tolerance = 0.01f; // Adjust the tolerance as needed


    public Vector2EqualityComparer(float tolerance = 0.01f)
    {
        this.tolerance = tolerance;
    }

    public bool Equals(Vector2 v1, Vector2 v2)
    {
        return Vector2.Distance(v1, v2) < tolerance;
    }

    public int GetHashCode(Vector2 v)
    {
        return v.GetHashCode();
    }
}
