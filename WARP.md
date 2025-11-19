# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

**Angry Grids** is a Unity multiplayer game combining physics-based slingshot mechanics with Tic-Tac-Toe. Players take turns launching birds at a 3x3 grid to claim squares. Built with Unity Netcode for GameObjects.

## Development Commands

### Opening the Project
```powershell
# Open Unity Editor (assumes Unity Hub is installed)
# The project uses Unity 2023.x with URP (Universal Render Pipeline)
# Open via Unity Hub or directly:
# C:\Program Files\Unity\Hub\Editor\<version>\Editor\Unity.exe -projectPath "C:\Users\dduran8801\Documents\GitHub\Accident-2"
```

### Building the Project
In Unity Editor:
- **File > Build Settings** (Ctrl+Shift+B)
- Select target platform (Standalone, Android, iOS)
- Click "Build" or "Build and Run"

### Testing Multiplayer
Since this is a networked game, testing requires running multiple instances:
1. Build the game executable
2. Run the built executable (acts as Host)
3. Run another instance from Unity Editor or build (acts as Client)
4. Host clicks "Host Game", Client clicks "Join Game"

### Running Tests
The project uses Unity Test Framework:
- **Window > General > Test Runner**
- Run tests from the Test Runner window
- Or via command line (if configured):
  ```powershell
  # Unity command line test execution
  Unity.exe -runTests -batchmode -projectPath "." -testResults results.xml
  ```

## High-Level Architecture

### Network Architecture

This is a **client-server multiplayer game** using Unity Netcode for GameObjects:

- **Server-authoritative gameplay**: All game state changes (turn switching, square claiming, win detection) are validated and executed on the server
- **NetworkVariables** synchronize state from server to all clients
- **ServerRpc** calls allow clients to request actions (e.g., launching bird, claiming square)
- **ClientRpc** calls push updates from server to all clients (e.g., visual updates, game over)

### Core Systems

#### 1. **TurnManager** (Assets/Scripts/TurnManager.cs)
The central authority managing game flow:
- Tracks current player turn (NetworkVariable)
- Validates player actions (only current player can launch)
- Manages turn switching after bird settles/hits ground/board
- Handles win/loss/draw detection
- Controls camera switching between player perspectives
- **Key Pattern**: Host is always Player 1, Client is always Player 2

#### 2. **SlingShotController** (Assets/Scripts/SlingShotController.cs)
Handles bird physics and input:
- Two-stage aiming: vertical drag → horizontal drag → launch
- Each player has their own slingshot (playerNumber=1 or playerNumber=2)
- Only active during that player's turn (controlled by TurnManager)
- Uses Rigidbody physics for bird trajectory
- **Key Pattern**: Launch request goes through ServerRpc → validated → ClientRpc executes for all players

#### 3. **TicTacToeBoard** (Assets/Scripts/TicTacToeBoard.cs)
Manages the 3x3 game board:
- Server-authoritative square claiming
- Win condition checking (3 in a row)
- Board state synchronized via NetworkVariables
- Spawns visual symbols (X/O) when squares are claimed

#### 4. **NetworkGameLauncher** (Assets/Scripts/NetworkGameLauncher.cs)
Handles lobby, connection, and game startup:
- Host/Client connection flow
- Spawns all NetworkObjects when game starts
- Manages UI state (menu → lobby → gameplay)
- Handles disconnection and return to menu

### Data Flow Example: Player Launches Bird

1. **Input** (Client): Player drags and releases on their SlingShotController
2. **Validation** (Client): SlingShotController checks if it's their turn via TurnManager
3. **Request** (Client→Server): `LaunchBirdServerRpc(pullVector, clientId)`
4. **Execute** (Server→All Clients): `ExecuteLaunchClientRpc(pullVector)` applies physics
5. **Notify** (Server): TurnManager's `OnBirdLaunchedServerRpc()` marks turn as "waiting for bird"
6. **Collision** (Any Client): Bird hits board/ground, triggers collision handler
7. **Claim/Reset** (Client→Server): Appropriate ServerRpc called (board hit or ground hit)
8. **Turn Switch** (Server→All Clients): TurnManager switches turn, updates UI

