using System;
using System.Globalization;

public static class Calculator
{
    public static double EvaluateExpression(string expression)
    {
        var parts = expression
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3 || parts.Length % 2 == 0)
            throw new FormatException("Выражение должно иметь форму: число оператор число [оператор число]…");

        double result = ParseNumber(parts[0]);

        for (int i = 1; i < parts.Length; i += 2)
        {
            char op = parts[i][0];
            double next = ParseNumber(parts[i+1]);

            result = Compute(result, next, op);
        }

        return result;
    }
    // Выполняет одну арифметическую операцию.
    // Возвращает результат как double.
    public static double Compute(double left, double right, char op)
    {
        return op switch
        {
            '+' => left + right,
            '-' => left - right,
            '*' => left * right,
            '/' => right == 0
                    ? throw new DivideByZeroException("Division by zero is not allowed.")
                    : left / right,
            '%' => left % right,
            '^' => Math.Pow(left, right),
            _   => throw new ArgumentException($"Unsupported operator '{op}'.")
        };
    }

    // Преобразует строку в double с учётом текущей культуры.
    public static double ParseNumber(string input)
    {
        if (double.TryParse(input, 
                            NumberStyles.Float, 
                            CultureInfo.InvariantCulture, 
                            out var value))
            return value;

        throw new FormatException($"Не удалось распарсить число: '{input}'");
    }
}