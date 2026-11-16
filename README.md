The MCP Grafana/Unity setup basically consists of three things:
* Grafana
* A Python server script with Flask (HTTP local server that will communicate with Unity) and MCP (that will communicate with Grafana) the Python server script acts as a middleman between the two
* Unity (with a script that will receive a Flask request and then set AIPlayerMovement = true for 5 seconds)

These steps need to unfortunately be performed manually outside of the GitHub repo in order to perform MCP for assignment 4
(The repo already includes a server.py, that is a Python server that will act as a middleman between Claude and Unity, so that step is already done)

Python MCP = Communicates with Grafana

Flask (HTTP) = Communicates with Unity, basically a local server that starts up and then tells commands to Unity (move = true)

Run python -m pip install "mcp[cli]" flask
in CMD

If you didn't install the Python MCP/Flask through CMD, the server.py in the repo wouldn't work

Install Claude desktop

In Claude you must change the config to include 
(Go settings, desktop app, extensions, MCP server should bring you to .json file for configuration of Claude)

<img width="1090" height="837" alt="image" src="https://github.com/user-attachments/assets/d39ae775-ffd9-4ca0-87ed-281b1f484746" />

```{
  "mcpServers": {
    "unity": {
      "command": "python",
      "args": [
        "C:\\UnityMCP\\server.py"
      ]
    }
  }
}
```

REPLACE "C:\\UnityMCP\\server.py" WITH THE GITHUB REPO PATH FOR server.py, E.G. "C:\\Users\\StudentDylanBarry(TL\\Desktop\\GM_2026\\UnityMCP\\server.py"

It might have a bunch of blabble in it so copy paste the entire .json and then ask ChatGPT where it should go

That is all that is necessary
