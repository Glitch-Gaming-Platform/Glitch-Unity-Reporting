using UnityEngine;  
using UnityEngine.Networking;  
using System.Collections;  
using System.Text;  
  
/// <summary>  
/// Sample Unity script to interact with GlitchFun analytics endpoints:  
/// 1) Create or update an Install record (also logs retention events)  
/// 2) Send optional fingerprint data  
/// 3) Record purchases  
///  
/// Attach this to a GameObject, set auth/token, call the coroutines in your code.  
/// </summary>  
public class ApiClient : MonoBehaviour  
{  
    [Tooltip("Base URL for the GlitchFun API (typically 'https://api.glitch.fun/api/')")]  
    public string baseUrl = "https://api.glitch.fun/api/";  
  
    [Tooltip("Bearer token for your API calls. Retrieve from your auth system or config.")]  
    public string authToken = "YOUR_AUTH_TOKEN";  
  
    /// <summary>  
    /// Creates an install record if user_install_id is new;  
    /// otherwise logs a retention event if user_install_id already exists.  
    ///  
    /// Optionally pass 'fingerprintJson' for cross-device identification.  
    /// </summary>  
    public IEnumerator CreateInstallRecord(string titleId, string userInstallId, string platform, string fingerprintJson = null)  
    {  
        string endpoint = $"{baseUrl}titles/{titleId}/installs";  
  
        StringBuilder sb = new StringBuilder();  
        sb.Append("{");  
        sb.AppendFormat("\"user_install_id\":\"{0}\",", userInstallId);  
        sb.AppendFormat("\"platform\":\"{0}\"", platform);  
  
        // Insert fingerprint JSON if provided  
        if (!string.IsNullOrEmpty(fingerprintJson))  
        {  
            sb.Append(",");  
            sb.AppendFormat("\"fingerprint_components\":{0}", fingerprintJson);  
        }  
  
        sb.Append("}");  
        string jsonBody = sb.ToString();  
  
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);  
  
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))  
        {  
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);  
            request.downloadHandler = new DownloadHandlerBuffer();  
  
            request.SetRequestHeader("Content-Type", "application/json");  
            request.SetRequestHeader("Authorization", "Bearer " + authToken);  
  
            yield return request.SendWebRequest();  
  
            if (request.result == UnityWebRequest.Result.Success)  
            {  
                Debug.Log("Install record (or retention event) created/updated: " + request.downloadHandler.text);  
            }  
            else  
            {  
                Debug.LogError("Error creating install record: " + request.error);  
            }  
        }  
    }  
  
    /// <summary>  
    /// A helper method that logs a session activity.   
    /// The server will see the same user_install_id => logs a retention event.  
    /// Optionally pass fingerprint data too.  
    /// </summary>  
    public IEnumerator LogSessionActivity(string titleId, string userInstallId, string platform, string sessionId, string fingerprintJson = null)  
    {  
        string endpoint = $"{baseUrl}titles/{titleId}/installs";  
  
        StringBuilder sb = new StringBuilder();  
        sb.Append("{");  
        sb.AppendFormat("\"user_install_id\":\"{0}\",", userInstallId);  
        sb.AppendFormat("\"platform\":\"{0}\",", platform);  
        sb.AppendFormat("\"session_id\":\"{0}\"", sessionId);  
  
        if (!string.IsNullOrEmpty(fingerprintJson))  
        {  
            sb.Append(",");  
            sb.AppendFormat("\"fingerprint_components\":{0}", fingerprintJson);  
        }  
  
        sb.Append("}");  
        string jsonBody = sb.ToString();  
  
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);  
  
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))  
        {  
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);  
            request.downloadHandler = new DownloadHandlerBuffer();  
  
            request.SetRequestHeader("Content-Type", "application/json");  
            request.SetRequestHeader("Authorization", "Bearer " + authToken);  
  
            yield return request.SendWebRequest();  
  
            if (request.result == UnityWebRequest.Result.Success)  
            {  
                Debug.Log("Session ping / retention event logged: " + request.downloadHandler.text);  
            }  
            else  
            {  
                Debug.LogError("Error logging session: " + request.error);  
            }  
        }  
    }  
  
    /// <summary>  
    /// Send a purchase/revenue event to the "purchases" endpoint,   
    /// referencing an existing "game_install_id".  
    /// </summary>  
    public IEnumerator CreatePurchaseRecord(  
        string titleId,  
        string gameInstallId,  
        float purchaseAmount,  
        string currency,  
        string itemSku,  
        string itemName,  
        int quantity = 1  
    )  
    {  
        string endpoint = $"{baseUrl}titles/{titleId}/purchases";  
  
        string jsonBody = $@"{{  
  ""game_install_id"": ""{gameInstallId}"",  
  ""purchase_amount"": {purchaseAmount},  
  ""currency"": ""{currency}"",  
  ""item_sku"": ""{itemSku}"",  
  ""item_name"": ""{itemName}"",  
  ""quantity"": {quantity}  
}}";  
  
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);  
  
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))  
        {  
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);  
            request.downloadHandler = new DownloadHandlerBuffer();  
  
            request.SetRequestHeader("Content-Type", "application/json");  
            request.SetRequestHeader("Authorization", "Bearer " + authToken);  
  
            yield return request.SendWebRequest();  
  
            if (request.result == UnityWebRequest.Result.Success)  
            {  
                Debug.Log("Purchase record created: " + request.downloadHandler.text);  
            }  
            else  
            {  
                Debug.LogError("Error creating purchase record: " + request.error);  
            }  
        }  
    }  
}  
  
/// <summary>  
/// Optional: Classes to help you build and JSON-serialize   
/// 'fingerprint_components' from Unity system data.  
/// </summary>  
[System.Serializable]  
public class FingerprintComponents  
{  
    public DeviceInfo device;  
    public OSInfo os;  
    public DisplayInfo display;  
    public HardwareInfo hardware;  
    public EnvironmentInfo environment;
    public DesktopData desktop_data; 
}  
  
[System.Serializable]  
public class DeviceInfo  
{  
    public string model;  
    public string type;  
    public string manufacturer;  
}  
  
[System.Serializable]  
public class OSInfo  
{  
    public string name;  
    public string version;  
}  
  
[System.Serializable]  
public class DisplayInfo  
{  
    public string resolution;  
    public int density;  
}  
  
[System.Serializable]  
public class HardwareInfo  
{  
    public string cpu;  
    public int cores;  
    public string gpu;  
    public int memory;  
}  
  
[System.Serializable]  
public class EnvironmentInfo  
{  
    public string language;  
    public string timezone;  
    public string region;  
} 

[System.Serializable]  
public class DesktopData  
{  
    public string[] formFactors;       // e.g. ["Desktop"]  
    public string architecture;        // e.g. "x86" or "arm"  
    public string bitness;            // e.g. "64" if 64-bit  
    public string platformVersion;     // e.g. "10.0.22621" for Windows 11 build  
    public bool wow64;                // e.g. true if 32-bit process on 64-bit OS  
}  
