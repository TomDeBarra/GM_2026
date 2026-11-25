from mcp.server.mcpserver import MCPServer
from flask import Flask, jsonify
from threading import Thread
from queue import Queue, Empty

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