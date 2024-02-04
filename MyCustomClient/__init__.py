import server
from .nodes import NODE_CLASS_MAPPINGS, NODE_DISPLAY_NAME_MAPPINGS
from aiohttp import web

WEB_DIRECTORY = "./js"
__all__ = ['NODE_CLASS_MAPPINGS', 'NODE_DISPLAY_NAME_MAPPINGS', 'WEB_DIRECTORY']

# テキストを更新し、queuePromptを実行するエンドポイントを追加
@server.PromptServer.instance.routes.post('/my_custom_client/update_text')
async def update_text(request):
    data = await request.json()
    text = data["text"]
    # クライアントでテキストを更新するメッセージを送信
    server.PromptServer.instance.send_sync("update_text", data)
    # クライアントでqueuePromptを呼び出すメッセージを送信
    server.PromptServer.instance.send_sync("run_workflow", {})
    return web.Response()