    <Import Project="./common.props" />
    <PropertyGroup>
        <TargetFramework>net5.0</TargetFramework>
        <GeneratePackageOnBuild>$(OdinGeneratePackageOnBuild)</GeneratePackageOnBuild>
        <AssemblyName>$(OdinAssemblyName)</AssemblyName>
        <RootNamespace>$(OdinCoreDemo)</RootNamespace>
        <OutputType>$(OdinOutputType)</OutputType>
        <PackageId>$(OdinPackageId)</PackageId>
        <Authors>$(OdinAuthors)</Authors>
        <Company>$(OdinCompany)</Company>
        <Copyright>$(OdinCopyright)</Copyright>
        <Product>$(OdinProduct)</Product>
        <Description>$(OdinDescription)</Description>
        <Version>$(OdinVersion)</Version>
        <PackageTags>$(OdinPackageTags)</PackageTags>

        <RepositoryUrl>$(OdinGitUrl)/$(OdinAssemblyName)</RepositoryUrl>
        <RepositoryType>$(OdinRepositoryType)</RepositoryType>
        <RepositoryBranch>master</RepositoryBranch>
        <PackageIcon>icon.png</PackageIcon>
    </PropertyGroup>
    <ItemGroup>
        <None Include="images\icon.png" Pack="true" PackagePath="\"/>
    </ItemGroup>