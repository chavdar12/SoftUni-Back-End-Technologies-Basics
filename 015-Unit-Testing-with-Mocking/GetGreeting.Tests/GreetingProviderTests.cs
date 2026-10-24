using Moq;

namespace GetGreeting.Tests;

public class GreetingProviderTests
{
    [TestCase(5, "Good morning!")]
    [TestCase(11, "Good morning!")]
    [TestCase(12, "Good afternoon!")]
    [TestCase(17, "Good afternoon!")]
    [TestCase(18, "Good evening!")]
    [TestCase(21, "Good evening!")]
    [TestCase(22, "Good night!")]
    [TestCase(0, "Good night!")]
    [TestCase(4, "Good night!")]
    public void GetGreeting_ReturnsGreetingForHour(int hour, string expected)
    {
        var timeProvider = new Mock<ITimeProvider>();
        timeProvider.Setup(t => t.GetCurrentTime()).Returns(new DateTime(2026, 1, 1, hour, 30, 0));

        var greeting = new GreetingProvider(timeProvider.Object).GetGreeting();

        Assert.That(greeting, Is.EqualTo(expected));
        timeProvider.Verify(t => t.GetCurrentTime(), Times.Once);
    }
}
