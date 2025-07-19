# Car Obstacle Avoidance Game

## Project Overview
A Unity-based car simulation game where players must manually steer around obstacles while following a predefined waypoint path. The car moves automatically between waypoints, and players must use keyboard controls to avoid obstacles or face a game over scenario.

## Game Theme
This project follows the **"Limited Perspective"** theme from the assignment brief - players have limited control over the car's movement and must react quickly to obstacles with restricted steering capabilities.

## Features

### Core Gameplay
- **Automatic waypoint navigation**: Car follows a predefined path (Point A → B → C → D → A)
- **Manual obstacle avoidance**: Players must actively steer to avoid obstacles
- **Physics-based collision**: Realistic collision detection with solid obstacles
- **Game over mechanics**: Collision with obstacles stops the game
- **Restart functionality**: Press R key or click retry button to restart

### Controls
- **Spacebar**: Slow down the car
- **Spacebar + A**: Slow down and turn left (obstacle avoidance)
- **Spacebar + D**: Slow down and turn right (obstacle avoidance)
- **R Key**: Restart game after collision

### Technical Implementation

#### 1. Movement System
- Waypoint-based pathfinding system
- Physics-based movement using Rigidbody
- Smooth rotation and translation between waypoints

#### 2. Obstacle Avoidance
- Real-time obstacle detection using raycasting
- Manual steering controls for player intervention
- Visual warning system when obstacles are detected

#### 3. Collision Detection
- Physics-based collision using `OnCollisionEnter`
- Solid obstacles that prevent car passage
- Game state management for crash scenarios

#### 4. Audio System
- Dynamic engine sound based on car speed
- Pitch modulation reflecting movement speed
- Crash sound effects on collision

#### 5. Camera System
- Third-person camera following the car
- Smooth camera movement with configurable offset
- Behind-the-car perspective for optimal gameplay

#### 6. UI System
- World-space obstacle warnings
- Game over screen with retry functionality
- Real-time status indicators

## Project Structure

```
Assets/
├── Scripts/
│   ├── ManualSteeringAvoidance.cs    # Main car movement and collision logic
│   └── CameraFollow.cs               # Camera following system
├── Materials/
│   ├── AsphaltMaterial               # Road surface material
│   └── ObstacleMaterial              # Obstacle material
├── Models/
│   ├── RoadModel.fbx                 # Road/track 3D model
│   └── CarModel.fbx                  # Volkswagen car model
├── Audio/
│   ├── EngineSound.wav               # Car engine audio
│   └── CrashSound.wav                # Collision sound effect
└── Scenes/
    └── SampleScene.unity             # Main game scene
```

## Setup Instructions

### Prerequisites
- Unity 2022.3.45f1 or later
- Basic understanding of Unity Editor

### Installation
1. Clone this repository
2. Open the project in Unity
3. Load the `SampleScene`
4. Press Play to start the game

### Configuration
- **Waypoints**: Adjust waypoint positions in the ManualSteeringAvoidance component
- **Speed Settings**: Modify `moveSpeed` and `slowSpeed` values
- **Obstacles**: Position obstacles along the car's path
- **Camera**: Adjust camera offset in CameraFollow component

## Technical Requirements Fulfilled

### ✅ Assignment Criteria Met:
1. **Theme Implementation**: Limited perspective gameplay
2. **World Space UI**: Obstacle warnings and game over screen
3. **First Person Perspective**: Behind-the-car camera view
4. **Input System**: Keyboard controls (Spacebar, A, D, R)
5. **Audio System**: Engine sounds and collision effects
6. **Physics System**: Rigidbody-based movement and collision
7. **Game State Management**: Game over and restart functionality
8. **Project Organization**: Clean folder structure and naming conventions

### Game Mechanics
- **Pathfinding**: Automatic waypoint navigation
- **Obstacle Avoidance**: Manual steering system
- **Collision Detection**: Physics-based obstacle interaction
- **Performance Optimization**: Efficient collision detection and audio management

## Controls Reference

| Input | Action |
|-------|--------|
| Spacebar | Slow down car |
| Spacebar + A | Turn left (avoid obstacles) |
| Spacebar + D | Turn right (avoid obstacles) |
| R | Restart after collision |
| No Input | Auto-navigate (will hit obstacles) |

## Gameplay Flow

1. **Start**: Car begins at Point A
2. **Navigation**: Car automatically moves toward waypoints
3. **Obstacle Detection**: Warning appears when obstacle ahead
4. **Player Action**: Use Spacebar + A/D to steer around obstacles
5. **Collision**: Game over if obstacle is hit
6. **Restart**: Press R to restart from Point A

## Development Notes

### Key Components
- **ManualSteeringAvoidance.cs**: Main game logic, movement, and collision handling
- **CameraFollow.cs**: Camera system for following the car
- **Rigidbody Physics**: Ensures realistic movement and collision
- **Raycast Detection**: Early warning system for obstacles

### Performance Considerations
- Efficient obstacle detection using LayerMask
- Optimized audio system with pitch modulation
- Smooth interpolation for camera movement
- Physics-based movement for realistic behavior

## Future Enhancements
- Multiple difficulty levels
- Additional obstacle types
- Score system based on successful avoidance
- Multiple car models
- Enhanced visual effects
- Multiplayer support

## Credits
- **Development**: Aprilian Adha Eka Pasha (Personal Development)
- **Car Model**: Volkswagen Polo GTI Mk5
- **Course**: VR03 UAS Assignmnt
- **Institution**: Cakrawala University

---

*This project demonstrates fundamental game development concepts including physics simulation, user input handling, collision detection, and game state management within the Unity engine.*