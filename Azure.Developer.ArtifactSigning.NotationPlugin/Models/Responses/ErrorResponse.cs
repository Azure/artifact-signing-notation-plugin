// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.Developer.ArtifactSigning.NotationPlugin.Models;

/// <summary>
/// Documentation: https://github.com/notaryproject/specifications/blob/main/specs/plugin-extensibility.md#plugin-contract
/// </summary>
internal class ErrorResponse : BaseResponse
{
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string>? ErrorMetadata { get; set; }
}
