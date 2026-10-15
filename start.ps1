# Устанавливает .NET 10 SDK (если его нет), собирает и запускает приложение.
# Запуск: двойной клик по start.cmd или в PowerShell: .\start.ps1

$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $MyInvocation.MyCommand.Path
$Project = Join-Path $Root "poly\poly.csproj"
$Url = "http://localhost:5115"

function Test-Sdk10 {
    $dotnet = Get-Command dotnet -ErrorAction SilentlyContinue
    if (-not $dotnet) { return $false }
    return [bool](& dotnet --list-sdks 2>$null | Select-String -Pattern "^10\.")
}

if (-not (Test-Sdk10)) {
    Write-Host ".NET 10 SDK не найден, устанавливаю..." -ForegroundColor Yellow
    $installed = $false

    if (Get-Command winget -ErrorAction SilentlyContinue) {
        winget install --id Microsoft.DotNet.SDK.10 -e --accept-source-agreements --accept-package-agreements
        $env:Path = [Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [Environment]::GetEnvironmentVariable("Path", "User")
        $installed = Test-Sdk10
    }

    if (-not $installed) {
        $installDir = Join-Path $env:LOCALAPPDATA "Microsoft\dotnet"
        $script = Join-Path $env:TEMP "dotnet-install.ps1"
        Invoke-WebRequest "https://dot.net/v1/dotnet-install.ps1" -OutFile $script
        & $script -Channel 10.0 -InstallDir $installDir
        $env:DOTNET_ROOT = $installDir
        $env:Path = "$installDir;$env:Path"
        [Environment]::SetEnvironmentVariable("Path", "$installDir;" + [Environment]::GetEnvironmentVariable("Path", "User"), "User")
    }

    if (-not (Test-Sdk10)) {
        throw "Не удалось установить .NET 10 SDK. Установите вручную: https://dotnet.microsoft.com/download/dotnet/10.0"
    }
}

$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_NOLOGO = "1"

Write-Host "SDK: $(& dotnet --version)" -ForegroundColor Green
Write-Host "Восстанавливаю пакеты и собираю проект..." -ForegroundColor Cyan
dotnet restore $Project
dotnet build $Project --no-restore -c Debug

Write-Host ""
Write-Host "Приложение: $Url" -ForegroundColor Green
Write-Host "Демо-вход:  hr@demo.ru / Demo123!" -ForegroundColor Green
Write-Host "Остановить: Ctrl+C" -ForegroundColor DarkGray
Write-Host ""
dotnet run --project $Project --no-build --launch-profile http
