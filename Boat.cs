using System;
using System.Collections.Generic;
using System.Text;

namespace PortSimulation
{
    abstract class Boat
    {
        public string ID { get; set; }
        public decimal Weight { get; set; }
        public int MaxSpeedNots { get; set; }
        public bool AcceptedInPort { get; set; }
        public string BoatSpacesRequired { get; set; }
        public int ParkingPlace { get; set; }
        public PortSide ParkingPort { get; set; }
        public int DayAllowedEntrance { get; set; }
        public TypeBoat Type { get; set; }

        public enum TypeBoat
        {
            Rowboat,
            Motorboat,
            Sailsboat,
            Cargoboat,
            Katamaran
        }

        protected void InitBoat()
        {
            ID = AssignID();
            Weight = AssignWeight();
            MaxSpeedNots = AssignMaxSpeedNots();
            BoatSpacesRequired = AssignBoatStringType();
        }

        private string AssignBoatStringType()
        {
            string boatType = "";

            if (this is RowBoat) boatType = "A";
            else if (this is MotorBoat) boatType = "A";
            else if (this is SailsBoat) boatType = "AA";
            else if (this is CargoBoat) boatType = "AAAA";
            else if (this is Katamaran) boatType = "AAA";

            return boatType;
        }

        protected string AssignID()
        {
            // Full ID is composed of IDType and IDNumber
            string IDType = "";
            string IDNumber = "";
            int numberOfDigits = 3; // Number of digits after de type.

            if (this is RowBoat)
            {
                Type = Boat.TypeBoat.Rowboat;
                IDType = "R";
            }
            else if (this is MotorBoat)
            {
                Type = Boat.TypeBoat.Motorboat;
                IDType = "M";
            }
            else if (this is SailsBoat)
            {
                Type = Boat.TypeBoat.Sailsboat;
                IDType = "S";
            }
            else if (this is CargoBoat)
            {
                Type = Boat.TypeBoat.Cargoboat;
                IDType = "C";
            }
            else if (this is Katamaran)
            {
                Type = Boat.TypeBoat.Katamaran;
                IDType = "K";
            }

            for (int i = 0; i < numberOfDigits; i++)
            {
                IDNumber += (char)(new Random().Next(65, 91)); // A=65 -> Z=90
            }

            return IDType + "-" + IDNumber;
        }

        protected decimal AssignWeight()
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

        protected int AssignMaxSpeedNots()
        {
            int maxSpeed = 0;

            if (this is RowBoat) maxSpeed = (new Random()).Next(0, 4);
            else if (this is MotorBoat) maxSpeed = (new Random()).Next(0, 61);
            else if (this is SailsBoat) maxSpeed = (new Random()).Next(0, 13);
            else if (this is CargoBoat) maxSpeed = (new Random()).Next(0, 21);
            else if (this is Katamaran) maxSpeed = (new Random()).Next(0, 12);

            return maxSpeed;
        }

        /*
         * How much time in port the katamaran?
         */
        internal bool CheckForDeparture()
        {
            bool departure = false;

            int daysInPort = MainWindow.Day - DayAllowedEntrance;
            switch(daysInPort)
            {
                case 1:
                    {
                        if(this is RowBoat)
                        {
                            departure = true;
                        }
                        break;
                    }
                case 3:
                    {
                        if (this is MotorBoat)
                        {
                            departure = true;
                        }
                        break;
                    }
                case 4:
                    {
                        if (this is SailsBoat)
                        {
                            departure = true;
                        }
                        break;
                    }
                case 5:
                    {
                        if (this is Katamaran)
                        {
                            departure = true;
                        }
                        break;
                    }
                case 6:
                    {
                        if (this is CargoBoat)
                        {
                            departure = true;
                        }
                        break;
                    }
                default:
                    break;
            }

            return departure;
        }
    }
}
