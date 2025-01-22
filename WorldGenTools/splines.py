import tkinter as tk
from tkinter import ttk
import json

class SplineEditor(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Spline Editor")
        self.geometry("600x450")  # Increased height to accommodate the point info label

        self.points = [(0, 0), (1,1)]  # Initial points adjusted for bottom-left to top-right
        self.selected_point_index = None

        self.canvas_width = 1800
        self.canvas_height = 900
        self.canvas = tk.Canvas(self, width=self.canvas_width, height=self.canvas_height, bg="black")
        self.canvas.pack(pady=10)

        self.canvas.bind("<Button-1>", self.on_canvas_click)
        self.canvas.bind("<B1-Motion>", self.on_canvas_drag)
        self.canvas.bind("<ButtonRelease-1>", self.on_canvas_release)

        self.point_info_label = ttk.Label(self, text="No point selected")
        self.point_info_label.pack()

        self.button_frame = ttk.Frame(self)
        self.button_frame.pack()

        self.add_button = ttk.Button(self.button_frame, text="Add spline", command=self.add_spline_point)
        self.add_button.pack(side="left", padx=5)

        self.remove_button = ttk.Button(self.button_frame, text="Remove spline", command=self.remove_spline_point)
        self.remove_button.pack(side="left", padx=5)

        self.invert_button = ttk.Button(self.button_frame, text="Invert", command=self.invert_spline)
        self.invert_button.pack(side="left", padx=5)

        self.reverse_button = ttk.Button(self.button_frame, text="Reverse", command=self.reverse_spline_direction)
        self.reverse_button.pack(side="left", padx=5)

        self.export_button = ttk.Button(self.button_frame, text="Export", command=self.export_spline_data)
        self.export_button.pack(side="left", padx=5)

        self.draw_grid()
        self.draw_spline()

    def draw_grid(self):
        grid_color = "gray"
        for i in range(1, 4):
            x = self.canvas_width * i / 4
            self.canvas.create_line(x, 0, x, self.canvas_height, fill=grid_color)
            y = self.canvas_height * i / 4
            self.canvas.create_line(0, y, self.canvas_width, y, fill=grid_color)

    def draw_spline(self):
        self.canvas.delete("spline")
        self.canvas.delete("points")

        if len(self.points) < 2:
            return

        scaled_points = [(p[0] * self.canvas_width, (1 - p[1]) * self.canvas_height) for p in self.points] # Invert y for drawing
        self.canvas.create_line(scaled_points, fill="red", width=3, tags="spline")

        for i, p in enumerate(self.points):
            x_canvas, y_canvas = self.coords_to_canvas(p[0], p[1])
            color = "cyan" if i == self.selected_point_index else "white"
            self.canvas.create_oval(x_canvas - 5, y_canvas - 5, x_canvas + 5, y_canvas + 5, fill=color, tags=("points", f"point_{i}"))

    def canvas_to_coords(self, x, y):
        norm_x = x / self.canvas_width
        norm_y = 1 - (y / self.canvas_height)  # Invert y for internal representation
        return norm_x, norm_y

    def coords_to_canvas(self, norm_x, norm_y):
        x = norm_x * self.canvas_width
        y = (1 - norm_y) * self.canvas_height
        return x, y

    def update_point_info(self):
        if self.selected_point_index is not None:
            x, y = self.points[self.selected_point_index]
            self.point_info_label.config(text=f"Selected Point: ({x:.4f}, {y:.4f})")
        else:
            self.point_info_label.config(text="No point selected")

    def on_canvas_click(self, event):
        x, y = self.canvas_to_coords(event.x, event.y)
        self.selected_point_index = None
        min_distance = 10  # Minimum distance to consider a click on a point
        closest_index = -1

        for i, point in enumerate(self.points):
            px_canvas, py_canvas = self.coords_to_canvas(point[0], point[1])
            distance = ((event.x - px_canvas)**2 + (event.y - py_canvas)**2)**0.5
            if distance < min_distance:
                min_distance = distance
                closest_index = i

        if closest_index != -1:
            self.selected_point_index = closest_index
            self.update_point_info()
        else:
            # Add a new point if no point is selected
            self.add_point_at(x, y)
            self.update_point_info()  # Update to "No point selected" as a new point might not be selected immediately

        self.draw_spline()

    def on_canvas_drag(self, event):
        if self.selected_point_index is not None:
            x, y = self.canvas_to_coords(event.x, event.y)
            self.points[self.selected_point_index] = (
                max(0, min(1, x)),
                max(0, min(1, y)),
            )
            self.draw_spline()
            self.update_point_info()

    def on_canvas_release(self, event):
        # Keep the selected point highlighted after releasing the mouse
        pass

    def add_point_at(self, x, y):
        # Insert the new point based on x-coordinate order
        for i, point in enumerate(self.points):
            if x < point[0]:
                self.points.insert(i, (x, y))
                break
        else:
            self.points.append((x, y))
        self.selected_point_index = None # Deselect any previously selected point

    def add_spline_point(self):
        # Add a new point in the middle, can be improved
        if self.points:
            mid_index = len(self.points) // 2
            if len(self.points) > 1:
                p1 = self.points[mid_index - 1]
                p2 = self.points[mid_index]
                new_x = (p1[0] + p2[0]) / 2
                new_y = (p1[1] + p2[1]) / 2
            else:
                new_x = 0.5
                new_y = 0.5
            self.add_point_at(new_x, new_y)
        else:
            self.points = [(0.5, 0.5)]
        self.draw_spline()
        self.update_point_info()

    def remove_spline_point(self):
        if self.selected_point_index is not None and len(self.points) > 0:
            del self.points[self.selected_point_index]
            self.selected_point_index = None
            self.draw_spline()
            self.update_point_info()
        else:
            self.update_point_info() # Ensure "No point selected" is shown

    def invert_spline(self):
        self.points = [(p[0], 1 - p[1]) for p in self.points]
        self.draw_spline()
        self.update_point_info()

    def reverse_spline_direction(self):
        self.points.reverse()
        self.draw_spline()
        self.update_point_info()

    def get_spline_data_json(self):
        result_array = []
        for point in self.points:
            result_array.extend(point)
        return json.dumps(result_array, indent=4)

    def export_spline_data(self):
        spline_data = self.get_spline_data_json()
        print("Current Spline Data (JSON):")
        print(spline_data)

if __name__ == "__main__":
    app = SplineEditor()
    app.mainloop()