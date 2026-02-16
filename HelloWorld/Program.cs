using System;

Console.WriteLine("=== Простой калькулятор (левая‑на‑правую) ===");
Console.WriteLine("Поддерживаемые операции: +  -  *  /  %  ^");
Console.WriteLine("Числа могут быть в экспоненциальной нотации, например 1.23e4");
Console.Write("Введите выражение (пример: 2 + 3 * 4 ^ 2): ");

string? input = Console.ReadLine();

if (string.IsNullOrWhiteSpace(input))
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Ошибка: пустой ввод.");
    Console.ResetColor();
    return;               // завершить Main
}

try
{
    double answer = Calculator.EvaluateExpression(input);
    Console.WriteLine($"Ответ = {answer}");
}
catch (Exception ex) when (ex is FormatException ||
                           ex is ArgumentException ||
                           ex is DivideByZeroException)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Ошибка: {ex.Message}");
    Console.ResetColor();
}