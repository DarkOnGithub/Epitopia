from typing import List, Optional, Union
import math
from Vector2 import Vector2

class Spline:
    def __init__(self, floating_points: Optional[List[float]] = None):
        self._points: List[Vector2] = []
        self._size = 0
        if floating_points:
            if len(floating_points) % 2 != 0:
                raise ValueError("The number of elements in floating_points must be even")
            self.add_points([
                Vector2(floating_points[i], floating_points[i + 1])
                for i in range(0, len(floating_points), 2)
            ])

    def add_point(self, x: float, y: float) -> None:
        self.add_points([Vector2(x, y)])

    def add_points(self, points: List[Vector2]) -> None:
        self._points.extend(points)
        self._points.sort(key=lambda p: p.x)
        self._size = len(self._points)

    def remove_point(self, x: float, threshold: float = 0.001) -> bool:
        for i, point in enumerate(self._points):
            if abs(point.x - x) < threshold:
                self._points.pop(i)
                self._size -= 1
                return True
        return False

    def clear(self) -> None:
        self._points.clear()
        self._size = 0

    def get_points(self) -> List[Vector2]:
        return self._points.copy()

    def apply_spline(self, p: float) -> float:
        if self._size == 0:
            return p
        if self._size == 1:
            return self._points[0].y
        if p <= self._points[0].x:
            return self._points[0].y
        if p >= self._points[self._size - 1].x:
            return self._points[self._size - 1].y

        for i in range(1, self._size):
            if p <= self._points[i].x:
                p1 = self._points[i - 1]
                p2 = self._points[i]
                t = (p - p1.x) / (p2.x - p1.x)
                return p1.y + t * (p2.y - p1.y)

        return p