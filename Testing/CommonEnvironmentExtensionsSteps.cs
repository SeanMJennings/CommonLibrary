using BDD;
using Common.Environment;
using Shouldly;

namespace Testing;

public partial class CommonEnvironmentExtensionsShould : Specification
{
    private CommonEnvironment? environment;

    protected override void before_each()
    {
        base.before_each();
        environment = null!;
        there_is_no_environment_in_environment_variables();
    }

    protected override void after_all()
    {
        there_is_no_environment_in_environment_variables();
        base.after_all();
    }

    private void there_is_no_environment_in_environment_variables()
    {
        System.Environment.SetEnvironmentVariable("ENVIRONMENT", null);
    }
    
    private void there_is_an_environment_in_environment_variables(CommonEnvironment theCommonEnvironment)
    {
        System.Environment.SetEnvironmentVariable("ENVIRONMENT", theCommonEnvironment.ToString().ToLower());
    }

    private void getting_environment()
    {
        environment = CommonEnvironmentExtensions.GetEnvironment();
    }

    private void environment_is_local_development()
    {
        environment.ShouldBe(CommonEnvironment.LocalDevelopment);
    }

    private void environment_is_correct(CommonEnvironment theCommonEnvironment)
    {
        environment.ShouldBe(theCommonEnvironment);
    }
}