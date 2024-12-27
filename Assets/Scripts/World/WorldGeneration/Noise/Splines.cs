using UnityEngine;

public class Splines
{
    private Vector2[] _points;
    private int _size;

    public Splines(float[] floatingPoints)
    {
        if (floatingPoints == null) throw new System.ArgumentNullException(nameof(floatingPoints));
        if (floatingPoints.Length % 2 != 0)
            throw new System.ArgumentException("The number of elements in floatingPoints must be even.",
                                               nameof(floatingPoints));

        var points = new Vector2[floatingPoints.Length / 2];
        for (var i = 0; i < floatingPoints.Length; i += 2)
            points[i / 2] = new Vector2(floatingPoints[i], floatingPoints[i + 1]);

        System.Array.Sort(points, (a, b) => a.x.CompareTo(b.x));
        _points = points;
        _size = points.Length;
    }

    public float ApplySpline(float p)
    {
        if (_size == 0) return p;

        if (_size == 1) return _points[0].y;

        if (p <= _points[0].x) return _points[0].y;

        if (p >= _points[_size - 1].x) return _points[_size - 1].y;

        for (var i = 1; i < _size; i++)
            if (p <= _points[i].x)
            {
                var p1 = _points[i - 1];
                var p2 = _points[i];
                return Mathf.Lerp(p1.y, p2.y, (p - p1.x) / (p2.x - p1.x));
            }

        return p;
    }
}