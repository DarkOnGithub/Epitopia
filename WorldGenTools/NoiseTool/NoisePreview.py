from dataclasses import dataclass
from FastNoise import FastNoise
import dearpygui.dearpygui as dpg
import array
from typing import Tuple, Optional

@dataclass
class ViewState:
    position: Tuple[float, float] = (0.0, 0.0)
    is_dragging: bool = False
    last_mouse_pos: Tuple[float, float] = (0.0, 0.0)
    sensitivity: float = 0.5

class NoisePreview:
    def __init__(self, width: int = 256, height: int = 256):
        self.dimensions = (width, height)
        self.view_state = ViewState()
        self.window: Optional[int] = None
        self.raw_image = array.array("f", [0] * (width * height * 3))
        self.noise = FastNoise.from_encoded_node_tree("FwAAAIC/AACAPwAAAAAAAIA/CQA=")
        
    def handle_drag(self, sender: int, mouse_data: Tuple[int, float, float]) -> None:
        if not dpg.is_item_hovered(self.window) or not all(mouse_data[1:]):
            return
            
        button, x, y = mouse_data
        if button != 0:
            return

        if not self.view_state.is_dragging:
            self.view_state.is_dragging = True
            self.view_state.last_mouse_pos = (x, y)
        else:
            delta_x = self.view_state.last_mouse_pos[0] - x
            delta_y = self.view_state.last_mouse_pos[1] - y
            
            self.view_state.position = (
                self.view_state.position[0] + delta_x * self.view_state.sensitivity,
                self.view_state.position[1] + delta_y * self.view_state.sensitivity
            )
            self.view_state.last_mouse_pos = (x, y)
            self.update_noise(self.noise)

    def handle_release(self, sender: int, mouse_data: Tuple[int, float, float]) -> None:
        self.view_state.is_dragging = False

    def render(self) -> None:
        with dpg.window(label="Noise Preview", width=400, height=400) as self.window:
            with dpg.texture_registry(show=False): 
                dpg.add_raw_texture(
                    width=self.dimensions[0],
                    height=self.dimensions[1],
                    default_value=self.raw_image,
                    format=dpg.mvFormat_Float_rgb,
                    tag="noise_texture"
                )
            
            image_id = dpg.add_image("noise_texture")
            self.image_id = image_id
            with dpg.handler_registry():
                dpg.add_mouse_drag_handler(callback=self.handle_drag)
                dpg.add_mouse_release_handler(callback=self.handle_release)
            dpg.bind_item_handler_registry(image_id, "handler_registry")
            
    def update_noise(self, noise: FastNoise, frequency: float = 0.1, seed: int = 1, y: int = 0) -> None:
        buffer = [0] * (self.dimensions[0] * self.dimensions[1])

        noise.gen_uniform_grid_2d(
            buffer,
            int(self.view_state.position[0]),
            int(self.view_state.position[1]),
            self.dimensions[0],
            self.dimensions[1],
            frequency,
            seed
        )

        rgb_buffer = [value for value in buffer for _ in range(3)]

        self.raw_image = array.array('f', rgb_buffer)

        dpg.set_value("noise_texture", self.raw_image)

    def show(self) -> None:
        dpg.show_item(self.window)
        
    def hide(self) -> None:
        dpg.hide_item(self.window)