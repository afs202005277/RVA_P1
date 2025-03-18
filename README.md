# BaseDefense

BaseDefense is an augmented reality (AR) tower defense game that challenges players to protect a central base from waves of monsters. Utilizing Unity for game development and Vuforia for AR capabilities, the game incorporates strategic resource management, dynamic difficulty settings, and immersive AR-based gameplay.

Players can construct defenses, manage resources, and adjust defensive placements using AR markers. The game features both Medieval and Sci-Fi themes, allowing for diverse strategic experiences. By leveraging AR, players can position and reposition defenses by physically moving printed image markers, creating an interactive and engaging environment.

## Features
- **Augmented Reality Gameplay**: Use AR markers to position and manage defenses dynamically.
- **Thematic Customization**: Choose between Medieval and Sci-Fi aesthetics.
- **Strategic Resource Management**: Allocate funds for upgrading and constructing defenses.
- **Adaptive Monster AI**: Encounter a variety of monsters with unique behaviors and abilities.
- **Real-World Integration**: The game adjusts difficulty and spawns based on geographic location, weather, and time of day.

## Installation and Setup
### 1. Install the Game
1. Download the APK file.
2. Install the APK on your Android device.
3. Launch the BaseDefense app from your home screen or app menu.

### 2. Setting Up the AR Scene
1. **Position the Base Marker**: Place a pre-printed A4 paper base marker (featuring a castle image) on a flat surface.
2. **Initiate the Scene**: The game recognizes the base marker using Vuforia, spawning the castle base and surrounding terrain.
3. **Place Additional Defense Markers**:
   - Position printed markers (featuring towers or barriers) around the base marker.
   - Defenses will spawn at these locations and persist until the round ends.

## Game Mechanics
### Themes
- **Medieval**: Classic castles, towers, and knights defending against fantasy monsters.
- **Sci-Fi**: High-tech defenses, cyber bases, and futuristic weaponry.

### Economy System
- **Passive Income**: Earn money over time based on difficulty level.
- **Active Income**: Gain money by defeating monsters.
- **Spending**: Use resources to build and upgrade towers, barriers, and the base.

### Defensive Structures
- **Towers**: Attack incoming monsters with projectiles.
- **Base**: Evolves with upgrades, adding new defensive elements.
- **Barriers**: Delay enemies and provide soldier-based attacks.

### Monster Types
- **Zombie**: Standard enemy with balanced stats.
- **Lizard**: Fast-moving enemy with increased damage.
- **Ogre**: Slow but powerful.
- **Scavenger**: Night-time enemy that becomes visible only when attacking.
- **Chomper**: Fire-based monster that sets defenses ablaze.
- **Spitter**: Water-based enemy that stuns defenses.

### Difficulty Settings
- **Easy**: Lower monster HP and attack power, cheaper defenses.
- **Medium**: Balanced settings.
- **Hard**: High monster stats, expensive defenses, reduced income.

## External Data Integration
### Weather & Geographic Adaptation
- **Geographic Data**: Tracks the player's location to request local weather conditions.
- **Weather API**: Adjusts monster spawns based on real-time temperature and precipitation.

### Time-of-Day Adjustments
- **Day/Night Cycle**: The game adjusts lighting based on local sunset and sunrise times.
- **Monster Spawning**: Certain monsters appear more frequently at specific times (e.g., nocturnal enemies at night).

