# Unity - Glitch Gaming API Integration

This repository contains an example C# script for **Unity** that communicates with the GlitchFun API to record game installs. The code is located in the `/src/ApiClient.cs` file.

## Overview

When a user installs and launches your Unity game, you can call the API to create a **"GameInstall"** record. This helps track installs, user devices, and platforms within your Glitch marketing funnel for your game.

## File Information

- **File Name**: `ApiClient.cs`
- **Location**: `/src/ApiClient.cs`
- **Language**: C#  
- **Unity Version**: 2018+ (though the code should work in most modern Unity versions)

## Installation

1. **Clone or copy** this repository into your Unity project's Assets folder (or a subfolder). For example: [YourUnityProject]/Assets/ApiClient/
2. Ensure you have **UnityEngine.Networking** (available by default in Unity 2019.x or via the UnityWebRequest package).
3. Open the `ApiClient.cs` file and update:
- `baseUrl` if needed (default is `https://api.glitch.fun/api/`).
- `authToken` with your actual Bearer token or a method to retrieve it from your auth system.

## Usage

1. Attach the `ApiClient` script to a **GameObject** in your Unity scene (or create a dedicated manager object).
2. From another script, call:

```csharp
StartCoroutine(
    apiClientInstance.CreateInstallRecord("TITLE_UUID", "DEVICE_UNIQUE_ID", "android")
);
```
- Replace "TITLE_UUID" with the actual Title ID from your GlitchFun system.
- Replace "DEVICE_UNIQUE_ID" with your user’s or device’s unique ID (must be consistent within your game).
- Replace "android" with the correct platform string (e.g. apple, steam, etc.).
3. Check the Unity Console for success or error messages.

```csharp
public class ExampleUsage : MonoBehaviour
{
    public ApiClient apiClient;

    void Start()
    {
        // Example usage after retrieving user's device ID or generating a random ID
        StartCoroutine(apiClient.CreateInstallRecord("TITLE_UUID_HERE", "SomeUniqueDeviceID123", "android"));
    }
}

```

### Contributing
Feel free to open issues or pull requests if you encounter bugs or want to add features.

### License
This sample code is provided under the MIT License. You’re free to use it in your Unity projects.
