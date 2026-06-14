#!/usr/bin/env bash
set -euo pipefail

# regenerate.sh
# Generates C# DTOs from a JSON/OpenAPI file using NSwag.ConsoleCore.
# Defaults:
#  - input: ./cnbapi.json
#  - namespace: BitcoinMarketLoader.Application.Dtos.CnbApi
#  - output file: ./BitcoinMarketLoader.Application/Dtos/CnbApi/CnbApiDtos.cs

INPUT_FILE="${1:-./cnbapi.json}"
OUT_DIR="${2:-./BitcoinMarketLoader.Application/Dtos/CnbApi}"
OUT_FILE="$OUT_DIR/CnbApiDtos.cs"
NSWAG_LOCAL_TOOL="nswag"

echo "Input: $INPUT_FILE"
echo "Output: $OUT_FILE"

if [ ! -f "$INPUT_FILE" ]; then
  echo "ERROR: input file not found: $INPUT_FILE" >&2
  exit 2
fi

mkdir -p "$OUT_DIR"

# Ensure a dotnet local tool manifest exists and install NSwag.ConsoleCore locally if needed.
if ! dotnet tool list --local 2>/dev/null | grep -q NSwag.ConsoleCore; then
  echo "Creating dotnet tool manifest (if missing) and installing NSwag.ConsoleCore locally..."
  dotnet new tool-manifest --force >/dev/null
  dotnet tool install NSwag.ConsoleCore --version 13.* --local || dotnet tool install NSwag.ConsoleCore --local
fi

# Use the locally installed tool (./.config/dotnet-tools.json -> dotnet tool run nswag ...)
# Generate C# DTOs only (no client), using System.Text.Json attributes

# NSwag supports openapi2csclient which can generate DTO types from OpenAPI/JSON schema.
# Options used:
#  - GenerateDtoTypes: True
#  - GenerateClientClasses: False
#  - JsonSerializer: SystemTextJson
#  - Output: path
#  - Namespace: desired namespace

DOTNET_CMD=(dotnet tool run nswag)

echo "Running: ${DOTNET_CMD[*]} openapi2csclient /input:$INPUT_FILE /output:$OUT_FILE /namespace:BitcoinMarketLoader.Application.Dtos.CnbApi /GenerateDtoTypes:true /GenerateClientClasses:false /JsonSerializer:SystemTextJson"

"${DOTNET_CMD[@]}" openapi2csclient \
  /input:"$INPUT_FILE" \
  /output:"$OUT_FILE" \
  /namespace:BitcoinMarketLoader.Application.Dtos.CnbApi \
  /GenerateDtoTypes:true \
  /GenerateClientClasses:false \
  /JsonSerializer:SystemTextJson

rc=$?
if [ $rc -ne 0 ]; then
  echo "NSwag generation failed (exit $rc)" >&2
  exit $rc
fi

# Post-fix: ensure file compiles by adding using for System.Text.Json.Serialization if NSwag did not already
if ! grep -q "System.Text.Json.Serialization" "$OUT_FILE"; then
  sed -i '' '1s/^/using System.Text.Json.Serialization;\n/' "$OUT_FILE" || true
fi

echo "Generated DTOs: $OUT_FILE"
