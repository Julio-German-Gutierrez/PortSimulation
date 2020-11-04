using System;
using System.Collections.Generic;
using System.Text;

namespace PortSimulation
{
    class SailsBoat : Boat
    {
        public decimal BoatLenght { get; set; } // 10 to 60 feet
        Func<decimal> AssignLenght = () => (new Random()).Next(10, 61);

        public SailsBoat(bool create = false)
        {
            if (create)
            {
                InitBoat();
                BoatLenght = AssignLenght();
            }
        }
    }
}
