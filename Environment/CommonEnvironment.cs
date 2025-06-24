namespace Common.Environment;

public enum CommonEnvironment
{
    LocalDevelopment,
    LocalTesting,
    BuildPipeline,
    Development,
    UserAcceptanceTesting,
    Production,
}

public static class CommonEnvironmentExtensions
{
    public static bool IsAProductionEnvironment(this CommonEnvironment environment)
    {
        return environment is CommonEnvironment.Production;
    }
    
    public static bool IsLocalTestingOrBuildPipelineEnvironment(this CommonEnvironment environment)
    {
        return environment is CommonEnvironment.LocalTesting or CommonEnvironment.BuildPipeline;
    }
    
    public static CommonEnvironment GetEnvironment()
    {
        return Enum.TryParse<CommonEnvironment>(System.Environment.GetEnvironmentVariable("ENVIRONMENT")?.ToLower(), true, out var environment) ? environment : CommonEnvironment.LocalDevelopment;
    }
    
    public static void SetEnvironment(this CommonEnvironment environment)
    {
        System.Environment.SetEnvironmentVariable("ENVIRONMENT", environment.ToString().ToLower());
    }
}