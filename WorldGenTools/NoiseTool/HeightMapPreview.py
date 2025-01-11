import dearpygui.dearpygui as imgui


class HeightMapPreview:
    def __init__(self):
        pass
    
    def render(self):
        with imgui.window(label="Height Map Preview") as self.window:
            with imgui.plot(label="HeightMap", height=-1, width=-1):
                imgui.add_plot_axis(imgui.mvXAxis, label="X", no_gridlines=True)
                imgui.add_plot_axis(imgui.mvYAxis, label="Y", tag="height_map_y")
                

    def update(self, data):
        imgui.add_line_series(*data, parent="height_map_y")
        
    def show(self):
        imgui.show_item(self.window)
        
    def hide(self):
        imgui.hide_item(self.window)
