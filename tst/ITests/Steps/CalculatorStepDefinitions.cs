using App;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace ITests.Steps
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private const string NumberA = nameof(NumberA);
        private const string NumberB = nameof(NumberB);
        private const string Result = nameof(Result);

        private readonly ScenarioContext _scenarioContext;
        private readonly Calculator _sut;

        public CalculatorStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _sut = new Calculator();
        }

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            _scenarioContext.Add(NumberA, number);
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            _scenarioContext.Add(NumberB, number);
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            int result = _sut.Add(_scenarioContext.Get<int>(NumberA), _scenarioContext.Get<int>(NumberB));
            _scenarioContext.Add(Result, result);
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            _scenarioContext.Get<int>(Result).Should().Be(result);
        }
    }
}
