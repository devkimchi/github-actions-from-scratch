# Events & Refactoring #

## Events &ndash; `push`, `pull_request` ##

```yaml
name: 'My First GitHub Actions'

on:
  push:
    branches:
    - main
    tags:
    - 'v*'
  pull_request:
    branches:
    - main

jobs:
  build:
    name: Build Apps

    runs-on: ubuntu-latest

    steps:
    - name: Checkout the repo
      uses: actions/checkout@v2

    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '6.x'

    - name: Restore NuGet packages
      shell: bash
      run: |
        dotnet restore ./api

    - name: Build solution
      shell: bash
      run: |
        dotnet build ./api -c Release

    - name: Create FunctionApp artifact
      shell: bash
      run: |
        dotnet publish ./api -c Release -o ./api/bin/published

    - name: Upload FunctionApp artifact
      uses: actions/upload-artifact@v2
      with:
        name: apiapp
        path: api/bin/published

  release:
    name: Release Apps
    needs: build

    runs-on: ubuntu-latest

    steps:
    - name: Download FunctionApp artifact
      uses: actions/download-artifact@v2
      with:
        name: apiapp
        path: artifacts/api

    - name: Zip FunctionApp artifact
      shell: bash
      run: |
        pushd artifacts/api
        zip -qq -r apiapp.zip .
        popd

        mv artifacts/api/apiapp.zip artifacts/apiapp.zip

    - name: Release FunctionApp artifact to GitHub
      uses: "marvinpinto/action-automatic-releases@latest"
      with:
        repo_token: "${{ secrets.GITHUB_TOKEN }}"
        prerelease: false
        files: |
          artifacts/apiapp.zip
```


## Events &ndash; `workflow_dispatch` ##

```yaml
name: 'Manual Release'

on:
  workflow_dispatch:
    inputs:
      title:
        type: string
        required: true
        description: Enter the release title

jobs:
  build:
    name: Build Apps

    runs-on: ubuntu-latest

    steps:
    - name: Checkout the repo
      uses: actions/checkout@v2

    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '6.x'

    - name: Restore NuGet packages
      shell: bash
      run: |
        dotnet restore ./api

    - name: Build solution
      shell: bash
      run: |
        dotnet build ./api -c Release

    - name: Create FunctionApp artifact
      shell: bash
      run: |
        dotnet publish ./api -c Release -o ./api/bin/published

    - name: Upload FunctionApp artifact
      uses: actions/upload-artifact@v2
      with:
        name: apiapp
        path: api/bin/published

  release:
    name: Release Apps
    needs: build

    runs-on: ubuntu-latest

    steps:
    - name: Download FunctionApp artifact
      uses: actions/download-artifact@v2
      with:
        name: apiapp
        path: artifacts/api

    - name: Zip FunctionApp artifact
      shell: bash
      run: |
        pushd artifacts/api
        zip -qq -r apiapp.zip .
        popd

        mv artifacts/api/apiapp.zip artifacts/apiapp.zip

    - name: Release FunctionApp artifact to GitHub
      uses: "marvinpinto/action-automatic-releases@latest"
      with:
        repo_token: "${{ secrets.GITHUB_TOKEN }}"
        prerelease: false
        title: ${{ github.event.inputs.title }}
        files: |
          artifacts/apiapp.zip
```


## Events &ndash; `workflow_call` ##

### `main.yaml` ###

```yaml
name: 'My First GitHub Actions'

on:
  push:
    branches:
    - main
    tags:
    - 'v*'
  pull_request:
    branches:
    - main

jobs:
  call_build:
    uses: ./.github/workflows/ci.yaml
    with:
      artifact_name: apiapp

  call_release:
    uses: ./.github/worlfows/cd.yaml
    needs: call_build
    with:
      title: latest
      artifact_name: apiapp
    secrets: inherit
```


### `manual-release.yaml` ###

```yaml
name: 'Manual Release'

on:
  workflow_dispatch:
    inputs:
      title:
        type: string
        required: true
        description: Enter the release title

jobs:
  call_build:
    uses: ./.github/workflows/ci.yaml
    with:
      artifact_name: apiapp

  call_release:
    uses: ./.github/worlfows/cd.yaml
    needs: call_build
    with:
      title: ${{ github.event.inputs.title }}
      artifact_name: apiapp
    secrets: inherit
```


### `ci.yaml` ###

```yaml
name: 'Continuous Integration'

on:
  workflow_call:
    inputs:
      artifact_name:
        type: string
        required: false
        description: Name of artifact
        default: apiapp

jobs:
  build:
    name: Build Apps

    runs-on: ubuntu-latest

    steps:
    - name: Checkout the repo
      uses: actions/checkout@v2

    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '6.x'

    - name: Restore NuGet packages
      shell: bash
      run: |
        dotnet restore ./api

    - name: Build solution
      shell: bash
      run: |
        dotnet build ./api -c Release

    - name: Create FunctionApp artifact
      shell: bash
      run: |
        dotnet publish ./api -c Release -o ./api/bin/published

    - name: Upload FunctionApp artifact
      uses: actions/upload-artifact@v2
      with:
        name: ${{ inputs.artifact_name }}
        path: api/bin/published
```


### `cd.yaml` ###

```yaml
name: 'Continuous Deployment'

on:
  workflow_call:
    inputs:
      title:
        type: string
        required: true
        description: Enter the release title
      artifact_name:
        type: string
        required: false
        description: Name of artifact
        default: apiapp

jobs:
  release:
    name: Release Apps

    runs-on: ubuntu-latest

    steps:
    - name: Download FunctionApp artifact
      uses: actions/download-artifact@v2
      with:
        name: ${{ inputs.artifact_name }}
        path: artifacts/api

    - name: Zip FunctionApp artifact
      shell: bash
      run: |
        pushd artifacts/api
        zip -qq -r ${{ inputs.artifact_name }}.zip .
        popd

        mv artifacts/api/${{ inputs.artifact_name }}.zip artifacts/${{ inputs.artifact_name }}.zip

    - name: Release FunctionApp artifact to GitHub
      uses: "marvinpinto/action-automatic-releases@latest"
      with:
        repo_token: "${{ secrets.GITHUB_TOKEN }}"
        prerelease: false
        title: ${{ inputs.title }}
        files: |
          artifacts/${{ inputs.artifact_name }}.zip
```
