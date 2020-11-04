using System;

namespace PortSimulation
{
    class RowBoat : Boat
    {
        public int Passengers { get; set; }
        Func<int> AssignPassengers = () => (new Random()).Next(1, 7);

        public RowBoat(bool create = false)
        {
            if (create)
            {
                InitBoat();
                Passengers = AssignPassengers();
            }
        }
    }
}
