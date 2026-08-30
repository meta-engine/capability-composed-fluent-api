#!/usr/bin/env bash
set -euo pipefail

repository_root="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)"
project="$repository_root/CapabilityComposedFluentApi.csproj"

dotnet restore "$project"
dotnet build "$project" --configuration Release --no-restore

expected_output=$'Economy MAD-LHR | seat 12A | cabin bags 1 | checked bags 0\nBusiness MAD-JFK | seat 2A | cabin bags 1 | checked bags 1'
actual_output="$(dotnet run --project "$project" --configuration Release --no-build)"

if [[ "$actual_output" != "$expected_output" ]]; then
    printf 'The valid example produced unexpected output.\nExpected:\n%s\nActual:\n%s\n' \
        "$expected_output" "$actual_output" >&2
    exit 1
fi

printf '%s\n' "$actual_output"

compile_log="$(mktemp "${TMPDIR:-/tmp}/capability-composed-fluent-api.XXXXXX")"
trap 'rm -f "$compile_log"' EXIT

set +e
dotnet build "$project" \
    --configuration Release \
    --no-restore \
    -p:CompileInvalidContract=true >"$compile_log" 2>&1
build_status=$?
set -e

cat "$compile_log"

if [[ $build_status -eq 0 ]]; then
    printf 'The intentionally invalid economy example compiled successfully.\n' >&2
    exit 1
fi

if ! grep -Fq "error CS1061" "$compile_log" ||
    ! grep -Fq "'IEconomyBookingBuilder' does not contain a definition for 'WithCheckedBag'" "$compile_log"; then
    printf 'The invalid example did not fail with the expected CS1061 diagnostic.\n' >&2
    exit 1
fi

if grep -F ": error " "$compile_log" | grep -Fv "error CS1061" >/dev/null; then
    printf 'The invalid example produced an unrelated compiler error.\n' >&2
    exit 1
fi

printf 'Verified the expected CS1061 compile contract.\n'
