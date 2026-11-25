
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

public class AIChat : MonoBehaviour
{
    public TMP_InputField input;
    public TMP_Text responseText;

    [System.Serializable]
    public class ChatMessage
    {
        public string message;
    }

    [System.Serializable]
    public class ChatResponse
    {
        public string reply;
    }

    public void SendMessageToAI()
    {
        if (string.IsNullOrWhiteSpace(input.text))
            return;

        StartCoroutine(SendToPython(input.text));
    }

    IEnumerator SendToPython(string message)
    {
        responseText.text = "Thinking...";

        ChatMessage chat = new ChatMessage();
        chat.message = message;

        string json = JsonUtility.ToJson(chat);

        using (UnityWebRequest request = new UnityWebRequest(
            "http://127.0.0.1:8765/chat", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(
                Encoding.UTF8.GetBytes(json));

            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader(
                "Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ChatResponse response =
                    JsonUtility.FromJson<ChatResponse>(
                        request.downloadHandler.text);

                responseText.text = response.reply;
            }
            else
            {
                responseText.text =
                    "Request failed: " + request.error;
            }
        }
    }
}
