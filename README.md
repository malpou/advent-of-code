# Advent of Code Solutions

Personal repository containing my [Advent of Code](https://adventofcode.com/) solutions implemented in .NET. This
repository serves as a collection of my problem-solving approaches for various years of the Advent of Code challenge.

## Structure

Solutions are organized by year and day:

```
├── Y2023
│   ├── Day01.cs
│   ├── Day02.cs
│   └── ...
├── Y2022
│   ├── Day01.cs
│   └── ...
└── ...
```

## CLI Setup

### Overview

This guide explains how to set up a command-line interface for Advent of Code in ZSH and PowerShell (with Oh My
Posh). The CLI provides commands for:

- Setting the current year and day
- Running solutions
- Submitting answers
- Managing session tokens
- Validating session status

## Unix Installation (ZSH)

### 1. Add to .zshrc

Add the following configuration to your `~/.zshrc`:

```zsh
# Advent of Code Configuration
export AOC_YEAR=""
export AOC_DAY=""
export AOC_DIR="$HOME/git/advent-of-code"

# Main AOC function
aoc() {
    case "$1" in
        "set")
            if [[ -z "$2" || -z "$3" ]]; then
                echo "Usage: aoc set <year> <day>"
                return 1
            fi
            export AOC_YEAR="$2"
            export AOC_DAY="$3"
            echo "Set year to $AOC_YEAR and day to $AOC_DAY"
            ;;
        "run")
            if [[ -z "$AOC_YEAR" || -z "$AOC_DAY" ]]; then
                echo "Year and day not set. Use 'aoc set <year> <day>' first"
                return 1
            fi
            (cd "$AOC_DIR" && dotnet run "$AOC_YEAR" "$AOC_DAY")
            ;;
        "submit")
            if [[ -z "$AOC_YEAR" || -z "$AOC_DAY" ]]; then
                echo "Year and day not set. Use 'aoc set <year> <day>' first"
                return 1
            fi
            (cd "$AOC_DIR" && dotnet run "$AOC_YEAR" "$AOC_DAY" -s)
            ;;
        "token")
            if [[ -z "$2" ]]; then
                echo "Usage: aoc token <session-token>"
                return 1
            fi
            echo "$2" > "$AOC_DIR/secret"
            echo "Session token updated. Validating..."
            (cd "$AOC_DIR" && dotnet run ok)
            ;;
        *)
            if [[ -n "$AOC_YEAR" && -n "$AOC_DAY" ]]; then
                echo "Current configuration:"
                echo "Year: $AOC_YEAR"
                echo "Day:  $AOC_DAY"
                echo "\nValidating session token..."
            else
                echo "No year and day set. Use 'aoc set <year> <day>' to set them."
                echo "\nValidating session token..."
            fi
            (cd "$AOC_DIR" && dotnet run ok)
            ;;
    esac
}

# Command completion
_aoc() {
    local -a commands
    commands=(
        'set:Set the year and day'
        'run:Run the current day'
        'submit:Submit the current day'
        'token:Update session token'
    )
    _arguments "1: :{_describe 'commands' commands}"
}
compdef _aoc aoc

# Shorthand aliases
alias aocr='aoc run'
alias aocs='aoc submit'
```

### 2. Reload ZSH Configuration

After adding the configuration, reload your ZSH configuration:

```bash
source ~/.zshrc
```

## Windows Installation (Oh My Posh)

### 1. Add to PowerShell Profile

Add the following to your PowerShell profile (usually at `$PROFILE` or
`~\Documents\PowerShell\Microsoft.PowerShell_profile.ps1`):

```powershell
# Advent of Code Configuration
$env:AOC_YEAR = ""
$env:AOC_DAY = ""
$env:AOC_DIR = "$HOME\git\advent-of-code"

# Main AOC function
function aoc {
    param(
        [Parameter(Position = 0)]
        [string]$Command,
        [Parameter(Position = 1)]
        [string]$Param1,
        [Parameter(Position = 2)]
        [string]$Param2
    )

    switch ($Command) {
        "set" {
            if (-not $Param1 -or -not $Param2) {
                Write-Host "Usage: aoc set <year> <day>"
                return
            }
            $env:AOC_YEAR = $Param1
            $env:AOC_DAY = $Param2
            Write-Host "Set year to $env:AOC_YEAR and day to $env:AOC_DAY"
        }
        "run" {
            if (-not $env:AOC_YEAR -or -not $env:AOC_DAY) {
                Write-Host "Year and day not set. Use 'aoc set <year> <day>' first"
                return
            }
            Push-Location $env:AOC_DIR
            dotnet run $env:AOC_YEAR $env:AOC_DAY
            Pop-Location
        }
        "submit" {
            if (-not $env:AOC_YEAR -or -not $env:AOC_DAY) {
                Write-Host "Year and day not set. Use 'aoc set <year> <day>' first"
                return
            }
            Push-Location $env:AOC_DIR
            dotnet run $env:AOC_YEAR $env:AOC_DAY -s
            Pop-Location
        }
        "token" {
            if (-not $Param1) {
                Write-Host "Usage: aoc token <session-token>"
                return
            }
            $Param1 | Out-File -FilePath "$env:AOC_DIR\secret"
            Write-Host "Session token updated. Validating..."
            Push-Location $env:AOC_DIR
            dotnet run ok
            Pop-Location
        }
        default {
            if ($env:AOC_YEAR -and $env:AOC_DAY) {
                Write-Host "Current configuration:"
                Write-Host "Year: $env:AOC_YEAR"
                Write-Host "Day:  $env:AOC_DAY"
                Write-Host "`nValidating session token..."
            }
            else {
                Write-Host "No year and day set. Use 'aoc set <year> <day>' to set them."
                Write-Host "`nValidating session token..."
            }
            Push-Location $env:AOC_DIR
            dotnet run ok
            Pop-Location
        }
    }
}

# Shorthand aliases
Set-Alias -Name aocr -Value { aoc run }
Set-Alias -Name aocs -Value { aoc submit }

# Tab completion
Register-ArgumentCompleter -CommandName aoc -ScriptBlock {
    param($wordToComplete, $commandAst, $cursorPosition)
    $commands = @('set', 'run', 'submit', 'token')
    $commands | Where-Object { $_ -like "$wordToComplete*" } | ForEach-Object {
        [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
    }
}
```

### 2. Reload PowerShell Profile

After adding the configuration, reload your PowerShell profile:

```powershell
. $PROFILE
```

## Usage (Both Platforms)

### Basic Commands

```bash
aoc                 # Show current configuration and validate token
aoc set 2023 1      # Set the year to 2023 and day to 1
aoc run             # Run the current day's solution
aoc submit          # Submit the current day's solution
aoc token <token>   # Update session token
```

### Shorthand Aliases

```bash
aocr    # Same as 'aoc run'
aocs    # Same as 'aoc submit'
```

### Command Completion

The CLI supports tab completion for all commands. Press TAB after typing `aoc` to see available commands.

## Prerequisites

- ZSH shell (Unix) or PowerShell with Oh My Posh (Windows)
- The Advent of Code solution repository should be at:
    - Unix: `$HOME/git/advent-of-code`
    - Windows: `$HOME\git\advent-of-code`
- .NET SDK installed for running solutions

## Author

[@malpou](https://github.com/malpou)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
