# Unity – GlitchFun API Integration  
  
This repository contains example scripts for **Unity** that communicate with the GlitchFun API. You'll learn how to:  
  
1. **Record a Game Install** for new users or devices    
2. **Log Retention Events** (session pings) at intervals    
3. **Send Fingerprinting Data** (optional but recommended)    
4. **Track Purchases** or revenue events    
  
By sending this data to GlitchFun, you can measure installs, sessions, cross-device attribution, and revenue metrics in one centralized analytics system.  
  
---  
  
## File Overview  
  
- **File Name**: `ApiClient.cs`    
- **Location**: `Assets/GlitchApiClient/ApiClient.cs` (example folder structure)    
- **Language**: C#    
- **Unity Version**: Works in 2018+ or any modern version with UnityWebRequest    
- **HTTP**: Uses `UnityWebRequest` for POST calls to the GlitchFun API    
  
---  
  
## 1. Installation & Setup  
  
1. **Import or Copy**    
   - Place `ApiClient.cs` into your Unity project (e.g. `Assets/GlitchApiClient/`).  
  
2. **Configure**    
   - Open `ApiClient.cs` and set `baseUrl` if your endpoint differs (default: `https://api.glitch.fun/api/`).  
   - Set `authToken` to your Bearer token or implement a method to retrieve it securely.  
  
3. **Attach**    
   - Attach the `ApiClient` script to a GameObject in your initial scene (or any “Manager” object).  
  
4. **Obtain Your Title ID**    
   - Log in to your GlitchFun dashboard, create or view your game (“title”), and get its **UUID**.  
  
---  
  
## 2. Creating a Game Install  
  
When a user first installs or launches your game, create a new **Game Install** record:  
  
```csharp  
public class ExampleUsage : MonoBehaviour  
{  
    public ApiClient apiClient;  
  
    void Start()  
    {  
        // Some unique device/user ID you generate or retrieve from your platform:  
        string userInstallId = "MY_UNIQUE_USER_ID_123";  
  
        // e.g., "android", "apple", "steam", "windows", "ps", "xbox", etc.  
        string platform = Application.platform.ToString().ToLower();   
        // Or choose a consistent string from your own logic.  
  
        // Call the API  
        StartCoroutine(apiClient.CreateInstallRecord(  
            "YOUR_TITLE_UUID",  // Title ID from GlitchFun  
            userInstallId,  
            platform  
        ));  
    }  
}
```
  
What does this do?

- If no install exists for that user_install_id, GlitchFun creates a new install record.
- If the same user_install_id is used again, GlitchFun registers a retention event (see next section).

## 3. Logging Retention Events (Session Pings)
To measure how often users return (retention), call the same endpoint again with the same user_install_id. The server sees it’s an existing install and automatically logs a GameRetention event:

```csharp
// Example: Send a "ping" every 5 minutes to track ongoing sessions  
private float retentionTimer;  
private float retentionInterval = 300f; // 5 minutes (300 seconds)  
  
void Update()  
{  
    retentionTimer += Time.deltaTime;  
    if (retentionTimer >= retentionInterval)  
    {  
        retentionTimer = 0f;  
        StartCoroutine(apiClient.LogSessionActivity(  
            "YOUR_TITLE_UUID",  
            "MY_UNIQUE_USER_ID_123",  
            "windows",   
            "SESSION_ABC_123"   // optional session ID  
        ));  
    }  
}
```

## 4. Sending Optional Fingerprint Data

GlitchFun uses fingerprint data to help unify analytics across platforms (web → PC → console → mobile). On Unity, you can gather some hardware and environment details from SystemInfo or Application, then pass them as a JSON string to the ApiClient methods:

```csharp
// Minimal example snippet  
private string CollectFingerprintData()  
{  
    // For demonstration: build a JSON with standard keys recognized by GlitchFun  
    FingerprintComponents comps = new FingerprintComponents  
    {  
        device = new DeviceInfo  
        {  
            model = SystemInfo.deviceModel,        
            type  = SystemInfo.deviceType.ToString().ToLower(),  
            manufacturer = "Unknown"  
        },  
        os = new OSInfo  
        {  
            name    = SystemInfo.operatingSystem,  
            version = "10.0.19042" // parse from operatingSystem if you want  
        },  
        display = new DisplayInfo  
        {  
            resolution = Screen.currentResolution.width + "x" + Screen.currentResolution.height,  
            density    = 96 // approximate for desktop  
        },  
        hardware = new HardwareInfo  
        {  
            cpu     = SystemInfo.processorType,  
            cores   = SystemInfo.processorCount,  
            gpu     = SystemInfo.graphicsDeviceName,  
            memory  = SystemInfo.systemMemorySize  
        },  
        environment = new EnvironmentInfo  
        {  
            language = Application.systemLanguage.ToString(),  
            timezone = "Unknown",  
            region   = "XX"  
        }  
    };  
  
    return JsonUtility.ToJson(comps);  
}  
  
void Start()  
{  
    // ...  
    string fingerprintJson = CollectFingerprintData();  
    StartCoroutine(apiClient.CreateInstallRecord(  
        "YOUR_TITLE_UUID",  
        "MY_UNIQUE_USER_ID_123",  
        "windows",  
        fingerprintJson // optional  
    ));  
}  
```

## 5. Recording Purchases (Revenue Events)

To track user purchases (in-app transactions, DLC, etc.), call the purchases endpoint with the relevant data:

```csharp
StartCoroutine(apiClient.CreatePurchaseRecord(  
    "YOUR_TITLE_UUID",  
    "installUUIDFromServer",  // The ID you got from the "GameInstall" object  
    4.99f,  
    "USD",  
    "Booster_Pack_1",  
    "In-app booster pack",  
    1  
));  
```


