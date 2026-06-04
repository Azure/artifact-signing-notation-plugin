# Artifact Signing Notation Plugin

Artifact Signing Provider for the [Notation CLI](https://github.com/notaryproject/notation)

This Notation plugin allows you to use Microsoft's **Artifact Signing** service to sign and verify container images and other OCI artifacts. Signatures enable users to verify that these artifacts are from a trusted source and have not been tampered with since their release.

Please refer to [MS Learn Artifact Signing Documentation](https://learn.microsoft.com/en-us/azure/artifact-signing/) for more information on the service.

# Installation of the artifact signing plugin

1. Navigate to the [Releases](https://github.com/Azure/artifactsigning-notation-plugin/releases) page and choose a release of `notation-azure-artifactsigning`.
2. Download, verify, and then install the specified version of the plugin.

   **Automatic installation**:

   For Notation >= v1.1.0, please use [notation plugin install](https://github.com/notaryproject/notation/blob/v1.1.0/specs/commandline/plugin.md#notation-plugin-install) command to automatically install azure-artifactsigning plugin.

   For Linux amd64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.1/notation-azure-artifactsigning_1.1.1_linux_amd64.tar.gz --sha256sum 5ad180f8b88f5ac735db7b2ea937c3a57e41c4ae94f60f25227aa51a660cf0a3
   ```

   For Linux arm64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.1/notation-azure-artifactsigning_1.1.1_linux_arm64.tar.gz --sha256sum 029402c02c1d4ce85998f29539701e775785325bcd91c01c76e4f6beda90417c
   ```

   For Windows amd64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.1/notation-azure-artifactsigning_1.1.1_windows_amd64.zip --sha256sum 8a6552df310fc508a90b49c0d79f30c030808d18a0dca01c3077d301fa9542da
   ```

   For Windows arm64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.1/notation-azure-artifactsigning_1.1.1_windows_arm64.zip --sha256sum 5ccba7b8c9fb7f7653bdea8547568ea21177328a7bf3ba03d10d1d3e2ca02aa3
   ```

   For macOS amd64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.1/notation-azure-artifactsigning_1.1.1_darwin_amd64.tar.gz --sha256sum b97d6a83e0b5099e679cc9c1962771cb8e0ff525cc39f0dab170ca23243c4e51
   ```

   For macOS arm64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.1/notation-azure-artifactsigning_1.1.1_darwin_arm64.tar.gz --sha256sum 2ad442fe6d31f697bff2fb47d3a90cd428fc08bfecd29d121159469640f4da13
   ```
    **Manual installation**:

   For Linux Bash:

   ```bash
   version=1.1.1
   arch=amd64
   install_path="${HOME}/.config/notation/plugins/azure-artifactsigning"

   # download tarball and checksum
   checksum_file="notation-azure-artifactsigning_${version}_checksums.txt"
   tar_file="notation-azure-artifactsigning_${version}_linux_${arch}.tar.gz"
   curl -Lo ${checksum_file} "https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v${version}/${checksum_file}"
   curl -Lo ${tar_file} "https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v${version}/${tar_file}"

   # validate checksum
   grep ${tar_file} ${checksum_file} | sha256sum -c

   # install the plugin
   mkdir -p ${install_path}
   tar xvzf ${tar_file} -C ${install_path} notation-azure-artifactsigning
   ```

   For macOS Zsh:

   ```zsh
   version=1.1.1
   arch=amd64
   install_path="${HOME}/Library/Application Support/notation/plugins/azure-artifactsigning"

   # download tarball and checksum
   checksum_file="notation-azure-artifactsigning_${version}_checksums.txt"
   tar_file="notation-azure-artifactsigning_${version}_darwin_${arch}.tar.gz"
   curl -Lo ${checksum_file} "https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v${version}/${checksum_file}"
   curl -Lo ${tar_file} "https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v${version}/${tar_file}"

   # validate checksum
   grep ${tar_file} ${checksum_file} | shasum -a 256 -c

   # install the plugin
   mkdir -p ${install_path}
   tar xvzf ${tar_file} -C ${install_path} notation-azure-artifactsigning
   ```

   For Windows Powershell:

   ```powershell
   $version = "1.1.1"
   $arch = "amd64"
   $install_path = "${env:AppData}\notation\plugins\azure-artifactsigning"

   # download zip file and checksum
   $checksum_file = "notation-azure-artifactsigning_${version}_checksums.txt"
   $zip_file = "notation-azure-artifactsigning_${version}_windows_${arch}.zip"
   Invoke-WebRequest -OutFile ${checksum_file} "https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v${version}/${checksum_file}"
   Invoke-WebRequest -OutFile ${zip_file} "https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v${version}/${zip_file}"

   # validate checksum
   $checksum = (Get-Content ${checksum_file} | Select-String -List ${zip_file}).Line.Split() | Where-Object {$_}
   If ($checksum[0] -ne (Get-FileHash -Algorithm SHA256 $checksum[1]).Hash) {
      throw "$($checksum[1]): Failed"
   }

   # install the plugin
   mkdir ${install_path}
   Expand-Archive -Path ${zip_file} -DestinationPath ${install_path}
   ```

3. Run `notation plugin list` and confirm the `azure-artifactsigning` plugin is installed.

## Getting started

1. [Sign and verify an artifact with with a artifact signing certficate profile](docs/sign-and-verify.md)

## Registry support

This plugin supports signing and verifying artifacts in any OCI-compliant registry, see here for a list of [supported registries](https://notaryproject.dev/docs/registry-support/).  Some registries may have specific requirements or limitations regarding artifact signing.  Please refer to your registry's documentation for more information.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
