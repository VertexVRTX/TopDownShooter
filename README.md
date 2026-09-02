# Arena Survival Demo (Unity)

A wave-based top-down shooter with dual weapons, dash evasion, explosive hazards, and NavMesh-driven enemy AI. Built to answer: **how do you make 20+ NavMesh agents feel like they're coordinating without actually writing squad AI?**

<img width="600" height="337" alt="ezgif com-video-to-gif-converter" src="https://github.com/user-attachments/assets/e3d4ad1a-6b3c-4d37-b3ed-1a8159f8cdf6" />

---

## Quick Facts

| Category | Details |
|----------|---------|
| **Genre** | Wave-based arena shooter |
| **Enemies per wave** | Scales from 5 to 25+ |
| **AI** | Unity NavMeshAgent + dynamic obstacle carving |
| **Weapons** | 2 (Assault Rifle, Shotgun unlocked after Wave 5 boss) |
| **Special mechanics** | Dash (collision ignore), explosive barrels, shields |
| **Platform** | WebGL (itch.io) + Standalone |

---

## Gameplay

Survive endless waves of enemies. Every 5th wave — a Mega Boss with high HP and melee damage.

- **Shoot** with LMB, **aim** with mouse
- **Dash** with Shift/RMB to escape crowds (phases through enemies)
- **Switch weapons** with Q after defeating the first boss
- **Explosive barrels** damage both player and enemies
- **Power-ups:** Health, Ammo, temporary invulnerability shield

---

## Architecture

```text
GameManager (Singleton)
├── WaveManager      → wave counter, difficulty scaling, boss trigger
├── EnemySpawner     → spawn points, enemy pooling, wave-based spawn tables
├── PlayerController → movement, aim, shoot, dash, weapon switch
├── WeaponSystem     → 2 weapons with independent ammo, reload logic
├── HealthSystem     → player + enemy HP, damage events, death
├── PowerUpManager   → random spawn timers, pickup effects
├── BarrelSystem     → explosive barrels with radius damage
└── UIManager        → HUD, menus, event-driven updates

Enemy (NavMeshAgent)
├── EnemyAI          → SetDestination to player, attack range check
├── NavMeshObstacle  → dynamic carving (enabled when enemy stops moving)
└── HealthSystem     → shared component

Weapon
├── AssaultRifle     → high fire rate, medium damage, 30-round mag
└── Shotgun          → wide spread, high damage, 8-round mag, slow reload
```

---

## Visual Showcases

<details>
<summary>Click to expand: description and demonstration</summary>
  
### Dual Weapon & UI Switching
> Toggle weapons using the **Q** key. Ammunition counts persist between switches.

<img width="800" height="437" alt="ezgif com-video-to-gif-converter (1)" src="https://github.com/user-attachments/assets/e71859e4-0ef5-48aa-96c2-e3352cc172e0" />

### Tactical Dash Escape & Advanced Enemy AI
> Use the dash to escape deadly tight traps, gliding straight through crowds while the smart enemy AI attempts to encircle you.

<img width="800" height="450" alt="ezgif com-video-to-gif-converter (2)" src="https://github.com/user-attachments/assets/55160f7a-b345-4d38-b866-ff5ac41e72ed" />

### Strategic Hazards & Shields
> Detonate explosive barrels to wipe out groups of enemies, or grab the Energy Shield to activate a protective glowing dome.

<img width="800" height="450" alt="ezgif com-video-to-gif-converter (2)" src="https://github.com/user-attachments/assets/2dbe4efd-77a5-474e-a1e8-ad8877ac8c9e" />

### Mega Boss Wave
> Survived 4 waves? Face the towering, slow-moving Mega Boss with high HP and devastating melee damage.

<img width="800" height="450" alt="ezgif com-video-to-gif-converter (3)" src="https://github.com/user-attachments/assets/5926c383-c312-480c-80ea-a33eadf8a168" />

</details>

---

## Key Systems

### Wave System
- **Scaling:** enemy count per wave = `baseCount + waveIndex * multiplier`. Speed increases by 5% per wave.
- **Boss every 5 waves:** Mega Boss spawns with `health = baseHP * waveIndex`. Melee attack only, slow movement.
- **Spawn points:** 4 points around arena edge. `EnemySpawner` picks the one farthest from the player to prevent spawn-camping.

### Enemy AI (NavMesh + Crowd Avoidance)
- **Base behavior:** `NavMeshAgent.SetDestination(player.position)`. Attack when in range.
- **Crowd logic:** Each enemy has a `NavMeshObstacle` component with `carving = true`. When an enemy stops to attack, it carves a hole in the NavMesh, forcing other enemies to path around instead of stacking on top of each other. This creates the illusion of flanking/surrounding without any squad coordination code.
- **Performance:** With 20+ agents, `NavMeshAgent` updates are expensive. I set `NavMeshAgent.updateRotation = false` and handle facing manually in `Update()` to reduce per-agent overhead.

