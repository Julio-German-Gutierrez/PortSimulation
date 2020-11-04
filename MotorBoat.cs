using System;
using System.Collections.Generic;
using System.Text;

namespace PortSimulation
{
    class MotorBoat : Boat
    {
        public int HorsePower { get; set; }
        Func<int> AssignHorsePower = () => (new Random()).Next(10, 1001);

        public MotorBoat(bool create = false)
        {
            if (create)
            {
                InitBoat();
                HorsePower = AssignHorsePower();
            }
        }
    }
}
