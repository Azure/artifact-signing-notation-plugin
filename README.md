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
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.2/notation-azure-artifactsigning_1.1.2_linux_amd64.tar.gz --sha256sum aab5dc0509188e213fdd7e1166832d071c3b9071aab8aa216fa4473cb9da4ebc
   ```

   For Linux arm64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.2/notation-azure-artifactsigning_1.1.2_linux_arm64.tar.gz --sha256sum 25192eb6af30b5f85cc9ad4579f9da10281e3dd6c55eb6bcb06614053de974f5
   ```

   For Windows amd64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.2/notation-azure-artifactsigning_1.1.2_windows_amd64.zip --sha256sum 49b22bd0986c7947d2192c72e290f1432e72c43a160889fcc4d3d0bb1b1f1983
   ```

   For Windows arm64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.2/notation-azure-artifactsigning_1.1.2_windows_arm64.zip --sha256sum 750c7d17e8afb10b342ee5805ceb61b46dae12af4cc83a20c015b7598d6e87c0
   ```

   For macOS amd64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.2/notation-azure-artifactsigning_1.1.2_darwin_amd64.tar.gz --sha256sum 3dede080fb87c8dd1295f3f32febf349eb7128d0e9b5ac349f643a24b313e290
   ```

   For macOS arm64:

   ```
   notation plugin install --url https://github.com/Azure/artifact-signing-notation-plugin/releases/download/v1.1.2/notation-azure-artifactsigning_1.1.2_darwin_arm64.tar.gz --sha256sum d42ceeb20ab1e67194012d09329c6fbc5993ce8f09ee433eab189a2072c07fb6
   ```
    **Manual installation**:

   For Linux Bash:

   ```bash
   version=1.1.2
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
   version=1.1.2
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
   $version = "1.1.2"
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
