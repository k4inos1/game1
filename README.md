Automatización Unity - Uso en Windows (PowerShell)

Este directorio contiene `create_unity_project.py`, un script que crea la estructura mínima
de un proyecto Unity 3D y varios scripts C# de ejemplo para un prototipo tipo "Vampire Survivors".

Archivos clave:
- create_unity_project.py - script Python que crea carpetas y archivos.
- create_unity_project.ps1 - helper PowerShell para ejecutar el script (si existe venv lo usa).

Requisitos:
- Python 3.x
- Unity Editor instalado y conocer la ruta a Unity.exe (ej. C:\\Program Files\\Unity\\Hub\\Editor\\<version>\\Editor\\Unity.exe)

Ejemplos de uso (PowerShell):

1) Sólo crear archivos (no lanzar Unity):

```powershell
.\create_unity_project.ps1 -ProjectPath "C:\Users\Ricardo\MyVampire3D"
# o
# python C:\Users\Ricardo\game1\create_unity_project.py --project-path "C:\Users\Ricardo\MyVampire3D"
```

2) Crear y lanzar Unity en modo batch para ejecutar el initializer:

```powershell
.\create_unity_project.ps1 -ProjectPath "C:\Users\Ricardo\MyVampire3D" -UnityExe "C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.18f1\\Editor\\Unity.exe" -RunUnity
# o
# python C:\Users\Ricardo\game1\create_unity_project.py --project-path "C:\Users\Ricardo\MyVampire3D" --unity-exe "C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.18f1\\Editor\\Unity.exe" --run-unity
```

Notas:
- El script crea sólo scripts y carpetas; Unity genera .meta y serializa assets cuando abre el proyecto.
- Si Unity falla en -batchmode revisa la consola y los logs.

Si quieres, puedo ejecutar el script para crear el proyecto en la ruta que indiques o añadir más scripts.