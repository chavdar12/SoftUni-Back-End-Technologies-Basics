namespace Calculator;

public class CalculatorEngine
{
    public int Sum(int a, int b) => checked(a + b);

    public int Subtract(int a, int b) => checked(a - b);

    public int Multiply(int a, int b) => checked(a * b);

    public int Divide(int a, int b) => b == 0 ? throw new DivideByZeroException("Cannot divide by zero.") : a / b;
}
