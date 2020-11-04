using System;
using System.Collections.Generic;
using System.Text;

namespace PortSimulation
{
    class Katamaran : Boat
    {
        public int AmountBeds { get; set; }
        Func<int> AssignBeds = () => (new Random()).Next(1, 5);

        public Katamaran(bool create = false)
        {
            if (create)
            {
                InitBoat();
                AmountBeds = AssignBeds();
            }
        }
    }
}
