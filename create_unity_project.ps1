param(
    [Parameter(Mandatory=$true)]
    [string] $ProjectPath,
    [string] $UnityExe = $null,
    [switch] $RunUnity
)

# Si hay un venv en el directorio actual lo usamos
$venvPython = "$PSScriptRoot\.venv\Scripts\python.exe"
if (Test-Path $venvPython) {
    Write-Host "Usando python del venv: $venvPython"
    $pythonCmd = $venvPython
} else {
    # fallback al python del PATH
    $pythonCmd = "python"
}

$scriptPath = Join-Path $PSScriptRoot 'create_unity_project.py'

if (-not (Test-Path $scriptPath)) {
    Write-Error "No se encontró $scriptPath"
    exit 1
}

$args = @('--project-path', $ProjectPath)
if ($RunUnity) {
    if (-not $UnityExe) {
        Write-Error "Si especificas -RunUnity debes pasar -UnityExe <ruta a Unity.exe>"
        exit 1
    }
    $args += @('--unity-exe', $UnityExe, '--run-unity')
}

Write-Host "Ejecutando script: $pythonCmd $scriptPath $($args -join ' ')"
& $pythonCmd $scriptPath @args