### Dash System
- **Implementation:** On dash start, `Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true)` for 0.3s. Player gets `+speed` boost via `CharacterController.Move()`. After 0.3s, collision is re-enabled.
- **Visual:** Trail renderer on the player model.
- **Trade-off:** `IgnoreLayerCollision` is global — during dash, the player can't collide with *any* enemy, even if they'd want to. For a single-player game, acceptable.

### Weapon System
- **Ammo tracking:** independent `currentAmmo / maxAmmo` per weapon. `WeaponSystem` handles reload coroutine and fire-rate cooldown.
- **Switching:** Press Q to toggle. `UIManager` updates ammo display via `OnWeaponSwitched` event.
- **Shotgun spread:** 5 raycasts in a cone (`Quaternion.Euler(0, angle, 0) * forward`), each with independent damage falloff by distance.

### Explosive Barrels
- `Barrel` has `OnTriggerEnter` for bullets. On hit: `Physics.OverlapSphere` for explosion radius → applies damage to all `IDamageable` in range.
- `NavMeshObstacle` on barrel so enemies path around it, but players can kite enemies near it for strategic explosions.

---

## Design Decisions

### Why NavMeshObstacle carving instead of RVO (Reciprocal Velocity Obstacles)?
Unity's built-in RVO (via `NavMeshAgent.avoidanceType`) works for small crowds but breaks down at 15+ agents — agents start jittering and spinning in place. `NavMeshObstacle` with carving is heavier on the NavMesh rebuild, but at 20 agents it's more stable. I set `carvingTimeToStationary = 0.5s` so obstacles only carve when an enemy actually stops, not while moving.

### Why CharacterController instead of Rigidbody for the player?
Tried `Rigidbody` first. Felt "floaty" — knockback from enemy collisions was unpredictable, and dash through enemies required fighting the physics solver. `CharacterController` gives explicit control over movement via `Move()`, and `IgnoreLayerCollision` works cleanly. Trade-off: no physics-based knockback, but movement is snappy and predictable.

### Why two weapons instead of a progression tree?
A full weapon unlock system (5+ guns) would require balancing DPS, fire rate, reload speed, and ammo economy across 10+ variables. For a portfolio prototype, two weapons with distinct roles (AR = sustained DPS, Shotgun = burst/crowd) prove the system works without scope creep. The `WeaponSystem` is built to accept N weapons via a `List<WeaponData>` ScriptableObject — adding more is trivial.

### Why explosive barrels damage both sides?
Initially barrels only damaged enemies. Felt like a free win — players just kited enemies into barrels with zero risk. Adding self-damage forces the player to time explosions and creates tension. Game design lesson: friendly fire makes mechanics deeper.

### Why no object pooling for enemies?
This project predates my pooling work (see Box Sort). Enemies are `Instantiate`/`Destroy` per wave. At 20 enemies max, GC spikes are visible but brief (~40ms every 60s). For production, I'd pool them. I kept it as-is to show the evolution of my approach across projects.

---

## What I Learned

- **NavMesh carving has a cost:** With 20 enemies and barrels all carving simultaneously, NavMesh rebuilds caused 5ms spikes. Fixed by disabling carving on moving enemies (`carving = false` while `NavMeshAgent.hasPath`) and only enabling it when they stop to attack.
- **Dash through enemies feels good, but breaks AI:** When the player dashes through a crowd, enemies behind lose their target briefly and spin in place. Fixed by increasing `NavMeshAgent.acceleration` and `angularSpeed` so they reorient faster after the player reappears.
- **Shotgun spread needs falloff:** First version had all 5 pellets do full damage at any range. Shotgun became the optimal weapon at all distances. Added damage falloff (`damage * (1 - distance/maxRange)`) — now it's dominant close-range, AR wins at distance.
- **Wave scaling needs caps:** Early version had linear enemy count growth (`wave * 3`). By wave 10 there were 30 enemies and the game became unplayable. Added soft cap at 25 enemies and shifted difficulty to speed/HP instead of raw count.
- **UI clickthrough is easy to forget:** First build had players shooting when clicking the pause menu. `EventSystem.current.IsPointerOverGameObject()` check in `PlayerController` fixed it. Now a habit in every project.

---

## Tech Stack

- Unity 2022.3 LTS
- Universal Render Pipeline (URP)
- C#
- Unity NavMesh Components
- Unity UI (uGUI) + EventSystem
- TextMeshPro

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

## How to Run

### Run the Code in Unity
1. Open project in Unity (2022.3+ recommended)
2. Open the MainMenu Scene
3. Press Play

### Play in Browser (Quick Demo)
You can play the fully functional WebGL demo directly in your browser without downloading anything:
**[Play Arena Survival Demo on itch.io](https://vertexvrtx.itch.io/topdownshooter)**
