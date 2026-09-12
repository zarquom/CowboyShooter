# Cowboy Shooter

A top-down 2D arcade shooter built in Unity. You play a cowboy fending off waves of horse-riding enemies and periodic bosses, collecting powerups, and chasing a high score.

## Requirements

- Unity **6000.4.8f1** (see `ProjectSettings/ProjectVersion.txt`)
- 2D URP project template dependencies (already configured in `Packages/manifest.json`)

## Getting Started

1. Open the project root folder in Unity Hub (this directory).
2. Open the `Boot` scene under `Assets/Scenes/Boot.unity`.
3. Press Play. `Bootstrap` registers core services and loads `Loading` → `MainMenu` → `Game` automatically; you don't need to open any other scene manually.

## Controls

| Action | Keyboard / Mouse | Gamepad |
|---|---|---|
| Move | `WASD` or Arrow Keys | Left stick / D-pad |
| Attack | `Space` or Left Mouse Button | West button (e.g. X / Square) |

Touchscreen tap is also supported for attack.

## Gameplay

- **Enemies**: Basic, Fast, and Strong horse riders spawn from the top of the screen with different movement patterns (straight dive, homing, zig-zag, orbit) and stronger enemies take more hits to kill.
- **Boss**: appears periodically (or once, at a fixed time, in Boss-Defeat mode) and takes multiple hits before going down.
- **Powerups**: dropped periodically — restore life, deal self-damage (risk pickup), or grant a temporary spread-shot.
- **Lives**: you have 3 lives; taking lethal damage costs a life and resets your health.
- **Win conditions** (configured per-build in the `GameVariablesSO` asset): reach a target score, survive/last until a time limit, or defeat the boss.
- **High scores**: saved locally via `PlayerPrefs` and shown on the Main Menu; currently only recorded for Time-mode runs (see `GameManager.GameFinished`).

## Project Structure

```
Assets/Scripts/
├── Core/           ServiceLocator (simple service locator for cross-scene singletons)
├── Data/           GameVariablesSO — single ScriptableObject holding designer-tunable balance values
├── Services/       IAssetLoader/AssetLoaderManager (Addressables-based loading), ISaveService/PlayerPrefsSaveService
├── Loading/        Bootstrap, LoadingPreloader, LoadingAnimation — scene bootstrap & asset preloading
├── Menu/           MainMenuManager, MenuPreloader, HighscoreEntry, MenuItemAnimation
├── Game/           GameManager, PlayerController, EnemyController, BossController, BulletController,
│                   PowerupController, EnemySpawnerManager, GameObjectsPoolManager, GameUIManager, LifeBarObject
├── Game/MovementStrategies/  ScriptableObject-based movement strategies (Homing, ZigZag, Orbit, DiveBomb)
├── Audio/          AudioManager
└── Editor/         PlayerPrefsSaveServiceWindow (Tools > PlayerPrefs Save Service Tester)
```

## Architecture Notes

- **Scene flow**: `Boot` → `Loading` → `MainMenu` → `Game`. Each phase preloads only the Addressables label it needs (`Menu`, then `Game`) via `IAssetLoader.PreloadLabelAsync`, so gameplay assets aren't pulled in until they're needed.
- **Service Locator**: `Bootstrap` registers `IAssetLoader` and `ISaveService` once at startup; they're resolved on demand via `ServiceLocator.GetService<T>()` instead of being wired through inspector references.
- **Data-driven balance**: `GameVariablesSO` centralizes tunable gameplay values (damage, spawn rates, scoring, bounds, timings) so designers can rebalance without touching code.
- **Movement Strategy pattern**: enemy/boss movement is implemented as swappable `MovementStrategy` ScriptableObjects (`HomingMovement`, `ZigZagMovement`, `OrbitMovement`, `DiveBombMovement`), decoupled from any `MonoBehaviour`.
- **Object pooling**: enemies, bullets, powerups, and the boss are all pooled via `UnityEngine.Pool.ObjectPool<T>` through `GameObjectsPoolManager` to avoid runtime allocation churn.

## Known Limitations

- High-score saving is intentionally limited to Time-mode runs (Points/Boss-Defeat scores aren't directly comparable on the same leaderboard).
- No automated test suite yet; `MovementStrategy` implementations and `PlayerPrefsSaveService` are pure/decoupled enough to be straightforward candidates for unit tests.

## AI disclosure

Used Claude in four cases:

1. **The correct use of Addressables**: The task was to load asynchronously and release correctly the assets that were going to be used in the game. Prompted Claude to give me some examples of correct use in similar projects. Reused some of the code given, to adapt it to the ServiceLocator (I had ServiceLocator from past personal projects) and loading assets for Menu and Game.
2. **An editor tool for testing Highscores and PlayerPrefs save service**: The task was getting an editor tool (mostly doing the UI conventions for editor). Prompted Claude directly to create the script for the tool, given some data about the save system. Used the script with some slight changes to test correctly some features of the highscores.
3. **Movement strategies for enemies**: The task was to implement a way to move different enemies in an extensible and decoupled way (there were some options, like inheritance of enemies, or different interfaces for movement, but i was not convinced). Prompted Claude directly asking a way to do that correctly, suggested to use Scriptable Objects for Movement Strategies and gave some scripts. Used the strategy and scripts with some slight modifications, the resolution of the movement math was helpful.
4. **General check of project**: The task was to check the project for bugs or architectural problems. Checked the project folder with Claude and gave the base data. It found some bugs that i fixed manually. Also helped setting some hardcoded numbers into GameVariables ScriptableObject, and drafting this Readme.
