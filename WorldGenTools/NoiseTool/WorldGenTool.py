from abc import ABC, abstractmethod
from enum import Enum, auto
from typing import Dict, Any, Optional
from dataclasses import dataclass
import dearpygui.dearpygui as imgui
from pathlib import Path
import json
from FastNoise import FastNoise
from NoisePreview import NoisePreview
from SplineEditor import SplineEditor

class WorldGenComponent(ABC):
    """Base class for all world generation components."""
    
    def __init__(self, noise_preview: NoisePreview, spline_editor: SplineEditor):
        self.noise_preview = noise_preview
        self.spline_editor = spline_editor
        self.file_path: Optional[Path] = None
        self.buttons = None
        self._setup_file_dialog()
    
    @abstractmethod
    def to_json(self) -> Dict[str, Any]:
        """Convert component state to JSON serializable dict."""
        pass
    
    @abstractmethod
    def from_json(self, data: Dict[str, Any]) -> None:
        """Restore component state from JSON data."""
        pass
    
    def _setup_file_dialog(self) -> None:
        with imgui.file_dialog(
            directory_selector=False,
            show=False,
            tag=f"file_dialog_{id(self)}",
            width=700,
            height=400,
            callback=self._handle_file_selected
        ):
            imgui.add_file_extension(".json", color=(158, 198, 104))

    def _handle_file_selected(self, sender, appdata): 
        ext = appdata["file_path_name"].split(".")[-1]
        if ext[-1] == "*":
            self.file_path = Path(".".join(appdata["file_path_name"].split(".")[:-1]) + ".json")
        else:
            self.file_path = Path(appdata["file_path_name"])
        self.save()
    
    def save(self):
        if not self.file_path:
            self.save_as()
            return
        with open(self.file_path, "w") as file:
            file.write(json.dumps(self.to_json()))
        file.close()
    
    def save_as(self):
        imgui.show_item(f"file_dialog_{id(self)}")

    def show_save_buttons(self):
        if self.buttons:
            imgui.delete_item(self.buttons)
        with imgui.group(horizontal=True) as self.buttons:
            imgui.add_button(label="Save", callback=self.save, parent=imgui.last_container())
            imgui.add_button(label="Save As", callback=self.save_as, parent=imgui.last_container())

    @abstractmethod
    def render(self, parent):
        pass

class BiomeComponent(WorldGenComponent):
    def __init__(self, noise_preview, spline_editor):
        super().__init__(noise_preview, spline_editor)
    
    def to_json(self) -> Dict[str, Any]:
        return {}
    
    def from_json(self, data: Dict[str, Any]) -> None:
        pass
    
    def render(self, parent):
        pass
    
class SurfaceRuleComponent(WorldGenComponent):
    def __init__(self, noise_preview, spline_editor):
        super().__init__(noise_preview, spline_editor)
    
    def to_json(self) -> Dict[str, Any]:
        return {}
    
    def from_json(self, data: Dict[str, Any]) -> None:
        pass
    
    def render(self, parent):
        pass
    
class CarverComponent(WorldGenComponent):
    def __init__(self, noise_preview: NoisePreview, spline_editor: SplineEditor):
        super().__init__(noise_preview, spline_editor)
    
    def to_json(self) -> Dict[str, Any]:
        return {}
    
    def from_json(self, data: Dict[str, Any]) -> None:
        pass
    
    def render(self, parent):
        pass

class NoiseComponent(WorldGenComponent):
    def __init__(self, noise_preview: NoisePreview, spline_editor: SplineEditor):
        super().__init__(noise_preview, spline_editor)
    
    def on_noise_change(self, sender):
        try:
            self.noise_preview.update_noise(
                FastNoise.from_encoded_node_tree(imgui.get_value("noise_tree_input")),
                imgui.get_value("frequency_input")
            )
            self.noise_preview.show()
        except:
            self.noise_preview.hide()
            print("Invalid FastNoise Tree")
    
    def to_json(self) -> Dict[str, Any]:
        try:
            hash = imgui.get_value("noise_tree_input")
            if hash == "":
                raise Exception("Invalid FastNoise Tree")
            FastNoise.from_encoded_node_tree(hash),            
            return {
                "Noise": imgui.get_value("noise_tree_input"),
                "Frequency": imgui.get_value("frequency_input")
            }
        except:
            print("Invalid FastNoise Tree, not saving")
            return {}
    
    def from_json(self, data: Dict[str, Any]) -> None:
        pass
    
    def render(self, parent):
        with imgui.group(parent=parent) as tree:
            imgui.add_input_text(label="Fastnoise Tree", callback=self.on_noise_change, tag="noise_tree_input")
            imgui.add_input_float(label="Frequency", default_value=0.1, tag="frequency_input", callback=self.on_noise_change, step=0.001, min_value=0)
        self.show_save_buttons()
        return tree
class NoiseSettings(WorldGenComponent):
    def __init__(self, noise_preview, spline_editor):
        super().__init__(noise_preview, spline_editor)
    
    def to_json(self) -> Dict[str, Any]:
        return {}
    
    def from_json(self, data: Dict[str, Any]) -> None:
        pass
    
    def render(self, parent):
        pass
    
    
class WorldGenToolMode(Enum):
    Noise = auto()
    Biome = auto()
    SurfaceRule = auto()
    Carver = auto()
    NoiseSettings = auto()
    @property
    def component_class(self):
        return {
            WorldGenToolMode.Noise: NoiseComponent,
            WorldGenToolMode.Biome: BiomeComponent,
            WorldGenToolMode.SurfaceRule: SurfaceRuleComponent,
            WorldGenToolMode.Carver: CarverComponent,
            WorldGenToolMode.NoiseSettings: NoiseSettings
        }[self]

class WorldGenTool:
    def __init__(self, noise_preview: NoisePreview, spline_editor: SplineEditor):
        self.noise_preview = noise_preview
        self.spline_editor = spline_editor
        self.mode = WorldGenToolMode.Noise
        self.mode_instance = self.mode.component_class(noise_preview, spline_editor)

    def on_mode_change(self, sender):
        self.mode = WorldGenToolMode[imgui.get_item_label(sender)]
        imgui.set_item_label(self.menu_selector, self.mode.name)        
        imgui.delete_item(self.tree)
        self.mode_instance = self.mode.component_class(self.noise_preview, self.spline_editor)
        self.tree = self.mode_instance.render(self.window)
        
    def render(self):
        with imgui.window(label="WorldGenTool") as self.window:
            with imgui.menu_bar():  
                with imgui.menu(label=self.mode.name) as self.menu_selector:
                    for mode in WorldGenToolMode:
                        imgui.add_menu_item(label=mode.name, callback=self.on_mode_change)
            self.tree = self.mode_instance.render(self.window)