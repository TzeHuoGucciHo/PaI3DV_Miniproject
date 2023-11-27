# Ultrakill "But Worse" FPS Game

## Overview

The goal of this project was to develop a high-octane first-person shooter, Ultrakill, "but worse". The player uses the WASD keyboard keys for movement, the mouse to look around, and the left mouse button to shoot enemies. The player's object is to survive and accumulate. 

## Core Parts

- **Player:** Move with WASD, jump with SPACEBAR.
- **First-Person Perspective:** Controlled by the mouse.
- **Shooting Mechanic:** Shoots in the direction the player is looking.
- **Enemies:** Track and deal damage to the player.
- **Arena:** Closed area where the player moves, and enemies spawn.

## Game Features

- Free horizontal and vertical movement.
- First-person shooting with a revolver.
- Random enemy spawn locations.
- Player tracking on enemies.
- Tracking of health, ammunition, and total score.

## Project Parts

### Scripts

- **BulletScript:** Handles bullet interactions with "Ground" and "Enemy" tags. Interacts with PlayerController's GainHealth() function.
- **EnemyController:** Manages enemies' player tracking using NavMesh and UnityEngine.AI.
- **EnemySpawner:** Handles enemy spawning and instantiation, uses raycasting to ensure enemies are only spawns on "Ground" tagged game objects.
- **PlayerController:** Controls player movement, camera orientation, collisions, UI, and scene management.
- **ShooterScript:** Manages shooting and bullet instantiation, including raycasting and revolver reload animation.

### Models and Prefabs

- Revolver model made with BlockBench.
- Player, enemy, and bullet models made with Unity primitives.

### Materials

- Basic Unity materials for bullet and enemy game objects.
- Revolver textures and materials created with BlockBench.

### Scenes

- Single-screen game.

### Testing

- Tested on Windows.

## Timetable

# Time spent in hours:

| Task                                  | Time (hours) |
| ------------------------------------- | ------------ |
| Setting up Unity, GitHub project      | 0.5          |
| Research and conceptualization       | 1            |
| Setting up Unity Input System         | 1            |
| Implementing basic movement           | 3            |
| Testing different movement methods   | 2.5          |
| Implementing first-person camera      | 1.5          |
| Testing jumping mechanics             | 3            |
| Implementing health system            | 6.5          |
| Revolver modeling                     | 0.5          |
| EnemySpawner implementation          | 0.5          |
| EnemyController implementation        | 0.5          |
| ShooterScript implementation          | 1.5          |
| BulletScript implementation           | 0.5          |
| Reload animation implementation       | 2            |
| UI including Death screen             | 0.5          |
| Final adjustments                     | 1            |
| Code commenting                       | 1.5          |
| Writing report                        | 1.5          |
| Writing README.txt                     | 0.5          |
| Creating presentation                  | 0.5          |
| **Total**                             | **32**       |
