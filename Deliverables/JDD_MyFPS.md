# JDD - My FPS

## 1. Project Information

Game title: My FPS

Genre: First-person shooter

Engine: Unity 6000.5.0f1

Target platform: Windows PC

Build name: MyFPS.exe

Project base: Unity FPS Microgame, modified with custom difficulty, arena visuals, enemy balancing, and menu styling.

## 2. Game Concept

My FPS is a compact first-person shooter where the player enters a neon cyberpunk arena and must eliminate all enemies to win. The gameplay keeps the original FPS shooting, movement, health, pickups, enemy detection, and objective mechanics, but the presentation and difficulty structure have been redesigned.

The player chooses Easy, Medium, or Hard from the menu before entering the arena. Each difficulty changes enemy count, turret count, health pickups, enemy health, and fire rate pressure.

## 3. Target Audience

The game is designed for students and casual FPS players who understand basic keyboard and mouse controls. It is short, clear, and arcade-like, making it suitable for a class demo or small project submission.

## 4. Core Gameplay Loop

1. Start from the menu.
2. Select a difficulty level.
3. Spawn into the cyberpunk arena.
4. Move through the arena, collect health pickups, and fight enemies.
5. Destroy all enemies to complete the objective.
6. Reach the win screen or lose if the player dies.

## 5. Player Controls

Movement: WASD or arrow keys

Look: Mouse

Fire: Left mouse button

Jump: Space

Sprint: Left Shift

Crouch: C

Weapon select: Number keys

Pause/Menu: Escape

## 6. Objective

The main objective is to eliminate all active enemies in the arena. The objective counter updates during gameplay and shows how many enemies have been defeated out of the total required amount.

## 7. Difficulty Design

Easy:

- Hoverbot enemies: 3
- Turret enemies: 1
- Total enemies: 4
- Health pickups: 4
- Enemy health multiplier: 0.9
- Enemy fire interval multiplier: 1.15
- Purpose: Best for first-time players. Fewer enemies and more healing.

Medium:

- Hoverbot enemies: 6
- Turret enemies: 3
- Total enemies: 9
- Health pickups: 2
- Enemy health multiplier: 1.0
- Enemy fire interval multiplier: 1.0
- Purpose: Balanced default challenge.

Hard:

- Hoverbot enemies: 9
- Turret enemies: 4
- Total enemies: 13
- Health pickups: 1
- Enemy health multiplier: 1.2
- Enemy fire interval multiplier: 0.85
- Purpose: Highest pressure. More enemies, less healing, stronger enemies, and faster enemy attacks.

## 8. Enemies

Hoverbot:

- Mobile enemy type.
- Moves through the arena and attacks the player.
- Used in all difficulty levels.

Turret:

- Heavy stationary enemy type.
- Scaled larger to look like a stronger enemy.
- Uses aiming and shooting behavior.
- Runtime height pinning prevents the turret from being pulled below the redesigned arena floor by old navigation data.

## 9. Pickups

Health pickups are placed in the arena to help the player survive. The number of pickups depends on difficulty, so harder modes give the player fewer recovery opportunities.

## 10. Level and Atmosphere

The arena has been visually rebuilt into a brighter cyberpunk combat space. The old level geometry is replaced at runtime with:

- Dark sci-fi floor panels
- Neon cyan and magenta guide lines
- North, south, east, and west arena walls
- Ceiling beams
- Raised platforms
- Cover blocks
- Holographic billboard-style props
- Colored point lights
- Fog and ambient lighting

The goal of the new atmosphere is to make the game feel more modern, readable, and interesting while keeping the original FPS mechanics stable.

## 11. User Interface

The menu includes styled difficulty buttons for Easy, Medium, and Hard. The visual style uses darker button backgrounds, bright accent colors, outlines, and shadows to better match the neon arena theme.

The in-game UI keeps the original FPS information structure: objective text, health, weapon/ammo feedback, and game result screens.

## 12. Technical Implementation

Main custom scripts:

- DifficultySettings.cs: Saves and loads the selected difficulty.
- LoadSceneButton.cs: Builds the difficulty selection menu and loads the selected scene.
- DifficultyLevelController.cs: Spawns enemies and pickups based on difficulty.
- CyberpunkArenaBuilder.cs: Rebuilds the level atmosphere and geometry at runtime.
- PinnedSpawnHeight.cs: Keeps large turret enemies above the redesigned arena floor.
- BuildGame.cs: Provides a command-line Windows build entry point.

Main scenes:

- IntroMenu: Main menu and difficulty selection.
- MainScene: Gameplay arena.
- WinScene: Victory screen.
- LoseScene: Defeat screen.

## 13. Win and Lose Conditions

Win condition: The player destroys all required enemies.

Lose condition: The player health reaches zero.

## 14. Testing Notes

Tested script compilation after gameplay and packaging changes. The large turret enemy placement was corrected so it stays above the arena floor. Full Play Mode testing should be done inside the Unity Editor before final submission.

## 15. Future Improvements

- Add unique models or downloaded Unity Asset Store props.
- Add more weapon skins.
- Add sound changes for the cyberpunk style.
- Add a second arena or wave-based mode.
- Add a scoreboard or timer for replay value.
