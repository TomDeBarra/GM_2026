
using UnityEngine;
using Unity.Netcode;

public class NetworkLauncher : MonoBehaviour
{
    void OnGUI()
    {
        if (NetworkManager.Singleton == null ||
            NetworkManager.Singleton.IsListening)
            return;

        if (GUI.Button(new Rect(10, 10, 120, 40), "Host"))
            NetworkManager.Singleton.StartHost();

        if (GUI.Button(new Rect(10, 60, 120, 40), "Client"))
            NetworkManager.Singleton.StartClient();
    }
}
