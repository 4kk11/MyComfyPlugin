import os
from nodes import load_custom_node

dir = os.path.dirname(os.path.realpath(__file__))
plugin_folders = [
    "MyCustomNodes",
]
for folder in plugin_folders:
    plugin_dir = os.path.join(dir, folder)
    if os.path.isdir(plugin_dir):
        load_custom_node(plugin_dir)
