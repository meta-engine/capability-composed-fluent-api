#!/usr/bin/env bash
set -euo pipefail

repository_root="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)"
test_root="$(mktemp -d "${TMPDIR:-/tmp}/capability-verifier-tests.XXXXXX")"
trap 'rm -rf "$test_root"' EXIT

for scenario in extra-error different-member valid-chain; do
    sample="$test_root/$scenario"
    mkdir -p "$sample/CompileContracts" "$sample/scripts"
    cp "$repository_root/"*.cs "$repository_root/"*.csproj \
        "$repository_root/global.json" "$repository_root/.editorconfig" "$sample/"
    cp "$repository_root/scripts/verify.sh" "$sample/scripts/"
    fixture="$sample/CompileContracts/InvalidEconomyBooking.cs"

    case "$scenario" in
        extra-error)
            sed '/        definition.Economy/i\
        definition.MissingMember();
' "$repository_root/CompileContracts/InvalidEconomyBooking.cs" >"$fixture"
            expected_message='The invalid example produced an unrelated compiler error.'
            ;;
        different-member)
            sed 's/WithCheckedBag/MissingMember/g' \
                "$repository_root/CompileContracts/InvalidEconomyBooking.cs" >"$fixture"
            expected_message='The invalid example did not fail with the expected CS1061 diagnostic.'
            ;;
        valid-chain)
            sed 's/WithCheckedBag/WithCabinBag/g' \
                "$repository_root/CompileContracts/InvalidEconomyBooking.cs" >"$fixture"
            expected_message='The intentionally invalid economy example compiled successfully.'
            ;;
    esac

    log="$sample/verification.log"
    if "$sample/scripts/verify.sh" >"$log" 2>&1; then
        cat "$log" >&2
        printf 'Verifier incorrectly accepted %s.\n' "$scenario" >&2
        exit 1
    fi

    if ! grep -Fq "$expected_message" "$log"; then
        cat "$log" >&2
        printf 'Verifier rejected %s for an unexpected reason.\n' "$scenario" >&2
        exit 1
    fi

    printf 'Verified rejection of %s.\n' "$scenario"
done
