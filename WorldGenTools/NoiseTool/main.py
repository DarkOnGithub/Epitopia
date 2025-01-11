import dearpygui.dearpygui as imgui
import math
from FastNoise import FastNoise
from typing import List, Tuple, Dict
from SplineEditor import SplineEditor
from NoisePreview import NoisePreview
from WorldGenTool import WorldGenTool
from HeightMapPreview import HeightMapPreview
def main():
    imgui.create_context()
    spline_editor = SplineEditor()
    noise_editor = NoisePreview()
    height_map_preview = HeightMapPreview()
    height_map_preview.render()
    spline_editor.render()
    noise_editor.render()
    World_gen_tool = WorldGenTool(noise_editor, spline_editor)

    imgui.create_viewport(title='Spline Editor', width=800, height=600)
    imgui.setup_dearpygui()
    
    World_gen_tool.render()

    noise_editor.hide()
    spline_editor.hide()
    imgui.show_viewport()
    imgui.start_dearpygui()
    imgui.destroy_context()

if __name__ == "__main__":
    main()