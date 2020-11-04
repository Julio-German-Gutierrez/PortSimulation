using System;
using System.Collections.Generic;
using System.Text;

namespace PortSimulation
{
    class CargoBoat : Boat
    {
        public int NumberContainers { get; set; }
        Func<int> AssignNumberContainers = () => (new Random()).Next(0, 501);

        public CargoBoat(bool create = false)
        {
            if (create)
            {
                InitBoat();
                NumberContainers = AssignNumberContainers();
            }
        }
    }
}
