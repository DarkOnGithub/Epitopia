from FastNoise import FastNoise
import dearpygui.dearpygui as imgui
import array

class NoisePreview:
    def __init__(self):
        self.size = (256, 256)
        self.raw_image = array.array("f", [0] * (self.size[0] * self.size[1] * 3))
        self.position = (0, 0)
        self.dragging = False
        self.last_mouse = (0, 0)
        self.noise = FastNoise.from_encoded_node_tree("FwAAAIC/AACAPwAAAAAAAIA/CQA=")
        self.sensivity = 0.5
        
    def on_drag(self, sender, mouse_data):
        if not imgui.is_item_hovered(self.window):
            return
        button, x, y = mouse_data
        if x == 0 and y == 0:
            return
        if button == 0:
            if not self.dragging:
                self.dragging = True
                self.last_mouse = (x, y)
            else:
                current_mouse = (x, y)
                delta_x = self.last_mouse[0] - current_mouse[0] 
                delta_y =self.last_mouse[1] -  current_mouse[1]
                self.position = (
                    self.position[0] + delta_x * self.sensivity,
                    self.position[1] + delta_y * self.sensivity
                )
                self.last_mouse = current_mouse
                self.update_noise(self.noise)

    def on_release(self, sender, mouse_data):
        self.dragging = False


    def render(self):
        with imgui.window(label="Noise Preview", width=400, height=400) as self.window:
            with imgui.texture_registry(show=False): 
                imgui.add_raw_texture(
                    width=self.size[0],
                    height=self.size[1],
                    default_value=self.raw_image,
                    format=imgui.mvFormat_Float_rgb,
                    tag="noise_texture"
                )
            
            image_id = imgui.add_image("noise_texture")
            self.image_id = image_id
            with imgui.handler_registry():
                imgui.add_mouse_drag_handler(callback=self.on_drag)
                imgui.add_mouse_release_handler(callback=self.on_release)
            imgui.bind_item_handler_registry(image_id, "handler_registry")
            
    def update_noise(self, noise: FastNoise):
        buffer = [0] * (self.size[0] * self.size[1])

        noise.gen_uniform_grid_2d(
            buffer,
            int(self.position[0]),
            int(self.position[1]),
            self.size[0],
            self.size[1],
            0.01,
            10
        )

        rgb_buffer = [value for value in buffer for _ in range(3)]

        self.raw_image = array.array('f', rgb_buffer)

        imgui.set_value("noise_texture", self.raw_image)
