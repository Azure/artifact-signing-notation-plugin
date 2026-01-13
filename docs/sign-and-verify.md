# Sign and verify an artifact with a signing certficate profile

> [!NOTE]
> Signing with artifact signing requires a signing account and certificate profile. For more information, see [Set up artifact signing](https://learn.microsoft.com/en-us/azure/artifact-signing/quickstart).

## Requirements

To sign a component with artifact signing, an active signing account and certificate profile are needed. The endpoint uri corresponding to the profile's region is also required, to corroborate which endpoint to use, see the [following table](https://learn.microsoft.com/azure/artifact-signing/quickstart?tabs=registerrp-portal%2Caccount-portal%2Corgvalidation%2Ccertificateprofile-portal%2Cdeleteresources-portal#azure-regions-that-support-artifact-signing)

## Setting up the plugin signing key

From a terminal, run the following command to add a key with the artifact signing plugin:

```sh
notation key add "<<keyname>>" --plugin azure-artifactsigning --plugin-config accountName="<<signingAccount>>" --plugin-config certProfile="<<certificateProfile>>" --plugin-config baseUrl="<<endpointUri>>" --id "<<keyid>>"
```

> They key name is an user friendly name to refer to during sign operations from the CLI. The id is a unique identifier only known to the plugin. For more information, see [Plugin usage](https://github.com/notaryproject/specifications/blob/v1.0.0/specs/plugin-extensibility.md#using-a-plugin-for-signing)


## Signing an artifact

1. [Install the Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli)
2. Log in to Azure with Azure CLI:
   ```sh
   az login
   az account set --subscription $subscriptionID
   ```
> [!NOTE]
> Ensure the account used for signing has the necessary rbac permissions to sign with the artifact signing account.
> For more information, see [Supported roles for Artifact Signing](https://learn.microsoft.com/en-us/azure/artifact-signing/tutorial-assign-roles#supported-roles-for-artifact-signing)
3. [Create an Azure Container Registry](https://learn.microsoft.com/azure/container-registry/container-registry-get-started-portal?tabs=azure-cli). The remaining steps use the example login server `<registry-name>.azurecr.io`, but you must substitute your own login server value.
4. Log into the container registry and push an image for signing:
   ```sh
   registryName="<registry-name>"
   server="${registryName}.azurecr.io"

   az acr login --name $registryName
   # notation login $server  # if you don't use Azure Container Registry
   ```
   Push a hello-world image for signing
   ```sh
   docker pull hello-world:latest
   docker tag hello-world:latest $server/hello-world:v1
   docker push $server/hello-world:v1
   ```
5. Sign the image with the artifact signing key:
   ```sh
   notation sign $server/hello-world:v1 --key "<<keyname>>"
   ```
6. A message indicating `Successfully signed $server/hello-world:v1@sha256:...` is displayed.


## Verify the signature

To verify the successful signature of an artifact, run the following command:

```sh
notation ls $server/hello-world:v1
```

To view more details about the signature, run the following command:

```sh
notation inspect $server/hello-world:v1
```
