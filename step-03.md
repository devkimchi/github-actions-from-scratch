# Matrix & Conditions #

## Matrix #1 ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    strategy:
      matrix:
        os: [ 'windows-latest', 'macos-latest', 'ubuntu-latest' ]

    runs-on: ${{ matrix.os }}

    steps:
    - name: Say Hello World on ${{ matrix.os }}
      shell: bash
      run: |
        echo "Hello World on ${{ matrix.os }}"
```


## Matrix #2 ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    strategy:
      matrix:
        os: [ 'windows-latest', 'macos-latest', 'ubuntu-latest' ]
        message: [ 'Hello World', 'Lorem Ipsum' ]

    runs-on: ${{ matrix.os }}

    steps:
    - name: Greetings from ${{ matrix.os }}
      shell: bash
      run: |
        echo "${{ matrix.message }} from ${{ matrix.os }}"
```


## Matrix #3 ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    strategy:
      matrix:
        os: [ 'windows-latest', 'macos-latest', 'ubuntu-latest' ]
        nodejs: [ '14.x', '16.x' ]

    runs-on: ${{ matrix.os }}

    steps:
    - name: Setup node.js
      uses: actions/setup-node@v3
      with:
        node-version: ${{ matrix.nodejs }}

    - name: Check node.js version from ${{ matrix.os }}
      shell: bash
      run: |
        echo $(node --version)
```


## Conditions on Jobs ##

```yaml
name: 'My First GitHub Actions'

on: [ 'push', 'pull_request' ]

jobs:
  first-job:
    name: 'First Job'
    if: github.event_name == 'push'

    strategy:
      matrix:
        os: [ 'windows-latest', 'macos-latest', 'ubuntu-latest' ]

    runs-on: ${{ matrix.os }}

    steps:
    - name: Say Hello World from ${{ matrix.os }}
      shell: bash
      run: |
        echo "Hello World on ${{ matrix.os }}"

  second-job:
    name: 'Second Job'
    if: github.event_name == 'pull_request'

    strategy:
      matrix:
        os: [ 'windows-latest', 'macos-latest', 'ubuntu-latest' ]

    runs-on: ${{ matrix.os }}

    steps:
    - name: Say Hello World from ${{ matrix.os }}
      shell: bash
      run: |
        echo "Hello World on ${{ matrix.os }}"
```


## Conditions on Steps ##

```yaml
name: 'My First GitHub Actions'

on: push

jobs:
  first-job:
    name: 'First Job'

    strategy:
      matrix:
        os: [ 'windows-latest', 'macos-latest', 'ubuntu-latest' ]

    runs-on: ${{ matrix.os }}

    steps:
    - name: Say Hello World from ${{ matrix.os }} 1
      shell: bash
      run: |
        echo "Hello World on ${{ matrix.os }} 1"

    - name: Say Hello World from ${{ matrix.os }} 2
      if: matrix.os == 'ubuntu-latest'
      shell: bash
      run: |
        echo "Hello World on ${{ matrix.os }} 2"
```


## Job Chaining ##

```yaml
name: 'My First GitHub Actions'

on:
  push:
    branches:
    - main

jobs:
  first-job:
    name: 'First Job'

    runs-on: ubuntu-latest

    steps:
    - name: Say Hello World from first job
      shell: bash
      run: |
        echo "Hello World from first job"

  second-job:
    name: 'Second Job'
    needs: first-job

    runs-on: ubuntu-latest

    steps:
    - name: Say Hello World from second job
      shell: bash
      run: |
        echo "Hello World from second job"
```
