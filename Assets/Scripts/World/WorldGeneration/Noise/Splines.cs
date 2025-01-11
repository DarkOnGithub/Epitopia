using System;
using UnityEngine;

public class Splines
{
    private readonly Vector2[] _points;

    public Splines(float[] floatingPoints)
    {
        if (floatingPoints == null)
            throw new ArgumentNullException(nameof(floatingPoints));
        if (floatingPoints.Length % 2 != 0)
            throw new ArgumentException("The number of elements in floatingPoints must be even.",
                                        nameof(floatingPoints));

        var pointCount = floatingPoints.Length / 2;
        _points = new Vector2[pointCount];
        for (var i = 0; i < floatingPoints.Length; i += 2)
            _points[i / 2] = new Vector2(floatingPoints[i], floatingPoints[i + 1]);

        Array.Sort(_points, (a, b) => a.x.CompareTo(b.x));
        Debug.Log(string.Join(" ", _points));
    }

    public float ApplySpline(float p)
    {
        var size = _points.Length;
        if (size == 0) return p;

        if (p <= _points[0].x) return _points[0].y;
        if (p >= _points[size - 1].x) return _points[size - 1].y;

        int low = 0, high = size - 1;
        while (low < high - 1)
        {
            var mid = (low + high) / 2;
            if (p < _points[mid].x)
                high = mid;
            else
                low = mid;
        }

        var p1 = _points[low];
        var p2 = _points[high];
        var t = (p - p1.x) / (p2.x - p1.x);
        return Mathf.Lerp(p1.y, p2.y, t);
    }
}