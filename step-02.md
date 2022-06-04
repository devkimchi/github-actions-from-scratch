# Basic Syntax #

## `main.yaml` ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    runs-on: ubuntu-latest

    steps:
    - name: Say Hello World 1
      shell: bash
      run: |
        echo "Hello World from step 1"

    - name: Say Hello World 2
      shell: pwsh
      run: |
        echo "Hello World from step 2"
```


## Environment Variables ##

```yaml
name: 'My First GitHub Actions'

on: push

env:
  WORKFLOW_LEVEL: "This value comes from the WORKFLOW level"

jobs:
  first-job:
    name: 'First Job'

    runs-on: ubuntu-latest

    env:
      JOB_LEVEL: "This value comes from the JOB level"

    steps:
    - name: Say Hello World
      shell: bash
      run: |
        echo "Hello World"

    - name: Check environment variables 1
      shell: bash
      env:
        STEP_LEVEL_1: "This value comes from the STEP level #1"
      run: |
        echo "workflow level: $WORKFLOW_LEVEL"
        echo "job level: $JOB_LEVEL"
        echo "step level 1: $STEP_LEVEL_1"
        echo "step level 2: $STEP_LEVEL_2"
        echo "step level 3: $STEP_LEVEL_3"

    - name: Check environment variables 2
      shell: pwsh
      env:
        STEP_LEVEL_2: "This value comes from the STEP level #2"
      run: |
        echo "workflow level: $env:WORKFLOW_LEVEL"
        echo "job level: $env:JOB_LEVEL"
        echo "step level 1: $env:STEP_LEVEL_1"
        echo "step level 2: $env:STEP_LEVEL_2"
        echo "step level 3: $env:STEP_LEVEL_3"

    - name: Check environment variables 3
      shell: bash
      env:
        STEP_LEVEL_3: "This value comes from the STEP level #3"
      run: |
        echo "workflow level: ${{ env.WORKFLOW_LEVEL }}"
        echo "job level: ${{ env.JOB_LEVEL }}"
        echo "step level 1: ${{ env.STEP_LEVEL_1 }}"
        echo "step level 2: ${{ env.STEP_LEVEL_2 }}"
        echo "step level 3: ${{ env.STEP_LEVEL_3 }}"
```


## Runtime Environment Variables ##

```yaml
name: 'My First GitHub Actions'

on: push

env:
  PRESET_VALUE: "This is the preset value"

jobs:
  first-job:
    name: 'First Job'

    runs-on: ubuntu-latest

    steps:
    - name: Set environment variable 1
      shell: bash
      run: |
        echo "STEPSET_VALUE_1='This is the value 1 set in the step'" >> $GITHUB_ENV

    - name: Set environment variable 2
      shell: pwsh
      run: |
        echo "STEPSET_VALUE_2='This is the value 2 set in the step'" | Out-File -FilePath $env:GITHUB_ENV -Encoding utf-8 -Append

    - name: Check environment variables
      shell: bash
      run: |
        echo "preset value: ${{ env.PRESET_VALUE }}"
        echo "stepset value 1: ${{ env.STEPSET_VALUE_1 }}"
        echo "stepset value 2: ${{ env.STEPSET_VALUE_2 }}"
```


## Output Values Reference ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    runs-on: ubuntu-latest

    steps:
    - name: Set output value
      id: first
      shell: bash
      run: |
        first_value="This is the value for the first output"

        echo "::set-output name=first_value::$first_value"

    - name: Check output value
      shell: bash
      run: |
        echo "first value: ${{ steps.first.outputs.first_value }}"
```


## Output Values Mask ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    runs-on: ubuntu-latest

    steps:
    - name: Mask output value
      id: first
      shell: bash
      run: |
        first_value="This is the value for the first output"

        echo "::add-mask::$first_value"
        echo "::set-output name=first_value::$first_value"

    - name: Check output value
      shell: bash
      run: |
        echo "first value: ${{ steps.first.outputs.first_value }}"
```


## GitHub Secrets ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    runs-on: ubuntu-latest

    steps:
    - name: Show secret
      shell: bash
      run: |
        echo ${{ secrets.HELLO }}
```
