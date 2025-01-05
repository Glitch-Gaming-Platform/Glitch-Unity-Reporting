using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

public class ApiClient : MonoBehaviour 
{
    private string baseUrl = "https://api.glitch.fun/api/";
    private string authToken = "YOUR_AUTH_TOKEN"; // Normally set from your login process

    public IEnumerator CreateInstallRecord(string titleId, string userInstallId, string platform)
    {
        string endpoint = baseUrl + "titles/" + titleId + "/installs";
        
        // Prepare JSON payload
        string jsonBody = "{\"user_install_id\":\"" + userInstallId + "\",\"platform\":\"" + platform + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Install record created: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}

