// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.Developer.ArtifactSigning.NotationPlugin.Constants;
using Azure.Developer.ArtifactSigning.NotationPlugin.Models;
using Azure.Developer.ArtifactSigning.NotationPlugin.Utilities;

namespace Azure.Developer.ArtifactSigning.NotationPlugin;

internal static class Key
{
    public static BaseResponse GenerateKeyResponse(KeyRequest? request)
    {
        if (!Validator.ValidateKeyRequest(request, out string errorMessage))
        {
            throw new NotationException(errorMessage, ErrorCodes.ValidationError);
        }

        // Currently, ArtifactSigning only supports 3072 bit RSA keys. We will always return RSA-3072 as keyspec until ArtifactSigning expands its offerings
        return new KeyResponse()
        {
            KeyId = request.KeyId,
            KeySpec = PluginConstants.SupportedKeySpec
        };
    }
}
