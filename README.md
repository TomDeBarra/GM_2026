[README.md](https://github.com/user-attachments/files/35402976/README.md)# Assignment 4 — Unity MCP Setup Instructions

This guide explains how to connect **Claude Desktop** to the Unity project using **Model Context Protocol (MCP)**. Once configured, Claude can call the `move_forward` tool to make the player character walk forward for **5 seconds**.

## 1. Files already included in the repository

The project already contains the required code:

- **`PlayerMovement.cs`** — Controls the player's movement and animations.
- **`MCPController.cs`** — Receives movement commands from the local Python server.
- **`UnityMCP/server.py`** — Runs the MCP server for Claude and the Flask HTTP server used by Unity.

**You do not need to rewrite these scripts.** Follow the setup steps below on your own computer.

## 2. Install Python dependencies

Make sure [Python](https://www.python.org/downloads/) is installed. Then open **Command Prompt (CMD)** and run:

```cmd
python -m pip install "mcp[cli]" flask
```

This installs the Python MCP SDK and Flask, which are required by `server.py`.

## 3. Install and configure Claude Desktop

1. Install and open [Claude Desktop](https://claude.com/download).
2. Sign in to your account.
3. Click your profile/name in the lower-left corner.
4. Go to **Settings → Desktop App → Extensions** and locate the MCP server configuration option (**Edit/Open Config**). The exact menu labels may vary by Claude version.
5. Open **`claude_desktop_config.json`** in a text editor, such as Notepad.

### Add the Unity MCP server

Add the following **`mcpServers`** property inside the outermost `{ ... }` of the JSON file, **before its final `}`**:

```json
"mcpServers": {
  "unity": {
    "command": "python",
    "args": [
      "C:\\path\\to\\your\\repository\\UnityMCP\\server.py"
    ]
  }
}
```

**Important:** Replace the example path with the actual location of `server.py` on **your own computer**. Because this is JSON, Windows path separators must be written as **double backslashes (`\\`)**.

Also include a **comma after the preceding property** when adding `mcpServers` to an existing JSON configuration.

For example, if the preceding property is `coworkUserFilesPath`, the **end of the JSON file** could look like this:

```json
{
  "coworkUserFilesPath": "C:\\Users\\ExampleUser\\Claude",
  "mcpServers": {
    "unity": {
      "command": "python",
      "args": [
        "C:\\Users\\ExampleUser\\Desktop\\GM2026New\\UnityMCP\\server.py"
      ]
    }
  }
}
```

This is an **illustrative configuration**. If your existing JSON file contains other settings (such as `preferences`), **keep those settings** and add only the `mcpServers` property—do not replace the whole file with the example above.

Save the file and **fully restart Claude Desktop** so it can load the new MCP server configuration.

## 4. Open the correct Unity scene

1. Open the Unity project.
2. If the scene is empty, open **`SampleScene`** from the project's **Scenes** folder.
3. Confirm that the player character is visible.
4. Press **Play** in Unity.

## 5. Test the MCP connection

In Claude Desktop, enter:

> **Use the `move_forward` tool.**

Using the exact tool name makes it easier for Claude to select the correct MCP tool.

**Expected result:** Claude invokes the tool, and the Unity player moves forward for **5 seconds** using the project's existing movement and animation code.

## Troubleshooting

- **`unity couldn't start`** — Check Claude's MCP server logs. Confirm that Python and the dependencies are installed and that the configured `server.py` path is correct.
- **Claude cannot find `move_forward`** — Check that the `unity` MCP server appears as connected in Claude Desktop, and restart Claude after changing the configuration.
- **Claude calls the tool, but nothing moves** — Check that Unity is in Play mode, `SampleScene` is loaded, and the player has the `MCPController` and `PlayerMovement` scripts attached.
- **JSON configuration error** — Check for missing commas, missing closing braces, and single backslashes in Windows file paths.

---

**How it works:** Claude Desktop → Python MCP server (`server.py`) → Flask HTTP endpoint → Unity `MCPController.cs` → `PlayerMovement.cs`.