### Critical NetworkObject Requirements

All gameplay objects MUST have `NetworkObject` components:
- TurnManager
- Both SlingShotControllers (Player 1 & 2)
- TicTacToeBoard
- Optionally: TicTacToeSquares (if networked)

These must be:
1. Present in the scene OR spawned dynamically
2. Added to NetworkManager's prefab list (if spawned dynamically)
3. Spawned via `NetworkObject.Spawn()` before game starts

**Common Issue**: If NetworkObjects aren't spawned, players can't interact with the game. See QUICK_FIX_GUIDE.md and UNITY_SETUP_GUIDE.md for troubleshooting.

### Scene Structure

- **Menu/Lobby UI**: Connection buttons, player count display
- **Gameplay Objects**: Usually inactive until game starts (toggled by NetworkGameLauncher)
- **Cameras**: Each player has a camera; TurnManager activates the current player's camera
- **NetworkManager**: Unity Netcode's NetworkManager handles all networking (must be in scene)

## Key Unity Packages

- **com.unity.netcode.gameobjects** (2.5.1): Core multiplayer networking
- **com.unity.multiplayer.tools** (2.2.6): Debugging and profiling
- **com.unity.render-pipelines.universal** (17.0.3): Graphics rendering
- **com.unity.ugui** (2.0.0): UI system
- **com.unity.inputsystem** (1.11.2): Input handling
- **com.unity.services.vivox** (16.7.0): Voice chat (installed but may not be used)

## Common Development Patterns

### Adding a New Networked Feature

1. Determine authority: Server-authoritative (recommended) or client-predicted
2. Create NetworkVariable for state if needed
3. Create ServerRpc for client requests
4. Create ClientRpc for server broadcasts
5. Always validate on server before applying changes
6. Ensure all NetworkObjects are spawned before accessing them

### Debugging Network Issues

Common checks when something doesn't work:
1. Is the NetworkObject spawned? Check `IsSpawned` property
2. Is TurnManager.Instance not null and spawned?
3. Is it the correct player's turn? Check console logs from TurnManager
4. Are ServerRpc calls failing? Check ownership requirements
5. Review console logs—extensive debug logging throughout codebase

Enable verbose logging:
```csharp
Debug.Log($"Player {playerNumber}: Action details");
```

### Testing Locally

Two-instance testing on Windows:
1. Build the game (File > Build Settings > Build)
2. Run the built .exe (becomes Host)
3. Enter Play mode in Unity Editor (becomes Client)
4. Both connect, Host starts game

Alternative: Use Unity's Multiplayer Play Mode package (installed) for in-editor multi-instance testing.

## Project-Specific Conventions

- **Player numbering**: Host=1, Client=2 (assigned in TurnManager.OnNetworkSpawn)
- **Turn validation**: Always check `TurnManager.Instance.IsMyTurn()` before allowing input
- **Camera management**: Only one camera active at a time; TurnManager controls which
- **Error handling**: Try-catch blocks around NetworkRpc calls due to potential timing issues
- **NetworkVariable callbacks**: Subscribe in OnNetworkSpawn, unsubscribe in OnNetworkDespawn

## Important Files Reference

- **QUICK_FIX_GUIDE.md**: Troubleshooting common NetworkObject spawning issues
- **UNITY_SETUP_GUIDE.md**: Step-by-step Unity Inspector configuration
- **Assets/Scripts/**: All C# game logic
- **ProjectSettings/**: Unity project configuration
- **Packages/manifest.json**: Package dependencies

## Working with This Codebase

### Before Making Changes
1. Understand the network authority model (server validates, clients request)
2. Check if TurnManager or board state needs updating
3. Consider impact on both Host and Client perspectives

### When Adding Scripts
- Inherit from `NetworkBehaviour` if needs network functionality
- Call base methods: `base.OnNetworkSpawn()`, `base.OnNetworkDespawn()`
- Add NetworkObject component to GameObject in scene
- Register prefabs in NetworkManager if dynamically spawned

### When Modifying Network Code
- Test with both Host and Client roles
- Verify state synchronization across clients
- Check for race conditions during connection/spawning
- Use NetworkVariable for state, not direct field access across network
