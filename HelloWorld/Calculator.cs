using System;
using System.Globalization;

/// <summary>
/// Статический класс, реализующий простой калькулятор:
///   • парсинг чисел (включая экспоненциальную запись);
///   • арифметические операции +, -, *, /, %, ^;
///   • оценка выражения слева‑направо без приоритетов.
/// </summary>
public static class Calculator
{
    // -----------------------------------------------------------------
    // 1. Парсинг числа (поддержка экспоненциальной нотации)
    // -----------------------------------------------------------------
    /// <summary>
    /// Преобразует строку в <see cref="double"/> с помощью <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    /// <param name="input">Строка, содержащая число.</param>
    /// <returns>Число типа double.</returns>
    /// <exception cref="FormatException">Если строка не является корректным числом.</exception>
    public static double ParseNumber(string input)
    {
        if (double.TryParse(
                input,
                NumberStyles.Float,               // поддерживает экспоненту, знак, точку
                CultureInfo.InvariantCulture,
                out var value))
        {
            return value;
        }

        throw new FormatException($"Не удалось распарсить число: '{input}'.");
    }

    // -----------------------------------------------------------------
    // 2. Выполнение одной арифметической операции
    // -----------------------------------------------------------------
    /// <summary>
    /// Выполняет арифметическую операцию над двумя операндами.
    /// </summary>
    /// <param name="left">Левый операнд.</param>
    /// <param name="right">Правый операнд.</param>
    /// <param name="op">Оператор: '+', '-', '*', '/', '%', '^'.</param>
    /// <returns>Результат операции.</returns>
    /// <exception cref="DivideByZeroException">При делении на ноль.</exception>
    /// <exception cref="ArgumentException">Если оператор не поддерживается.</exception>
    public static double Compute(double left, double right, char op)
    {
        return op switch
        {
            '+' => left + right,
            '-' => left - right,
            '*' => left * right,
            '/' => right == 0
                    ? throw new DivideByZeroException("Деление на ноль запрещено.")
                    : left / right,
            '%' => left % right,
            '^' => Math.Pow(left, right),   // возведение в степень
            _   => throw new ArgumentException($"Оператор '{op}' не поддерживается.")
        };
    }

    // -----------------------------------------------------------------
    // 3. Оценка полного выражения слева‑направо
    // -----------------------------------------------------------------
    /// <summary>
    /// Оценивает строковое выражение, где токены разделены пробелами.
    /// Вычисление производится по порядку появления операторов (без приоритетов).
    /// </summary>
    /// <param name="expression">Например: "2 + 3 * 4 ^ 2".</param>
    /// <returns>Результат вычисления.</returns>
    /// <exception cref="FormatException">Если формат неверный.</exception>
    /// <exception cref="ArgumentException">Если встретился неизвестный оператор.</exception>
    public static double EvaluateExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new FormatException("Выражение пустое.");

        var parts = expression
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Ожидаем нечётное количество токенов: число, оператор, число, …
        if (parts.Length < 3 || parts.Length % 2 == 0)
            throw new FormatException(
                "Выражение должно иметь форму: число оператор число [оператор число]…");

        double result = ParseNumber(parts[0]);

        for (int i = 1; i < parts.Length; i += 2)
        {
            char   op   = parts[i][0];
            double next = ParseNumber(parts[i + 1]);

            result = Compute(result, next, op);
        }

        // **Обязательный возврат результата**
        return result;
    }
}