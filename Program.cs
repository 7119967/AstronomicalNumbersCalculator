using System;
using System.Text.RegularExpressions;

namespace AstronomicalNumbersCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            string first, second;
            string operation, result = string.Empty;
            do
            {
                ShowRequestLine("Input 1th number: ");
                first = Console.ReadLine().Trim();
                int[] firstArr = ConvertLineToArrayNumbers(first);
                ShowRequestLine("Input 2nd number: ");
                second = Console.ReadLine().Trim();
                int[] secondArr = ConvertLineToArrayNumbers(second);
                if ((firstArr.Length == 0) | (secondArr.Length == 0))
                {
                    ShowDangerLine($"The \"first\" or \"second\" number has an invalid symbol");
                    ShowDangerLine($"The parsing failed. Try again!");
                    break;
                }
                ShowRequestLine("Choose an operation. Enter a number of the operation:\n1 - Addition\n2 - Subtraction\n");
                operation = Console.ReadLine().Trim();
                switch (operation)
                {
                    case "1":
                        operation = "Addition";
                        result = DoAddition(firstArr, secondArr);
                        break;
                    case "2":
                        operation = "Subtraction";
                        result = DoSubtraction(firstArr, secondArr); ;
                        break;
                    default:
                        ShowDangerLine("Wrong input");
                        break;
                }
                if (string.IsNullOrEmpty(result))
                    ShowDangerLine($"The {operation} failed. Try again!");
                else
                    ShowInfoLine($"The {operation} result: {result}\n"); 

                ShowRequestLine("Press \"Escape\" to quit or \"Enter\" to continue the application\n");
                key = Console.ReadKey();
                Console.Clear();
                result = string.Empty;
            }
            while (key.Key != ConsoleKey.Escape);
        }
        static ulong ConvertArrayToNumber(int[] arr)
        {
            string strA = string.Empty;
            ulong sumA;
            foreach (var item in arr)
            {
                strA += item;
            }
            sumA = Convert.ToUInt64(strA);
            return sumA;
        }
        static int[] ConvertLineToArrayNumbers(string input)
        {
            string pattern = @"[0-9]";
            int[] arr = new int[input.Length];
            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (string.IsNullOrEmpty(input[i].ToString()))
                    {
                        throw new Exception($"The parsing failed. The \"{input[i]}\" is not a number");
                    }
                    if (!Regex.Match(input[i].ToString(), pattern).Success)
                    {
                        throw new Exception($"The parsing failed. The \"{input[i]}\" is not a number");
                    }
                    bool result = int.TryParse(input[i].ToString(), out var numberParse);
                    if (result != true)
                    {
                        throw new Exception($"The parsing failed. The \"{input[i]}\" is not a number");
                    }

                    arr[i] = numberParse;
                }
            }
            catch (Exception e)
            {
                ShowDangerLine(e.Message);
                return Array.Empty<int>();
            }
            return arr;
        }
        static string DoAddition(int[] firstNumber, int[] secondNumber)
        {
            Array.Reverse(firstNumber);
            Array.Reverse(secondNumber);
            List<int> result = new List<int>();
            int first, second, sum, rest = 0;
            string output = string.Empty;
            for (int i = 0; i < Math.Max(firstNumber.Length, secondNumber.Length); i++)
            {
                first = i < firstNumber.Length ? firstNumber[i] : 0;
                second = i < secondNumber.Length ? secondNumber[i] : 0;
                sum = first + second + rest;
                if (sum >= 10)
                {
                    result.Add(sum - 10);
                    rest = sum / 10;
                }
                else
                {
                    result.Add(sum);
                    rest = 0;
                }
            }
            if (rest > 0) result.Add(rest);
            result.Reverse();
            foreach (int i in result)
            {
                output += i.ToString();
            }
            return output;
        }
        static string DoSubtraction(int[] firstNumber, int[] secondNumber)
        {
            var sumA = ConvertArrayToNumber(firstNumber);
            var sumB = ConvertArrayToNumber(secondNumber);

            var arrA = firstNumber;
            var arrB = secondNumber;

            if ((firstNumber.Length < secondNumber.Length) | (sumA < sumB))
            {
                arrB = firstNumber;
                arrA = secondNumber;
            }
            Array.Reverse(arrA);
            Array.Reverse(arrB);
            List<int> result = new List<int>();
            int first, second, sum, deduct, rest = 0;
            string output = string.Empty;
            for (int i = 0; i < Math.Max(arrA.Length, arrB.Length); i++)
            {
                first = i < arrA.Length ? arrA[i] : 0;
                second = i < arrB.Length ? arrB[i] : 0;
                deduct = (first - rest) - second;
                if (deduct < 0)
                {
                    sum = (first + 10 - rest) - second;
                    result.Add(sum);
                    rest = 1;
                }
                else
                {
                    sum = deduct;
                    result.Add(sum);
                    rest = 0;
                }
            }
            result.Reverse();
            foreach (int i in result)
            {
                output = output + i.ToString();
            }
            if ((firstNumber.Length < secondNumber.Length) | (sumA < sumB))
            {
                return $"{Convert.ToInt32(output) * -1}";
            }
            return $"{Convert.ToInt32(output) * 1}";
        }
        static void ShowRequestLine(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(value.PadRight(value.Length));
            Console.ResetColor();
        }
        static void ShowInfoLine(string value)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(value.PadRight(value.Length));
            Console.ResetColor();
        }
        static void ShowDangerLine(string value)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(value.PadRight(value.Length));
            Console.ResetColor();
        }
    }
}