class TextInput:
    @classmethod
    def INPUT_TYPES(s):
        return {"required": 
                {
                    "text": ("STRING", {"multiline": True}),
                    "seed": ("INT:seed", {}),
                }, 
                }
    RETURN_TYPES = ("STRING",)
    FUNCTION = "run"
    CATEGORY = "MyCustomNodes"

    def run(self, text, seed = None):
        return (text,)

class TextOutput:
    @classmethod
    def INPUT_TYPES(s):
        return {"required": {"text": ("STRING", {"forceInput": True})}}
    OUTPUT_NODE = True
    RETURN_TYPES = ()
    FUNCTION  = "run"
    CATEGORY = "MyCustomNodes"

    def run(self, text):
        print(text)
        return text

NODE_CLASS_MAPPINGS = {
    "TextInput": TextInput,
    "TextOutput": TextOutput,
}

NODE_DISPLAY_NAME_MAPPINGS = {
    "TextInput": "TextInput",
    "TextOutput": "TextOutput",
}
