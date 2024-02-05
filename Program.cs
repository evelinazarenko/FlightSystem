using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using FlightSystem.Model;
using Microsoft.VisualBasic;

namespace FlightSystem
{
    public class Program
    {
        // перелік для меню програми
        public enum MenuOptions
        {
            AddFlightInfo = 1,
            RemoveFlightInfo = 2,
            GetAllFlightByAirCompany = 3,
            GetAllFlightByStatus = 4,
            Exit = 5,

        }

        // отримання від користувача інформації про рейс
        public static Flight GetFlightInfoFromUser()
        {
            var flightInfo = new Flight();
            Console.WriteLine(@"Enter aircompany name:\t");
            flightInfo.Airline = Console.ReadLine();
            Console.WriteLine(@"Enter aircraft number name:\t");
            flightInfo.FlightNumber = Console.ReadLine();

            return flightInfo;
        }

        // отримання від користувача статусу рейсу
        public static FlightStatus GetFlightStatusFromUser()
        {
            Console.WriteLine(@"Select flight status:");
            Console.WriteLine(@"1.) OnTime");
            Console.WriteLine(@"2.) Delayed");
            Console.WriteLine(@"3.) Cancelled");
            Console.WriteLine(@"4.) Boarding");
            Console.WriteLine(@"5.) InFlight");

            Byte flightStatus;

            while (!Byte.TryParse(Console.ReadLine(), out flightStatus))
            {
                Console.WriteLine($"Введено неправильні дані, перевірте та спробуйте знову");
            }

            FlightStatus selectedflightStatus = (FlightStatus)(flightStatus - 1);

            return selectedflightStatus;

        }

        // обробка меню програми
        public static void Menu(FlightInformationSystem flightSystem)
        {

            Console.WriteLine("Select menu item:");
            Console.WriteLine("1.) Add flight info");
            Console.WriteLine("2.) Remove flight info");
            Console.WriteLine("3.) Get all flights by aircompany");
            Console.WriteLine("4.) Get all flights by status");
            Console.WriteLine("5.) Exit");

            Byte menuOption;

            while (!Byte.TryParse(Console.ReadLine(), out menuOption))
            {
                Console.WriteLine($"Incorrect input, please check and try again");
            }

            MenuOptions selectedOption = (MenuOptions)menuOption;

            switch (selectedOption)
            {
                // додавання інформації про рейс
                case MenuOptions.AddFlightInfo:
                    flightSystem.AddFlightInfo(GetFlightInfoFromUser());
                    Console.ReadLine();
                    break;
                case MenuOptions.RemoveFlightInfo:
                    // видалення інформації про рейс
                    Console.ReadLine();
                    break;
                case MenuOptions.GetAllFlightByAirCompany:
                    Console.WriteLine(@"Enter aircompany name");
                    // отримання всіх рейсів за назвою авіакомпанії
                    flightSystem.GetAllFlightsByAirCompany(Console.ReadLine());
                    Console.ReadLine();
                    break;
                case MenuOptions.GetAllFlightByStatus:
                    // отримання всіх рейсів за статусом
                    flightSystem.GetAllFlightsByStatus(GetFlightStatusFromUser());
                    Console.ReadLine();
                    break;
                case MenuOptions.Exit:
                    // завершення програми
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

        }
        static void Main(string[] args)
        {
            // Підтримка кириличних символів
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            // шлях до файлів з даними про рейси
            var sourceFilePath = Path.Combine(AppContext.BaseDirectory, @"flights_data.json");
            var outputFilePath = Path.Combine(AppContext.BaseDirectory, @"result.json");

            // створення екземпляра системи інформації про рейси
            var flightSystem = new FlightInformationSystem(sourceFilePath,
                                                          outputFilePath);
            // безкінченний цикл виклику меню
            while (true)
            {
                Menu(flightSystem);
            }

        }
    }
}

