# Build the Windows EXE

The Windows build entry point has been added at:

`Assets/Editor/BuildGame.cs`

The intended output is:

`Builds/Windows/MyFPS.exe`

## Important

The command-line build cannot run while the project is already open in Unity. Close the Unity Editor first, then run:

```bash
/Applications/Unity/Hub/Editor/6000.5.0f1/Unity.app/Contents/MacOS/Unity -quit -batchmode -nographics -projectPath /Users/tstore/Desktop/game/my-fps -executeMethod BuildGame.BuildWindows64 -customBuildPath Builds/Windows/MyFPS.exe -logFile Logs/build-windows.log
```

## If Unity says Windows support is missing

This Mac Unity install currently shows only `MacStandaloneSupport` under PlaybackEngines. If the build log says the Windows build target is missing, install the Windows Build Support module for Unity 6000.5.0f1 from Unity Hub, then run the command again.

## Files to share after a successful build

Share the whole `Builds/Windows` folder, not only the `.exe`. Unity Windows builds normally include the `.exe` plus required data folders and DLL files.
