// Program.cs (top‑level statements)
using System;

Console.WriteLine("Введите выражение: ");
string? line = Console.ReadLine();

try
{
    double result = Calculator.EvaluateExpression(line);
    Console.WriteLine($"Результат: {line} = {result}");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Ошибка: {ex.Message}");
    Console.ResetColor();
}