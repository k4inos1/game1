Sugerencias y próximos pasos para el prototipo Vampire Survivors 3D

- Prefabs mínimos a crear en Unity:
  - Projectile: sphere/capsule con Rigidbody (isKinematic=false), Collider, y el script `Projectile.cs`.
  - Enemy prefab: capsule con `Enemy` script, collider y Rigidbody (o kinematic movement según prefieras).
  - Pickup prefabs: triggers con `Pickup` script.

- Gameplay:
  - Implementar salud del jugador y HUD.
  - Sistema de oleadas: aumentar spawnRate y dificultad con el tiempo.
  - Implementar upgrades por puntos recogidos o kills.

- Performance y calidad:
  - Pooling de proyectiles para evitar Instantiate/Destroy frecuentes.
  - Limitación de cantidad de enemigos activos y LOD si usas modelos complejos.

- Arte y VFX:
  - Partículas al matar enemigos, al recoger pickups y al disparar.
  - Sonidos para feedback (disparo, hit, muerte).

- Testing y CI:
  - Script de build (Editor script) y pipeline (GitHub Actions) para builds automáticos en Windows/Mac.

- Ideas avanzadas:
  - Sistema de armas con disparos en patrón (spread, piercing).
  - Tablero de mejoras persistente (PlayerPrefs o archivo JSON).
  - Soporte para gamepad con nuevo Input System.
