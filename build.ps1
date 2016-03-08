param($task = "default", $parameters = @{ "output"="/trackmatic-1-0" })
get-module psake | remove-module
import-module psake
$psake.use_exit_on_error = $true
exec { 
    invoke-psake "default.ps1" -task $task -parameters $parameters
}
