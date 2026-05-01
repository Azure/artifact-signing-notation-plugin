# Copilot Instructions

## Project overview

This repo builds `notation-azure-artifactsigning`, a [Notation](https://github.com/notaryproject/notation) plugin (a single-file native executable) that signs OCI artifacts using Microsoft's Artifact Signing service. Notation invokes the binary as a subprocess, sends a JSON request on stdin, and expects a JSON response on stdout (or a JSON `ErrorResponse` on stderr with a non-zero exit code).

Implements the Notation [plugin contract](https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md). Three commands are dispatched from `Program.ProcessCommand`:

- `get-plugin-metadata` → `Metadata.GenerateMetadataResponse`
- `describe-key` → `Key.GenerateKeyResponse`
- `generate-signature` → `Sign.GenerateSignatureResponse`

Capability advertised: `SIGNATURE_GENERATOR.RAW`. Only RSA-3072 / SHA-384 / RSASSA-PSS-SHA-384 are supported (see `Constants/PluginConstants.cs`); validators reject anything else.

## Build, test, lint

Targets **.NET 10** with central package management (`Directory.Packages.props`). Add new package versions there, then reference them without a `Version` attribute in the `.csproj`.

```bash
make build                # dotnet build -c Release, single-file self-contained
make test                 # dotnet test with TRX logger + XPlat Code Coverage
```

Run a single test using dotnet (Makefile has no helper):

```bash
dotnet test Azure.Developer.ArtifactSigning.NotationPlugin.UnitTests \
  --filter "FullyQualifiedName~PluginUsageTests.MainMethod_OutputsSuccess_ForValidMetadataRequest"
```

Produce release artifacts (zip/tar.gz with renamed binary) using:

```bash
python3 ./scripts/build.py v<version> <runtime>   # runtimes: win-x64, win-arm64, linux-x64, linux-arm64, osx-x64, osx-arm64
```

`build.py` rewrites the assembly name to `notation-azure-artifactsigning` via `-p:AssemblyName=...` and stamps the version with `-p:Version=...`. Notation locates the plugin by binary name, so the assembly rename is required for produced artifacts.

CI (`.github/workflows/test-build.yml`) runs `make test` + `build.py` on Linux/macOS/Windows x64 and arm64.

## Key conventions

- **Internal-by-default**: nearly every type is `internal`. The test project gets access via `<InternalsVisibleTo>` in the main `.csproj` plus global `<Using>` entries in the test `.csproj` (so tests reference `PluginConstants`, `ErrorCodes`, model types without `using` statements).
- **Error handling**: throw `NotationException(message, ErrorCodes.*)` for any expected/validation failure. `Program.Main` catches it and emits a JSON `ErrorResponse` to stderr with exit 1. Anything else becomes a `GenericError`. Do not write to stderr directly — only the serialized `ErrorResponse` belongs there.
- **Validation pattern**: `Utilities/Validator.cs` uses `bool TryValidateXxx(request, out string errorMessage)` with `[NotNullWhen(true)]` so callers can dereference `request` after a successful check. Add new validation as private helpers chained inside `ValidateXxxRequest`.
- **JSON I/O**: requests come from a single line on stdin via `InputProvider.ReadJsonRequest`; serialization uses `JsonNamingPolicy.CamelCase` (set in `Program.Main`). New request/response models go under `Models/Requests` or `Models/Responses` and rely on this camelCase policy — do not add `[JsonPropertyName]` unless the wire name diverges.
- **Auth**: signing uses `DefaultAzureCredential` from `Azure.Identity`. `PluginConfig.ExcludeCredentials` is a comma-separated list validated against `CredentialOptions.SupportedTypes`; extending supported credentials means updating both that set and the `DefaultAzureCredentialOptions` mapping in `SignatureRequest.GetAzureCredential`.
- **Crypto**: actual signing is delegated to `ISignContext` from the `Azure.Developer.ArtifactSigning.CryptoProvider` package. Tests mock it via `Mocks/MockSignContext.cs`; production code constructs an `AzSignContext` in `SignatureRequest.GetAzSignContext` (made `virtual` specifically so tests can override it).
- **Version string**: `PluginConstants.ClientVersionString` is `"<assemblyName>::<major.minor.build>"`, computed via reflection. The assembly name is rewritten at publish time, so `VersioningTests` validates this format end-to-end — be careful changing `GetClientVersionString` or the `VersionDelimiter`.
- **Tests**: xUnit + Moq. Inherit from `TestBase` for canned valid request JSON/objects (`ValidKeyRequest`, `GetDefaultSignatureRequest`, etc.). Console-redirection helpers live in `PluginUsageTests` for end-to-end `Program.Main` tests.
- **License header**: every `.cs` file starts with `// Copyright (c) Microsoft Corporation.` / `// Licensed under the MIT License.` — keep it on new files.
