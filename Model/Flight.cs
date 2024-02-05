using System.Text.Json.Serialization;

namespace FlightSystem.Model
{
    // ���� �� ����������� ���������� ��� ����
    public class Flight
    {
        // ����� �����
        public string FlightNumber { get; set; }
        
        // ����� ���������
        public string Airline { get; set; }

        // ���� �����������
        public string Destination { get; set; }

        //��� �������
        public DateTime DepartureTime { get; set; }

        //��� ��������
        public DateTime ArrivalTime { get; set; }

        //������ ����� (OnTime, Delayed, Cancelled, Boarding, InFlight)
        public FlightStatus Status { get; set; }

        // ��������� �����
        public TimeSpan Duration { get; set; }

        //��� ���������� �����
        public string AircraftType { get; set; }

        //������� ���������
        public string Terminal { get; set; }
    }
}