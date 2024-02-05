using System.Text.Json.Serialization;

namespace FlightSystem.Model
{
    // клас що представляє інформацію про рейс
    public class Flight
    {
        // номер рейсу
        public string FlightNumber { get; set; }
        
        // назва авіакомпанії
        public string Airline { get; set; }

        // місце призначення
        public string Destination { get; set; }

        //час вильоту
        public DateTime DepartureTime { get; set; }

        //час прибуття
        public DateTime ArrivalTime { get; set; }

        //статус рейсу (OnTime, Delayed, Cancelled, Boarding, InFlight)
        public FlightStatus Status { get; set; }

        // тривалість рейсу
        public TimeSpan Duration { get; set; }

        //тип повітряного судна
        public string AircraftType { get; set; }

        //термінал аеропорту
        public string Terminal { get; set; }
    }
}