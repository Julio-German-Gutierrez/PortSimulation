using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PortSimulation
{
    enum PortSide
    {
        North,
        South
    }

    class Port
    {
        public char[] docksNorth;
        public char[] docksSouth;

        public List<Boat> boats = new List<Boat>();
        
        public readonly static char Available = 'A';
        public readonly static char DoubleRowBoats = 'D';
        public readonly static char RowBoat = 'R';
        public readonly static char MotorBoat = 'M';
        public readonly static char SailsBoat = 'S';
        public readonly static char CargoBoat = 'C';
        public readonly static char Katamaran = 'K';

        public Port()
        {
            /*
             * docksNorth and docksSouth are two char[32] arrays representing
             * each of the bays in the port.
             * 
             * Each dock has a code as follows:
             * A = Available.
             * D = Fully taken by two Rowboats.
             * R = Half occupied by a single Rowboat.
             * M = Fully taken by a Motorboat.
             * S = Partially taken by a Sailsboat.
             * C = Partially taken by a Cargoboat.
             * K = Partially taken by a Katamaran.
             * 
             * We initiallite the docks with full availability.
             */
            docksNorth = (new string('A', 32)).ToCharArray();
            docksSouth = (new string('A', 32)).ToCharArray();
        }

        public bool CheckSpaceFor(Boat b)
        {
            /*
             * The method I use to find the first available space is very simple.
             * My docks are represented by char arrays. So, let's say I want
             * to check availability in the northern bay.
             * 
             * First, I will convert my char[] array to a string:
             * 
             *      string docksNorthString = new string(docksNorth);
             * 
             * And then I will use the IndexOf() method to search for a pattern:
             * 
             *      docksNorthString.IndexOf( "AA" );
             * 
             * In the example, "AA" is the string pattern of the Sailsboat, which occupies
             * 2 dock places.
             * Each boat has a different string pattern in the property BoatSpacesRequired:
             *      Rowboat has "A".
             *      Motorboat has "A".
             *      Cargoboat has "AAAA"
             *      and so on..
             * 
             * If the pattern if found in the string, it means that a space if found for the boat.
             * The IndexOf() method will return the index of the first ocurrence
             * of the pattern in the string.
             * 
             * Then I will use that index information to work on the docksNorth array,
             * because the index in the string will match the index in the char[].
             * 
             */
            string docksNorthString = new string(docksNorth); // Transfer the state of the docks to string
            string docksSouthString = new string(docksSouth); // Transfer the state of the docks to string
            int index;

            /*
             * Rowboat is a special case because they use only half the space of a dock.
             * In order to find the best place for them, I first need to check if there is
             * an actual dock which if half taken by another Rowboat.
             * 
             * So, I will check for a dock with the code "R" (Half taken) in both bays North and South.
             * If a dock with code "R" is found, I will place the newcomer Rowboat there and change
             * the dock code to "D", which means: Dock fully taken by two Rowboats.
             * If no "R" is found, I will check for an available dock: "A".
             * If no "A" is found in either of the bays, the boat is rejected.
             * 
             * In the case of the other type boats, we will simply search for a match of their own
             * string pattern into the dock's string.
             * Then again, if no match is found, the boat is rejected.
             */
            if (b.Type == TypeBoat.Rowboat && (index = docksNorthString.IndexOf(Port.RowBoat)) > -1) // -1 means "Not found"
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.North;
                b.ParkingPlace = index;
            }
            else if (b.Type == TypeBoat.Rowboat && (index = docksSouthString.IndexOf(Port.RowBoat)) > -1)//Now in the south bay.
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.South;
                b.ParkingPlace = index;
            }
            else if ((index = docksNorthString.IndexOf(b.BoatSpacesRequired)) > -1) // Ok, it is not a Rowboat...
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.North;
                b.ParkingPlace = index;
            }
            else if ((index = docksSouthString.IndexOf(b.BoatSpacesRequired)) > -1) // Maybe in the south bay?
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.South;
                b.ParkingPlace = index;
            }
            else
            {
                b.AcceptedInPort = false; // We are sorry, there is no space available.
            }
            
            /*
             * Now we set the days allowed to stay in the port, according to the type of boat.
             * With each new day at the port, this number will decrease automatically.
             * When it reaches 0. It will be time to leave.
             */
            if (b.AcceptedInPort)
            {
                switch (b.Type)
                {
                    case TypeBoat.Rowboat:
                        {
                            b.DaysRemaining = 1;
                            break;
                        }
                    case TypeBoat.Motorboat:
                        {
                            b.DaysRemaining = 3;
                            break;
                        }
                    case TypeBoat.Sailsboat:
                        {
                            b.DaysRemaining = 4;
                            break;
                        }
                    case TypeBoat.Cargoboat:
                        {
                            b.DaysRemaining = 6;
                            break;
                        }
                    case TypeBoat.Katamaran:
                        {
                            b.DaysRemaining = 3;
                            break;
                        }
                }
            }

            /*
             * Now it is time to reassign the codes at the docks to show the new status.
             * We check if the boat has been assign to the north or the south bay.
             * And commit the changes.
             */
            if (index > -1 && b.ParkingPort == PortSide.North)
            {
                switch (docksNorth[index])
                {
                    case 'R':
                        {
                            docksNorth[index] = Port.DoubleRowBoats;
                            break;
                        }
                    case 'A':
                        {
                            for (int i = 0; i < b.BoatSpacesRequired.Length; i++)
                            {
                                if (b is RowBoat)
                                {
                                    docksNorth[index + i] = Port.RowBoat;
                                }
                                if (b is MotorBoat)
                                {
                                    docksNorth[index + i] = Port.MotorBoat;
                                }
                                else if (b is SailsBoat)
                                {
                                    docksNorth[index + i] = Port.SailsBoat;
                                }
                                else if(b is CargoBoat)
                                {
                                    docksNorth[index + i] = Port.CargoBoat;
                                }
                                else if (b is Katamaran)
                                {
                                    docksNorth[index + i] = Port.Katamaran;
                                }
                            }
                            break;
                        }
                }
            }
            else if (index > -1 && b.ParkingPort == PortSide.South)
            {
                switch (docksSouth[index])
                {
                    case 'R':
                        {
                            docksSouth[index] = Port.DoubleRowBoats;
                            break;
                        }
                    case 'A':
                        {
                            for (int i = 0; i < b.BoatSpacesRequired.Length; i++)
                            {
                                if (b is RowBoat)
                                {
                                    docksSouth[index + i] = Port.RowBoat;
                                }
                                if (b is MotorBoat)
                                {
                                    docksSouth[index + i] = Port.MotorBoat;
                                }
                                else if (b is SailsBoat)
                                {
                                    docksSouth[index + i] = Port.SailsBoat;
                                }
                                else if (b is CargoBoat)
                                {
                                    docksSouth[index + i] = Port.CargoBoat;
                                }
                                else if (b is Katamaran)
                                {
                                    docksSouth[index + i] = Port.Katamaran;
                                }
                            }
                            break;
                        }
                }
            }

            return b.AcceptedInPort;
        }

        public void ReleaseBoat(Boat b)
        {
            int index = b.ParkingPlace;
            int spacesToRestore = b.BoatSpacesRequired.Length;

            switch (b.ParkingPort)
            {
                case PortSide.North:
                    {
                        for (int i = index; i < index + spacesToRestore; i++)
                        {
                            docksNorth[i] = Port.Available; // Fill with code "A" for Available.
                        }
                        break;
                    }
                case PortSide.South:
                    {
                        for (int i = index; i < index + spacesToRestore; i++)
                        {
                            docksSouth[i] = Port.Available; // Fill with code "A" for Available.
                        }
                        break;
                    }
            }

            boats.Remove(b);
        }
    }
}
