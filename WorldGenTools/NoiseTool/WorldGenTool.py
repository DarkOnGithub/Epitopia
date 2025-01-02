import dearpygui.dearpygui as imgui
from enum import Enum

class WorldGenToolMode(Enum):
    Noise = 0
    Biome = 1
    SurfaceRule = 3
    Carver = 4 
    

class Noise:
    def render(parent):
        pass

class WorldGenTool:
    def __init__(self):
        self.mode = WorldGenToolMode.Noise
        
    def on_mode_change(self, sender):
        print(imgui.get_item_label(sender))
        self.mode = WorldGenToolMode[imgui.get_item_label(sender)]
        imgui.set_item_label(self.menu_selector, self.mode.name)        

    def render(self):
        with imgui.window(label="WorldGenTool") as self.window:
            with imgui.menu_bar():  # Use menu_bar() for window menu
                with imgui.menu(label=self.mode.name) as self.menu_selector:
                    for mode in WorldGenToolMode:
                        imgui.add_menu_item(label=mode.name, callback=self.on_mode_change)
            with imgui.tree_node(label="Category"):
                with imgui.group():
                    imgui.add_text("Temperature")
                    imgui.add_input_int(label="##temp", default_value=0)
                    
                    imgui.add_text("Downfall")
                    imgui.add_input_int(label="##down", default_value=0)
                    
                    with imgui.tree_node(label="Effects"):
                        with imgui.group():
                            imgui.add_text("Sky color")
                            imgui.add_input_int(label="##sky", default_value=0)
                            
                            imgui.add_text("Fog color")
                            imgui.add_input_int(label="##fog", default_value=0)
                            
                            imgui.add_text("Water color")
                            imgui.add_input_int(label="##water", default_value=0)