# Arena Survival Demo (Unity)

A fast-paced, 3D Top-Down Shooter built with Unity. Face endless waves of smart enemies, manage resources, unlock powerful weapons, and defeat giant bosses in a highly dynamic arena environment. Optimized for smooth gameplay and commercial-grade polish.

## Key Features

### Core Gameplay & Mechanics
* **Dynamic Wave System**: Automated spawning architecture where enemy count and speed scale with each wave. Every 5th wave spawns a challenging Mega Boss.
* **Dual Weapon Arsenal**: Smart inventory tracking. Start with a rapid-fire Assault Rifle and unlock a wide-spread Shotgun after defeating the first Boss. Ammunition is accurately tracked independently for each weapon.
* **Tactical Dash Escape**: A high-mobility evasion mechanic mapped to `Shift` and `Right Click`. Temporarily completely disables player collisions, allowing the player to phase *through* enemy ranks with an artistic trailing effect.
* **Interactive Hazards**: Tactical explosive barrels spawn periodically, damaging both the player and enemies caught in the blast radius.
* **Power-Up System**: Randomly spawning survival pickups, including Health Packs, Ammo Crates, and a glowing Energy Shield providing temporary invulnerability.

### Controls & Polish
* **Dynamic Crosshair**: Custom hardware cursor that seamlessly tracks the 3D environment and turns red upon targeting enemies.
* **Smart NavMesh AI & Custom Crowd Logic**: Built using a dynamic carving `NavMeshObstacle` system. Enemies realistically flank, surround, and attempt to outmaneuver the player rather than just mindlessly stacking on top of them. Player coordinates are immune to external physics forces.
* **UI Anti-Clickthrough**: Production-ready event system preventing weapon fire or aiming logic when interacting with menus.
* **Visual Juice & 3D Audio**: Enhanced with a custom URP Post-Processing profile featuring neon Bloom, high-contrast Color Grading, and artistic Vignette, combined with spatial 3D audio and custom randomized audio queues for enemy vocals.

---

## Visual Showcases

### Dual Weapon & UI Switching
> Toggle weapons using the **Q** key. Ammunition counts persist between switches.

![Weapon Switching Demo]

### Tactical Dash Escape & Advanced Enemy AI
> Use the dash to escape deadly tight traps, gliding straight through crowds while the smart enemy AI attempts to encircle you.

![Dash and AI Flanking Demo]

### Strategic Hazards & Shields
> Detonate explosive barrels to wipe out groups of enemies, or grab the Energy Shield to activate a protective glowing dome.

![Hazards and Shield Demo]

### Mega Boss Wave
> Survived 4 waves? Face the towering, slow-moving Mega Boss with high HP and devastating melee damage.

![Mega Boss Fight]

---

## Controls

| Action | Input |
| :--- | :--- |
| **Move** | `W`, `A`, `S`, `D` / Arrow Keys |
| **Aim** | Mouse Movement |
| **Shoot** | `Left Mouse Button (Fire1)` |
| **Tactical Dash** | `Left Shift` / `Right Mouse Button (Fire2)` |
| **Switch Weapon** | `Q` (After Wave 5) |
| **Menus** | `UI Interaction` |

---

## Tech Stack & Architecture

* **Engine**: Unity 2022+ (Universal Render Pipeline - URP)
* **Language**: C# (Object-Oriented Programming, C# Actions/Events for UI-to-Spawner decoupled communication)
* **AI Navigation**: Unity NavMesh Components with dynamic Carving Obstacles
* **UI System**: Unity UI (UGUI) with EventSystem integrations

---

## How to Run

### Run the Code in Unity
1. Open project in Unity (2022.3+ recommended)
2. Open the MainMenu Scene
3. Press Play

### Play in Browser (Quick Demo)
You can play the fully functional WebGL demo directly in your browser without downloading anything:
**[Play Endless Runner Demo on itch.io](https://vertexvrtx.itch.io/endless-runner-demo)**
