namespace Calculator.Tests;

public class CalculatorEngineTests
{
    private readonly CalculatorEngine _calculator = new();

    [TestCase(2, 3, 5)]
    [TestCase(-2, -3, -5)]
    [TestCase(-2, 5, 3)]
    [TestCase(0, 0, 0)]
    public void Sum_ReturnsSum(int a, int b, int expected) => Assert.That(_calculator.Sum(a, b), Is.EqualTo(expected));

    [TestCase(5, 3, 2)]
    [TestCase(3, 5, -2)]
    [TestCase(-5, -3, -2)]
    public void Subtract_ReturnsDifference(int a, int b, int expected) => Assert.That(_calculator.Subtract(a, b), Is.EqualTo(expected));

    [TestCase(4, 3, 12)]
    [TestCase(-4, 3, -12)]
    [TestCase(7, 0, 0)]
    [TestCase(1, 9, 9)]
    public void Multiply_ReturnsProduct(int a, int b, int expected) => Assert.That(_calculator.Multiply(a, b), Is.EqualTo(expected));

    [TestCase(10, 2, 5)]
    [TestCase(7, 2, 3)]
    [TestCase(-9, 3, -3)]
    [TestCase(0, 5, 0)]
    public void Divide_ReturnsIntegerQuotient(int a, int b, int expected) => Assert.That(_calculator.Divide(a, b), Is.EqualTo(expected));

    [Test]
    public void Divide_ByZero_Throws() => Assert.Throws<DivideByZeroException>(() => _calculator.Divide(1, 0));

    [Test]
    public void Operations_Overflow_Throw()
    {
        Assert.Throws<OverflowException>(() => _calculator.Sum(int.MaxValue, 1));
        Assert.Throws<OverflowException>(() => _calculator.Subtract(int.MinValue, 1));
        Assert.Throws<OverflowException>(() => _calculator.Multiply(int.MaxValue, 2));
    }
}
