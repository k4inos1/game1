"""
Automatiza la creación inicial de un proyecto Unity 3D y copia un conjunto mínimo
de scripts para un prototipo estilo "Vampire Survivors" en 3D.

Requisitos:
- Python 3.x
- Tener Unity Editor instalado y conocer la ruta a Unity.exe

Uso (PowerShell):
python .\create_unity_project.py --project-path "C:\Users\Ricardo\MyVampire3D" --unity-exe "C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.18f1\\Editor\\Unity.exe"

El script hace:
- Crea la carpeta del proyecto si no existe
- Crea carpetas Assets/Scripts y Assets/Editor y escribe ejemplos de C#
- Opcional: lanza Unity en modo batch para ejecutar el método estatico ProjectInitializer.Run

Notas:
- Este script no instala Unity. Ajusta la ruta a Unity.exe según tu instalación.
- Si no deseas lanzar Unity automáticamente, omite el argumento --run-unity.
"""
import argparse
import os
import shutil
import subprocess
import textwrap


SCRIPTS = {
    "Assets/Editor/ProjectInitializer.cs": r'''using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class ProjectInitializer
{
    // Este método es invocado desde la línea de comandos: -executeMethod ProjectInitializer.Run
    public static void Run()
    {
        // Crear una nueva escena
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Main Camera
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        camGO.tag = "MainCamera";
        camGO.transform.position = new Vector3(0, 10, -10);
        camGO.transform.LookAt(Vector3.zero);

        // Directional Light
        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        lightGO.transform.rotation = Quaternion.Euler(50, -30, 0);

        // Player
        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = Vector3.zero;
        player.AddComponent<PlayerController>();

        // GameManager
        var gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();

        // EnemySpawner
        var spawner = new GameObject("EnemySpawner");
        spawner.AddComponent<EnemySpawner>();

        // Guardar escena
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Main.unity");

        // Forzar compilación de assets
        AssetDatabase.Refresh();
        Debug.Log("ProjectInitializer: Escena y objetos iniciales creados.");
    }
}
''',

    "Assets/Scripts/PlayerController.cs": r'''using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;

    private float fireTimer = 0f;

    void Update()
    {
        // Movimiento simple WASD
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

        // Disparo automático
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null) return;
        var proj = Instantiate(projectilePrefab, transform.position + transform.forward, Quaternion.identity);
        var rb = proj.GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = transform.forward * 10f;
    }
}
''',

    "Assets/Scripts/Enemy.cs": r'''using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 1;

    Transform target;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null) target = player.transform;
    }

    void Update()
    {
        if (target == null) return;
        Vector3 dir = (target.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Destroy(gameObject);
    }
}
''',

    "Assets/Scripts/EnemySpawner.cs": r'''using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 15f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = new Vector3(circle.x, 0, circle.y);
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}
''',

    "Assets/Scripts/GameManager.cs": r'''using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }
}
''',

    "README.md": textwrap.dedent('''
# Automatización Unity - Prototipo Vampire Survivors 3D (esqueleto)

Este repositorio contiene un script en Python para crear un proyecto Unity básico
con scripts C# de ejemplo (Player, Enemy, Spawner, GameManager) y un `ProjectInitializer`
para configurar la escena inicial desde el Editor en modo batch.

Cómo usar (PowerShell):

1) Ajusta la ruta a tu Unity Editor (Unity.exe) y al proyecto objetivo.

```powershell
python .\create_unity_project.py --project-path "C:\Users\Ricardo\MyVampire3D" --unity-exe "C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.18f1\\Editor\\Unity.exe" --run-unity
```

Opciones útiles:
- --project-path: ruta donde crear el proyecto Unity
- --unity-exe: ruta completa a Unity.exe (Editor)
- --run-unity: si se especifica, invoca Unity en modo batch para ejecutar ProjectInitializer.Run

Limitaciones y notas:
- Este script solo prepara la estructura de archivos y los scripts C#. No instala paquetes ni assets.
- Para que `ProjectInitializer.Run` funcione en -batchmode debes tener la ruta correcta a Unity.exe.
- Unity puede tardar en importar assets la primera vez; revisa la consola de Unity para errores.

Siguientes pasos sugeridos:
- Añadir prefabs de projectile con Rigidbody y collider.
- Implementar sistema de upgrades, pickups, VFX y audio.
- Añadir tests y CI para builds automatizados.
''')
}


def ensure_dirs(base_path, rel_paths):
    for p in rel_paths:
        full = os.path.join(base_path, p.replace('/', os.sep))
        d = os.path.dirname(full)
        if d and not os.path.exists(d):
            os.makedirs(d, exist_ok=True)


def write_scripts(project_path):
    for rel, content in SCRIPTS.items():
        dest = os.path.join(project_path, rel.replace('/', os.sep))
        d = os.path.dirname(dest)
        if not os.path.exists(d):
            os.makedirs(d, exist_ok=True)
        with open(dest, 'w', encoding='utf-8') as f:
            f.write(content)


def create_project_structure(project_path):
    # Unity will create its own layout when launched, but create minimal folders
    folders = [
        'Assets/Scripts',
        'Assets/Editor',
        'Assets/Scenes',
        'ProjectSettings'
    ]
    ensure_dirs(project_path, folders)
    write_scripts(project_path)


def run_unity_batch(unity_exe, project_path, log_file=None):
    # Llama a Unity para ejecutar el método ProjectInitializer.Run
    cmd = [
        unity_exe,
        '-batchmode',
        '-nographics',
        f'-projectPath', project_path,
        '-executeMethod', 'ProjectInitializer.Run',
        '-quit'
    ]
    print('Ejecutando Unity en modo batch (esto puede tardar)...')
    try:
        subprocess.check_call(cmd)
    except subprocess.CalledProcessError as e:
        print('Unity devolvió error:', e)


def main():
    parser = argparse.ArgumentParser(description='Crear proyecto Unity y agregar scripts base')
    parser.add_argument('--project-path', required=True, help='Ruta donde crear el proyecto Unity')
    parser.add_argument('--unity-exe', required=False, help='Ruta a Unity.exe (Editor)')
    parser.add_argument('--run-unity', action='store_true', help='Lanzar Unity en modo batch para ejecutar el initializer')
    args = parser.parse_args()

    project_path = os.path.abspath(args.project_path)
    print('Proyecto:', project_path)
    if not os.path.exists(project_path):
        print('Creando carpeta de proyecto...')
        os.makedirs(project_path, exist_ok=True)

    create_project_structure(project_path)
    print('Estructura y scripts creados en', project_path)

    if args.run_unity:
        if not args.unity_exe:
            print('Error: --unity-exe es necesario para --run-unity')
            return
        unity_exe = os.path.abspath(args.unity_exe)
        if not os.path.exists(unity_exe):
            print('No se encontró Unity.exe en:', unity_exe)
            return
        run_unity_batch(unity_exe, project_path)


if __name__ == '__main__':
    main()
