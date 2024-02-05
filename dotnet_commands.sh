@REM Створити однією командою два проекти (основний (FlightSystem), для тестування (FlightSystemTests))
@REM дял стоврення проектів використовуюється версія .NET 8.0 
dotnet new console --framework net8.0 -o "FlightSystem"

@REM Застосувати зміни 
dotnet restore