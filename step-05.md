# Multi-Stage Deployments #

## Release to DEV/PROD ##

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

  call_release_dev:
    uses: ./.github/worlfows/cd.yaml
    needs: call_build
    with:
      title: latest
      artifact_name: apiapp
      env: DEV
    secrets: inherit

  call_release_prod:
    uses: ./.github/worlfows/cd.yaml
    needs: call_release_dev
    with:
      title: latest
      artifact_name: apiapp
      env: PROD
    secrets: inherit
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
      env:
        type: string
        required: false
        description: Environment name
        default: DEV

jobs:
  release:
    name: Release Apps

    runs-on: ubuntu-latest

    environment: ${{ inputs.env }}

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
        zip -qq -r ${{ inputs.artifact_name }}-${{ inputs.env }}.zip .
        popd

        mv artifacts/api/${{ inputs.artifact_name }}-${{ inputs.env }}.zip artifacts/${{ inputs.artifact_name }}-${{ inputs.env }}.zip

    - name: Release FunctionApp artifact to GitHub
      uses: "marvinpinto/action-automatic-releases@latest"
      with:
        repo_token: "${{ secrets.GITHUB_TOKEN }}"
        prerelease: false
        title: ${{ inputs.title }}
        files: |
          artifacts/${{ inputs.artifact_name }}-${{ inputs.env }}.zip
```
