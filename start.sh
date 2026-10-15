#!/usr/bin/env bash
# Устанавливает .NET 10 SDK (если его нет), собирает и запускает приложение.
# Запуск: ./start.sh
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT="$ROOT/poly/poly.csproj"
URL="http://localhost:5115"

has_sdk10() {
    command -v dotnet >/dev/null 2>&1 && dotnet --list-sdks 2>/dev/null | grep -q '^10\.'
}

if ! has_sdk10; then
    echo ".NET 10 SDK не найден, устанавливаю..."
    if [[ "$(uname)" == "Darwin" ]] && command -v brew >/dev/null 2>&1; then
        brew install dotnet
        export DOTNET_ROOT="$(brew --prefix)/opt/dotnet/libexec"
    else
        INSTALL_DIR="$HOME/.dotnet"
        curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
        bash /tmp/dotnet-install.sh --channel 10.0 --install-dir "$INSTALL_DIR"
        export DOTNET_ROOT="$INSTALL_DIR"
        export PATH="$INSTALL_DIR:$PATH"
        echo "Чтобы dotnet был доступен в новых терминалах, добавьте в ~/.zshrc или ~/.bashrc:"
        echo "  export DOTNET_ROOT=\"$INSTALL_DIR\"; export PATH=\"$INSTALL_DIR:\$PATH\""
    fi
    has_sdk10 || { echo "Не удалось установить .NET 10 SDK: https://dotnet.microsoft.com/download/dotnet/10.0"; exit 1; }
fi

if [[ -z "${DOTNET_ROOT:-}" && "$(uname)" == "Darwin" ]] && command -v brew >/dev/null 2>&1 && [[ -d "$(brew --prefix)/opt/dotnet/libexec" ]]; then
    export DOTNET_ROOT="$(brew --prefix)/opt/dotnet/libexec"
fi
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1

echo "SDK: $(dotnet --version)"
echo "Восстанавливаю пакеты и собираю проект..."
dotnet restore "$PROJECT"
dotnet build "$PROJECT" --no-restore -c Debug

echo
echo "Приложение: $URL"
echo "Демо-вход:  hr@demo.ru / Demo123!"
echo "Остановить: Ctrl+C"
echo
dotnet run --project "$PROJECT" --no-build --launch-profile http
