import server

class TextSend:
    @classmethod
    def INPUT_TYPES(s):
        return {"required": {"text": ("STRING", {"forceInput": True})}}
    OUTPUT_NODE = True
    RETURN_TYPES = ()
    FUNCTION = "run"
    CATEGORY = "MyCustomClient"

    def run(self, text):
        server.PromptServer.instance.send_sync("send_text", {"text": text})
        return ()


NODE_CLASS_MAPPINGS = {
    "TextSend": TextSend,
}

NODE_DISPLAY_NAME_MAPPINGS = {
    "TextSend": "TextSend",
}