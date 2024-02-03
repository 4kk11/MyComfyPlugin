import os
import server
from .nodes import NODE_CLASS_MAPPINGS, NODE_DISPLAY_NAME_MAPPINGS
from aiohttp import web

WEB_DIRECTORY = "./js"
__all__ = ['NODE_CLASS_MAPPINGS', 'NODE_DISPLAY_NAME_MAPPINGS', 'WEB_DIRECTORY']

@server.PromptServer.instance.routes.post('/my_custom_client/update_text')
async def update_text(request):
    data = await request.json()
    text = data["text"]
    
    server.PromptServer.instance.send_sync("update_text", data)
    server.PromptServer.instance.send_sync("run_workflow", {})
    return web.Response()