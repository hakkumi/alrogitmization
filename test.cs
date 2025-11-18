using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("введите номер задания 1-100: ");
            int task = int.Parse(Console.ReadLine());
            switch (task)
            {
                case 1:
                    Console.WriteLine("Hello world!");
                    break;
                case 2:
                    Console.WriteLine("введите первое число - ");
                    int a = int.Parse(Console.ReadLine());
                    Console.WriteLine("введите второе число - ");
                    int b = int.Parse(Console.ReadLine());
                    int sum = a + b;
                    int raz = a - b;
                    int pr = a * b;
                    int ch = a / b;
                    Console.WriteLine("сумма - " + sum);
                    Console.WriteLine("разность - " + raz);
                    Console.WriteLine("произведение - " + pr);
                    Console.WriteLine("частное - " + ch);
                    break;
                case 3:
                    Console.WriteLine("введите температуру в градусах цельсия - ");
                    int celsium = int.Parse(Console.ReadLine());
                    Console.WriteLine("температура в градусах фаренгейта - " + ((celsium * 9/5) + 32));
                    break;
                case 4:
                    Console.WriteLine("введите длину прямоугольника - ");
                    int height = int.Parse(Console.ReadLine());
                    Console.WriteLine("введите ширину прямоугольника - ");
                    int weight = int.Parse(Console.ReadLine());
                    int S = height*weight;
                    int P = (height*2) + (weight*2);
                    Console.WriteLine("периметр прямоугольника - " + P);
                    Console.WriteLine("площадь прямоугольника - " +  S);
                    break;
                case 5:
                    Console.WriteLine("введите первое число - ");
                    int ost1 = int.Parse(Console.ReadLine());
                    Console.WriteLine("введите второе число - ");
                    int ost2 = int.Parse(Console.ReadLine());

                    Console.WriteLine("остаток от деления первого числа на второе - " + ost1 % ost2);
                    break;
                case 6:
                    int obm1 = 10;
                    int obm2 = 15;
                    Console.WriteLine($"{obm1}-{obm2}");
                    (obm1, obm2) = (obm2, obm1);
                    Console.WriteLine($"{obm1}-{obm2}");
                    break;
                case 7:
                    Console.WriteLine("введите первое число - ");
                    double sred1 = double.Parse(Console.ReadLine());
                    Console.WriteLine("введите второе число - ");
                    double sred2 = double.Parse(Console.ReadLine());
                    Console.WriteLine("введите третье число - ");
                    double sred3 = double.Parse(Console.ReadLine());
                    Console.WriteLine("среднее арифметическое - " + ((sred1 + sred2 + sred3)/3));
                    break;
                case 8:
                    Console.WriteLine("введите радиус круга - ");
                    int radius = int.Parse(Console.ReadLine());
                    Console.WriteLine("радиус круга - " + (radius*3,14));
                    break;
                case 9:
                    Console.WriteLine("введите свой возраст - ");
                    int age = int.Parse(Console.ReadLine());
                    Console.WriteLine("ваш возраст в секундах - " + age * 31536000);
                    break;
                case 10:
                    Console.WriteLine("введите сумму в рублях - ");
                    double money = double.Parse(Console.ReadLine());
                    double dollars = 81.13;
                    double euros = 94.42;
                    Console.WriteLine("сумма в евро - " + money / euros);
                    Console.WriteLine("сумма в долларах - "+ money/dollars);
                    break;
                case 11:
                    Console.WriteLine("введите число для проверки четности - ");
                    int test = int.Parse(Console.ReadLine());
                    if (test % 2 == 0)
                    {
                        Console.WriteLine("число четное");
                    }
                    else
                    {
                        Console.WriteLine("число нечетное");
                    }
                        break;
                case 12:
                    Console.WriteLine("введите первое число - ");
                    int max1 = int.Parse(Console.ReadLine());
                    Console.WriteLine("введите второе число - ");
                    int max2 = int.Parse(Console.ReadLine());
                    Console.WriteLine("введите третье число - ");
                    int max3 = int.Parse(Console.ReadLine());

                    if (max1 > max2 && max1 > max3)
                    {
                        Console.WriteLine("число 1 больше всех");
                    }
                    else if (max2>max1 && max2 > max3)
                    {
                        Console.WriteLine("число 2 больше всех");
                    }
                    else if (max3>max1 && max3 > max2) {     
                    {
                        Console.WriteLine("число 3 больше всех");
                    }

                        break;

            }   
        }
    }
}
