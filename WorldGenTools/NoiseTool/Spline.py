from typing import List, Optional, Sequence
from Vector2 import Vector2

class Spline:
    """Represents a 2D spline curve defined by control points."""
    
    def __init__(self, control_points: Optional[Sequence[float]] = None):
        """Initialize spline with optional control points as flat array [x1,y1,x2,y2,...]"""
        self._points: List[Vector2] = []
        if control_points:
            if len(control_points) % 2 != 0:
                raise ValueError("Control points must contain even number of elements")
            self.add_control_points([
                Vector2(control_points[i], control_points[i + 1])
                for i in range(0, len(control_points), 2)
            ])

    def add_control_point(self, x: float, y: float) -> None:
        """Add a single control point to the spline."""
        self.add_control_points([Vector2(x, y)])

    def add_control_points(self, points: List[Vector2]) -> None:
        """Add multiple control points and sort them by x coordinate."""
        self._points.extend(points)
        self._points.sort(key=lambda p: p.x)

    def remove_control_point(self, x: float, threshold: float = 0.001) -> bool:
        """Remove control point at given x coordinate within threshold."""
        for i, point in enumerate(self._points):
            if abs(point.x - x) < threshold:
                self._points.pop(i)
                return True
        return False

    def clear(self) -> None:
        """Clear all control points from the spline."""
        self._points.clear()

    def get_control_points(self) -> List[Vector2]:
        """Get a copy of the control points."""
        return self._points.copy()

    def evaluate(self, t: float) -> float:
        """Evaluate the spline at parameter t."""
        if not self._points:
            return t
        if len(self._points) == 1:
            return self._points[0].y

        # Clamp to spline range
        if t <= self._points[0].x:
            return self._points[0].y
        if t >= self._points[-1].x:
            return self._points[-1].y

        # Find segment and interpolate
        for i in range(1, len(self._points)):
            if t <= self._points[i].x:
                p1, p2 = self._points[i - 1], self._points[i]
                t_segment = (t - p1.x) / (p2.x - p1.x)
                return p1.y + t_segment * (p2.y - p1.y)

        return t