# Base directory of the script location
$BaseDir = $PSScriptRoot

# Define each project's path individually using Join-Path for clarity
$Projects = @(
    (Join-Path -Path $BaseDir -ChildPath "src\Modules\Stakeholders\Explorer.Stakeholders.Infrastructure"),
    (Join-Path -Path $BaseDir -ChildPath "src\Modules\Tours\Explorer.Tours.Infrastructure"),
    (Join-Path -Path $BaseDir -ChildPath "src\Modules\Blog\Explorer.Blog.Infrastructure")
)

# Define the relative path to the startup project
$StartupProject = Join-Path -Path $BaseDir -ChildPath "src\Explorer.API"

# Step 1: Delete existing "Migrations" folders
foreach ($project in $Projects) {
    $MigrationPath = Join-Path -Path $project -ChildPath "Migrations"
    if (Test-Path -Path $MigrationPath) {
        Remove-Item -Path $MigrationPath -Recurse -Force
        Write-Output "Deleted Migrations folder in $project"
    }
}

# Step 2: Recreate migrations and update the database for each context
# Define each context with the corresponding relative project path
$Contexts = @(
    @{ Name = "StakeholdersContext"; Project = (Join-Path -Path $BaseDir -ChildPath "src\Modules\Stakeholders\Explorer.Stakeholders.Infrastructure") },
    @{ Name = "ToursContext"; Project = (Join-Path -Path $BaseDir -ChildPath "src\Modules\Tours\Explorer.Tours.Infrastructure") },
    @{ Name = "BlogContext"; Project = (Join-Path -Path $BaseDir -ChildPath "src\Modules\Blog\Explorer.Blog.Infrastructure") }
)

foreach ($context in $Contexts) {
    # Run Add-Migration
    dotnet ef migrations add Init -c $context.Name -p $context.Project -s $StartupProject
    Write-Output "Added migration for $($context.Name)"

    # Run Update-Database
    dotnet ef database update -c $context.Name -p $context.Project -s $StartupProject
    Write-Output "Updated database for $($context.Name)"
}

Write-Output "All migrations have been deleted, recreated, and databases updated."
