import os
import server
from aiohttp import web

WEB_DIRECTORY = "./js"

__all__ = ['WEB_DIRECTORY']

client_id = "B9F75B5E163146F89C628CE56669C138"

@server.PromptServer.instance.routes.post('/my_custom_client/update_text')
async def update_text(request):
    data = await request.json()
    text = data["text"]
    
    server.PromptServer.instance.send_sync("update_text", data)
    return web.Response()