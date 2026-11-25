from mcp.server.mcpserver import MCPServer
from mcp import client
from flask import Flask, jsonify, request
from threading import Thread
from queue import Queue, Empty

from ollama import chat as ollama_chat
import asyncio

mcp = MCPServer("Unity Controller")
app = Flask(__name__)
commands = Queue()

@mcp.tool()
def move_forward() -> str:
    """Move the Unity player forward for 5 seconds."""
    commands.put("forward")
    return "Moving forward for 5 seconds."

@app.get("/next")
def get_command():
    try:
        commands.get_nowait()
        return jsonify(move=True)
    except Empty:
        return jsonify(move=False)

# Send a message to Ollama and execute MCP tools
async def process_message(message):
    async with Client(mcp) as client:

        # Discover the MCP tools
        available = await client.list_tools()

        tools = [
            {
                "type": "function",
                "function": {
                    "name": tool.name,
                    "description": tool.description or "",
                    "parameters": tool.input_schema
                }
            }
            for tool in available.tools
        ]

        # Ask the local AI what to do
        response = ollama_chat(
            model="qwen3:4b-instruct",
            messages=[
                {
                    "role": "system",
                    "content": (
                        "You control a Unity player. "
                        "When asked to move or walk forward, "
                        "call the move_forward tool. "
                        "Only forward movement is available."
                    )
                },
                {
                    "role": "user",
                    "content": message
                }
            ],
            tools=tools,
            options={"temperature": 0}
        )

        # Execute the tool through MCP
        if response.message.tool_calls:
            for call in response.message.tool_calls:
                if call.function.name == "move_forward":
                    result = await client.call_tool(
                        "move_forward", {}
                    )

                    if result.is_error:
                        return "MCP tool failed."

                    return "Moving forward for 5 seconds."

        return response.message.content or "No movement requested."

# New endpoint: Unity sends text here
@app.post("/chat")
def chat_from_unity():
    data = request.get_json(silent=True) or {}
    message = data.get("message", "")

    if not isinstance(message, str) or not message.strip():
        return jsonify(reply="Enter a message."), 400

    if len(message) > 500:
        return jsonify(reply="Message too long."), 400

    try:
        reply = asyncio.run(process_message(message))
        return jsonify(reply=reply)

    except Exception:
        app.logger.exception("AI request failed")
        return jsonify(reply="Error. Check Python logs."), 500

if __name__ == "__main__":
    Thread(
        target=lambda: app.run(
            host="127.0.0.1",
            port=8765,
            use_reloader=False
        ),
        daemon=True
    ).start()

    mcp.run()