
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MCPController : MonoBehaviour
{
    private PlayerMovement player;

    [System.Serializable]
    public class Command
    {
        public bool move;
    }

    IEnumerator Start()
    {
        player = GetComponent<PlayerMovement>();

        while (true)
        {
            using (var request = UnityWebRequest.Get("http://127.0.0.1:8765/next"))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Command command = JsonUtility.FromJson<Command>(
                        request.downloadHandler.text
                    );

                    if (command.move)
                        yield return StartCoroutine(MoveForward());
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator MoveForward()
    {
        player.aiMoveForward = true;
        yield return new WaitForSeconds(5f);
        player.aiMoveForward = false;
    }
}