Framework "4.5.2x64"

properties {
	$base_dir = $psake.build_script_dir
    $xunit_runner = "$base_dir\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe"
    $destination_dir = $output    
    $nuget = "$base_dir\.nuget\NuGet.exe"
}

Task default -depends Clean, Build, Test

Task Publish {
    $apikey = Read-Host -Prompt 'Enter Api Key'
    publish-package "Plot" $apikey
    publish-package "Plot.Neo4j" $apikey
    publish-package "Plot.Testing" $apikey
}

Task Publish-Plot {
    $apikey = Read-Host -Prompt 'Enter Api Key'
    publish-package "Plot" $apikey    
}

Task Publish-Plot-Neo4j {
    $apikey = Read-Host -Prompt 'Enter Api Key'
    publish-package "Plot.Neo4j" $apikey
}

Task Publish-Plot-Testing {
    $apikey = Read-Host -Prompt 'Enter Api Key'
    publish-package "Plot.Testing" $apikey
}

Task Build {

    exec { msbuild "$base_dir/Plot.sln" }
}

Task Clean {
    Get-ChildItem ./ -include bin,obj -Recurse -Force | % { 
        write-host "Cleaning $_"
        Remove-Item $_ -Recurse -Force  
    }
}

Task Test {
    # NSpec Tests
    exec { & $xunit_runner "$base_dir\Plot.Tests\bin\Debug\Plot.Tests.dll" }
}

function publish-package($nuspec, $apikey) {
    $file = "$base_dir\$nuspec\$nuspec.nuspec"
    $spec = [xml](get-content $file)
    $version = $spec.package.metadata.version
    $package = "$base_dir\$nuspec.$version.nupkg"

    # Create nuget package and upload to nuget
    exec { & $nuget pack "$base_dir/$nuspec/$nuspec.csproj" -IncludeReferencedProjects }
    exec { & $nuget setApiKey $apikey }
    write-host $package
    exec { & $nuget push "$package" -source "https://www.nuget.org/api/v2/package" }

    # Perform some cleanup on the folder
    remove-item $package
}