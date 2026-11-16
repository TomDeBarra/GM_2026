These steps need to unfortunately be performed manually outside of the GitHub repo in order to perform MCP for assignment 4
(The repo already includes a server.py, that is a Python server that will act as a middleman between Claude and Unity, so that step is already done)

Python MCP = Communicates with Grafana
Flask (HTTP) = Communicates with Unity, basically a local server that starts up and then tells commands to Unity (move = true)

Run python -m pip install "mcp[cli]" flask
in CMD

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

It might have a bunch of blabble in it so copy paste the entire .json and then ask ChatGPT where it should go

That is all that is necessary
