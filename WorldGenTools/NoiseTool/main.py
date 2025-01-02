import dearpygui.dearpygui as imgui
import math
from typing import List, Tuple, Dict
from SplineEditor import SplineEditor
from NoisePreview import NoisePreview
from WorldGenTool import WorldGenTool
def main():
    imgui.create_context()
    spline_editor = SplineEditor()
    noise_editor = NoisePreview()
    World_gen_tool = WorldGenTool()
    imgui.create_viewport(title='Spline Editor', width=800, height=600)
    imgui.setup_dearpygui()
    
    World_gen_tool.render()
    spline_editor.render()
    noise_editor.render()
    imgui.show_viewport()
    imgui.start_dearpygui()
    imgui.destroy_context()

if __name__ == "__main__":
    main()