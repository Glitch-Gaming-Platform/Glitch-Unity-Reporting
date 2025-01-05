# Unity - GlitchFun API Integration

This repository contains an example C# script for **Unity** that communicates with the GlitchFun API to record game installs. The code is located in the `/src/ApiClient.cs` file.

## Overview

When a user installs and launches your Unity game, you can call the API to create a **"GameInstall"** record. This helps track installs, user devices, and platforms within your Laravel backend (at `https://api.glitch.fun/api/`).

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
