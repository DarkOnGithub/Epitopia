import dearpygui.dearpygui as imgui
from typing import List, Tuple, Dict
from Spline import Spline
class SplineEditor:
    def __init__(self):
        self.points = {}
        self.point_id = 0
        self.line = None
        self.plot = None
        self.spline_editor = None
        self.mode = 0
        self.spline = Spline()
    def clamp(self, value: float, min_val: float, max_val: float) -> float:
        return max(min_val, min(value, max_val))
    
    def on_point_move(self, sender, data):
        if self.mode == 1:
            self.spline.remove_point(imgui.get_value(sender)[0])
            del self.points[imgui.get_item_alias(sender)]
            imgui.delete_item(sender)
            self.update_lines()
            self.mode = 0
            return
        x_min_max = imgui.get_axis_limits("x_axis")
        y_min_max = imgui.get_axis_limits("y_axis")
        
        new_pos = [
            self.clamp(imgui.get_value(sender)[0], x_min_max[0], x_min_max[1]),
            self.clamp(imgui.get_value(sender)[1], y_min_max[0], y_min_max[1])
        ]
        
        imgui.set_value(sender, new_pos)
        item_name = imgui.get_item_alias(sender)
        
        if self.points[item_name] != new_pos:
            self.points[item_name] = new_pos
            self.update_lines()
        
    def add_new_point(self, at: List[float] = [0.5, 0.5]):
        point_tag = f"point_{self.point_id}"
        
        point_id = imgui.add_drag_point(
            color=[255, 255, 255, 255],
            default_value=at,
            callback=self.on_point_move,
            parent=self.plot,
            tag=point_tag
        )
        
        self.point_id += 1
        self.points[point_id] = at
        self.spline.add_control_point(at[0], at[1])
        self.update_lines()
        
    def update_lines(self):
        if self.line:
            imgui.delete_item(self.line)
            self.line = None
        
        points = list(self.points.values())
        if len(points) < 2:
            return
            
        sorted_points = sorted(points, key=lambda point: point[0])
        lines_x = [point[0] for point in sorted_points]
        lines_y = [point[1] for point in sorted_points]
        
        self.line = imgui.add_line_series(
            x=lines_x,
            y=lines_y,
            label="",
            parent="x_axis"
        )

    def render(self):
        with imgui.window(label="Splines Editor", width=400, height=400) as self.window:
            with imgui.plot(label="Editor", height=-1, width=-1, ) as plot:
                self.plot = plot    
                x_axis = imgui.add_plot_axis(imgui.mvXAxis, label="X", tag="x_axis")
                imgui.set_axis_limits(x_axis, 0, 1)
                y_axis = imgui.add_plot_axis(imgui.mvYAxis, label="Y", tag="y_axis")
                imgui.set_axis_limits(y_axis, 0, 1)
                self.add_new_point(at=(0, 0))
                self.add_new_point(at=(1, 1))
                
            with imgui.group(horizontal=True):
                imgui.add_button(label="Add Point", callback=lambda: self.add_new_point())
                imgui.add_button(label="Remove Point", callback=lambda: setattr(self, 'mode', 1))

    def show(self):
        imgui.show_item(self.window)
    
    def hide(self):
        imgui.hide_item(self.window)