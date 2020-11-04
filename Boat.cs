using System;
using System.Collections.Generic;
using System.Text;

namespace PortSimulation
{
    enum TypeBoat
    {
        Rowboat,
        Motorboat,
        Sailsboat,
        Cargoboat,
        Katamaran
    }

    abstract class Boat
    {
        public string ID { get; set; }
        public decimal Weight { get; set; }
        public int MaxSpeedNots { get; set; }
        public bool AcceptedInPort { get; set; }
        public string BoatSpacesRequired { get; set; }
        public int ParkingPlace { get; set; }
        public PortSide ParkingPort { get; set; }
        public int DaysRemaining { get; set; }
        public TypeBoat Type { get; set; }

        protected void InitBoat()
        {
            ID = AssignRandomID();
            Weight = AssignRandomWeight();
            MaxSpeedNots = AssignRandomMaxSpeedNots();
            BoatSpacesRequired = AssignBoatStringType();
        }

        private string AssignBoatStringType()
        {
            string sType = "";

            if (this is RowBoat) sType = "A";
            else if (this is MotorBoat) sType = "A";
            else if (this is SailsBoat) sType = "AA";
            else if (this is CargoBoat) sType = "AAAA";
            else if (this is Katamaran) sType = "AAA";

            return sType;
        }

        protected string AssignRandomID()
        {
            // An ID is composed of IDType and IDNumber
            string IDType = "";
            string IDLetters = "";
            int numberOfDigits = 3; // Amount of ID letters.

            if (this is RowBoat) IDType = "R";
            else if (this is MotorBoat) IDType = "M";
            else if (this is SailsBoat) IDType = "S";
            else if (this is CargoBoat) IDType = "C";
            else if (this is Katamaran) IDType = "K";

            for (int i = 0; i < numberOfDigits; i++)
            {
                IDLetters += (char)(new Random().Next(65, 91)); // A=65 -> Z=90
            }

            return $"{IDType}-{IDLetters}";
        }

        protected decimal AssignRandomWeight()
        {
            decimal weight = 0;

            // Apointed weights according to excercise.
            if (this is RowBoat) weight = (new Random()).Next(100, 301);
            else if (this is MotorBoat) weight = (new Random()).Next(200, 3001);
            else if (this is SailsBoat) weight = (new Random()).Next(800, 6001);
            else if (this is CargoBoat) weight = (new Random()).Next(3000, 20001);
            else if (this is Katamaran) weight = (new Random()).Next(1200, 8001);

            return weight;
        }

        protected int AssignRandomMaxSpeedNots()
        {
            int maxSpeed = 0;

            if (this is RowBoat) maxSpeed = (new Random()).Next(1, 4);
            else if (this is MotorBoat) maxSpeed = (new Random()).Next(1, 61);
            else if (this is SailsBoat) maxSpeed = (new Random()).Next(1, 13);
            else if (this is CargoBoat) maxSpeed = (new Random()).Next(1, 21);
            else if (this is Katamaran) maxSpeed = (new Random()).Next(1, 12);

            return maxSpeed;
        }

        internal bool CheckForDeparture()
        {
            if (DaysRemaining == 0) return true;
            else return false;
        }
    }
}
