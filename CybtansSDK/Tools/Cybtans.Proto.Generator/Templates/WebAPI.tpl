<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>

  <ItemGroup>    
    <PackageReference Include="Cybtans.AspNetCore" Version="1.0.12" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="5.5.1" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="3.1.6" />
    <PackageReference Include="Serilog.AspNetCore" Version="3.2.0" />   
    @{PACKAGES}

  </ItemGroup>

  @{FERERENCES}

</Project>
