using Common.Environment;
using NUnit.Framework;

namespace Testing;

[TestFixture]
public partial class CommonEnvironmentExtensionsShould
{
    [Test]
    public void assume_environment_is_local_development()
    {
        Given(there_is_no_environment_in_environment_variables);
        When(getting_environment);
        Then(environment_is_local_development);
    }

    [TestCase(CommonEnvironment.LocalDevelopment)]
    [TestCase(CommonEnvironment.Development)]
    [TestCase(CommonEnvironment.UserAcceptanceTesting)]
    [TestCase(CommonEnvironment.Production)]
    public void get_environment_from_environment_variables(CommonEnvironment commonEnvironment)
    {
        Given(() => there_is_an_environment_in_environment_variables(commonEnvironment));
        When(getting_environment);
        Then(() => environment_is_correct(commonEnvironment));
    }
}