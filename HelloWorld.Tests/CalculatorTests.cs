using System;
using Xunit;
using static Calculator;      // если Calculator без namespace, иначе используйте полное имя

public class CalculatorTests
{
    // базовые операции
    [Theory]
    [InlineData(2, 3, '+', 5)]
    [InlineData(10, 4, '-', 6)]
    [InlineData(5, 6, '*', 30)]
    [InlineData(9, 3, '/', 3)]
    [InlineData(10, 3, '%', 1)]
    [InlineData(2, 3, '^', 8)]
    public void Compute_BasicOperations_ReturnsCorrectResult(double left, double right, char op, double expected)
    {
        double actual = Compute(left, right, op);
        Assert.Equal(expected, actual);
    }

    // деление на ноль
    [Fact]
    public void Compute_DivideByZero_Throws()
        => Assert.Throws<DivideByZeroException>(() => Compute(5, 0, '/'));

    // неизвестный оператор
    [Fact]
    public void Compute_UnsupportedOperator_Throws()
        => Assert.Throws<ArgumentException>(() => Compute(1, 2, '#'));

    // парсинг научной нотации
    [Theory]
    [InlineData("1.23e4", 12300)]
    [InlineData("-4.5E-2", -0.045)]
    [InlineData("6E3", 6000)]
    public void ParseNumber_ScientificNotation_ReturnsCorrectValue(string input, double expected)
    {
        double actual = ParseNumber(input);
        Assert.Equal(expected, actual);
    }

    // оценка выражения слева‑направо
    [Theory]
    [InlineData("2 + 3 * 4", 20)]          // (2+3)*4 = 20
    [InlineData("5 - 2 ^ 2", 9)]          // (5-2)^2 = 9
    [InlineData("10 / 2 * 3", 15)]        // (10/2)*3 = 15
    [InlineData("1 + 2 + 3 + 4", 10)]     // цепочка сложения
    public void EvaluateExpression_LeftToRight_CalculatesCorrectly(string expr, double expected)
    {
        double actual = EvaluateExpression(expr);
        Assert.Equal(expected, actual);
    }

    // некорректный ввод
    [Fact]
    public void EvaluateExpression_InvalidFormat_Throws()
        => Assert.Throws<FormatException>(() => EvaluateExpression("2 +"));
}