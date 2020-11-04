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
        //public char[] docks;
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
             * docks is a char[64] representing each of the 64 available docks.
             * A = Available.
             * F = Fully taken by a single boat.
             * H = Half taken by rowboat.
             * D = Double. Fully taken by two rowboats.
             * P = Partially taken. Meaning that a big boat is partially taking it.
             * We initiallite docks with full availability.
             */
            //docks = (new string('A', 64)).ToCharArray();
            docksNorth = (new string('A', 32)).ToCharArray();
            docksSouth = (new string('A', 32)).ToCharArray();
        }

        public bool CheckSpaceFor(Boat b)
        {
            /*
             * The method I use to find the first available space is very simple.
             * 
             * "docks" is a char[64] array. Each char in the array represents a dock.
             * A char with a code 'A' means the dock is available.
             * First I transform the "docks" array into a normal String "docksToString", because I need
             * to use the "String.IndexOf(stringToSearch)" method, which returns the index of
             * the first occurrence of the "stringToSearch" parameter.
             * 
             * It works like this:
             *      A Cargoboat will require 4 docks. So, the "CargoBoat.BoatSpacesRequired" property,
             *      will contain the string: "AAAA".
             *      Now, calling docksToString.IndexOf(CargoBoat.BoatSpacesRequired) will return the index
             *      of the first 4 spaces ("AAAA") available. or -1 if there is no space available.
             */
            //string docksToString = new string(docks);
            string docksNorthString = new string(docksNorth);
            string docksSouthString = new string(docksSouth);
            int index;
            //int indexNorth;
            //int indexSouth;

            //if (b.Type == Boat.TypeBoat.Rowboat && (index = docksToString.IndexOf(Port.RowBoat)) > -1) // cambie 'H' por 'R'
            //{
            //    b.AcceptedInPort = true;
            //    b.ParkingPlace = index;
            //}
            if (b.Type == Boat.TypeBoat.Rowboat && (index = docksNorthString.IndexOf(Port.RowBoat)) > -1) // cambie 'H' por 'R'
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.North;
                b.ParkingPlace = index;
            }
            else if (b.Type == Boat.TypeBoat.Rowboat && (index = docksSouthString.IndexOf(Port.RowBoat)) > -1)
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.South;
                b.ParkingPlace = index;
            }
            else if ((index = docksNorthString.IndexOf(b.BoatSpacesRequired)) > -1)
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.North;
                b.ParkingPlace = index;
            }
            else if ((index = docksSouthString.IndexOf(b.BoatSpacesRequired)) > -1)
            {
                b.AcceptedInPort = true;
                b.ParkingPort = PortSide.South;
                b.ParkingPlace = index;
            }
            else
            {
                b.AcceptedInPort = false;
            }

            //else
            //{
            //    index = docksToString.IndexOf(b.BoatSpacesRequired); // index >=  0 (Space available)
            //                                                         // index == -1 (No space found)
            //    if (index > -1)
            //    {
            //        b.AcceptedInPort = true;
            //        b.ParkingPlace = index;
            //    }
            //    else
            //    {
            //        b.AcceptedInPort = false;
            //    }
            //}

            if (index > -1 && b.ParkingPort == PortSide.North)
            {
                switch (docksNorth[index])
                {
                    case 'A':
                        {
                            if (b.Type == Boat.TypeBoat.Rowboat)
                            {
                                docksNorth[index] = Port.RowBoat;
                            }
                            else if (b is MotorBoat)
                            {
                                docksNorth[index] = Port.MotorBoat;
                            }
                            else
                            {
                                for (int i = 0; i < b.BoatSpacesRequired.Length; i++)
                                {
                                    if (b is SailsBoat)
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
                            }
                            break;
                        }
                    case 'R':
                        {
                            docksNorth[index] = Port.DoubleRowBoats;
                            break;
                        }
                }
            }
            else if (index > -1 && b.ParkingPort == PortSide.South)
            {
                switch (docksSouth[index])
                {
                    case 'A':
                        {
                            if (b.Type == Boat.TypeBoat.Rowboat)
                            {
                                docksSouth[index] = Port.RowBoat;
                            }
                            else if (b is MotorBoat)
                            {
                                docksSouth[index] = Port.MotorBoat;
                            }
                            else
                            {
                                for (int i = 0; i < b.BoatSpacesRequired.Length; i++)
                                {
                                    if (b is SailsBoat)
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
                            }
                            break;
                        }
                    case 'R':
                        {
                            docksSouth[index] = Port.DoubleRowBoats;
                            break;
                        }
                }
            }

            if (b.AcceptedInPort) boats.Add(b);

            return b.AcceptedInPort;
        }

        public void ReleaseBoat(Boat b)
        {
            int index = b.ParkingPlace;
            int spacesToRestore = b.BoatSpacesRequired.Length;

            if (b.ParkingPort == PortSide.North)
            {
                for (int i = index; i < index + spacesToRestore; i++)
                {
                    docksNorth[i] = Port.Available;
                }
            }
            else
            {
                for (int i = index; i < index + spacesToRestore; i++)
                {
                    docksSouth[i] = Port.Available;
                }
            }

            boats.Remove(b);
        }
    }
}
