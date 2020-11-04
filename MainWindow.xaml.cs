using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace PortSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Port port = new Port();
        static List<Boat> incomingBoats = new List<Boat>();
        static List<string> rejectedBoats = new List<string>();
        static public int Day { get; set; }

        public MainWindow()
        {
            // Little trick to give some time to the Splash screen.
            System.Threading.Thread.Sleep(3000);

            InitializeComponent();

            // Check for a "state.sav" file.
            // If not found, we initialize everything at zero.
            if (!LoadStatusFromFile())
            {
                ClearPort();
            }
        }

        private void ClearPort()
        {
            Day = 0;
            dayWin.Content = "DAY: " + Day;

            // Print basic start information.
            DockListPrint();

            // The main button's text is changed to "Start".
            nextButton.Content = "Start";
        }

        private void DockListPrint()
        {
            string docksN = "";
            string docksS = "";

            // For better visibility I added a space in between each character.
            // Remember that the docks are represented by char[] arrays.
            for (int i = 0; i < port.docksNorth.Length; i++)
            {
                docksN += port.docksNorth[i] + " ";
                docksS += port.docksSouth[i] + " ";
            }

            // Now we print the list of docks with spaces in between for better visibility.
            dockListWin.Content = "North docks: " + docksN + "\n" +
                                  "South docks: " + docksS;

        }

        private void Print()
        {
            dayWin.Content = "DAY: " + Day;

            // Like I said, this method simply prints the char[] with spaces in between each character.
            DockListPrint();

            // Now we print the information on the North bay only.
            foreach (Boat b in port.boats
                .Where(b => b.ParkingPort == PortSide.North) // Only boats in he north bay
                .OrderBy(b => b.ParkingPlace))               // And we sort them by parking place.
            {
                string s = $"Dock: {b.ParkingPlace,-5:00}" +
                           $"Type: {Enum.GetName(typeof(TypeBoat), b.Type),-12}" +
                           $"ID: {b.ID,-8}" +
                           $"Weight: {b.Weight,6}   " +
                           $"MaxSpeed: {b.MaxSpeedNots,3}   ";

                string special = "";
                switch (b.Type)
                {
                    case TypeBoat.Rowboat:
                        {
                            special = $"{"Passengers:",-12}{((RowBoat)b).Passengers,4}";
                            break;
                        }
                    case TypeBoat.Motorboat:
                        {
                            special = $"{"H.Power: ",-12}{((MotorBoat)b).HorsePower,4}";
                            break;
                        }
                    case TypeBoat.Sailsboat:
                        {
                            special = $"{"Lenght: ",-12}{((SailsBoat)b).BoatLenght,4}";
                            break;
                        }
                    case TypeBoat.Cargoboat:
                        {
                            special = $"{"Containers:",-12}{((CargoBoat)b).NumberContainers,4}";
                            break;
                        }
                    case TypeBoat.Katamaran:
                        {
                            special = $"{"Num Beds:",-12}{((Katamaran)b).AmountBeds,4}";
                            break;
                        }
                    default:
                        break;
                }

                // We print the normal attributes "s" followed by the special one "special".
                listNorthWin.Items.Add(s + special);
            }

            foreach (Boat b in port.boats
                .Where(b => b.ParkingPort == PortSide.South) // Now is the turn for the South bay.
                .OrderBy(b => b.ParkingPlace))               // Then again we order them by parking place.
            {
                string s = $"Dock: {b.ParkingPlace,-5:00}" +
                           $"Type: {Enum.GetName(typeof(TypeBoat), b.Type),-12}" +
                           $"ID: {b.ID,-8}" +
                           $"Weight: {b.Weight,6}   " +
                           $"MaxSpeed: {b.MaxSpeedNots,3}   ";

                string special = "";
                switch (b.Type)
                {
                    case TypeBoat.Rowboat:
                        {
                            special = $"{"Passengers:",-12}{((RowBoat)b).Passengers,4}";
                            break;
                        }
                    case TypeBoat.Motorboat:
                        {
                            special = $"{"H.Power: ",-12}{((MotorBoat)b).HorsePower,4}";
                            break;
                        }
                    case TypeBoat.Sailsboat:
                        {
                            special = $"{"Lenght: ",-12}{((SailsBoat)b).BoatLenght,4}";
                            break;
                        }
                    case TypeBoat.Cargoboat:
                        {
                            special = $"{"Containers:",-12}{((CargoBoat)b).NumberContainers,4}";
                            break;
                        }
                    case TypeBoat.Katamaran:
                        {
                            special = $"{"Num Beds:",-12}{((Katamaran)b).AmountBeds,4}";
                            break;
                        }
                    default:
                        break;
                }

                listSouthWin.Items.Add(s + special);
            }

            // No is the turn for the boats waiting to be accepted in port.
            // We print the list in the corresponding place in the application.
            foreach (Boat b in incomingBoats)
            {
                string s = $"ID: {b.ID,-7}" +
                           $"Type: {Enum.GetName(typeof(TypeBoat), b.Type),-11}" +
                           $"{((!b.AcceptedInPort) ? "REJECT" : "") }"; // If the boat was not accepted!!!
                incomingWin.Items.Add(s);
            }

            PrintResume();
        }

        private bool LoadStatusFromFile()
        {
            bool fileLocated = false;

            if (File.Exists("state.sav"))
            {
                fileLocated = true;

                string file = File.ReadAllText("state.sav");
                Match m = Regex.Match(file, @"<day>(\d*)</day>");

                Day = int.Parse(m.Groups[1].Value);

                m = Regex.Match(file, @"<docksNorth>(\w*)</docksNorth>");
                port.docksNorth = m.Groups[1].Value.ToCharArray();
                m = Regex.Match(file, @"<docksSouth>(\w*)</docksSouth>");
                port.docksSouth = m.Groups[1].Value.ToCharArray();

                MatchCollection mID = Regex.Matches(file, @"<ID>(\S*)</ID>");
                MatchCollection mParkingPort = Regex.Matches(file, @"<ParkingPort>(\w*)</ParkingPort>");
                MatchCollection mParkingPlace = Regex.Matches(file, @"<ParkingPlace>(\d*)</ParkingPlace>");
                MatchCollection mBoatSpacesRequired = Regex.Matches(file, @"<BoatSpacesRequired>(\w*)</BoatSpacesRequired>");
                MatchCollection mMaxSpeedNots = Regex.Matches(file, @"<MaxSpeedNots>(\d*)</MaxSpeedNots>");
                MatchCollection mWeight = Regex.Matches(file, @"<Weight>(\d*)</Weight>");
                MatchCollection mDaysRemaining = Regex.Matches(file, @"<DaysRemaining>(\d*)</DaysRemaining>");
                //MatchCollection mDayAllowedEntrance = Regex.Matches(file, @"<DayAllowedEntrance>(\d*)</DayAllowedEntrance>");
                MatchCollection mType = Regex.Matches(file, @"<Type>(\w*)</Type>");
                MatchCollection mSpecial = Regex.Matches(file, @"</Type>\W*<\w*>(\d*)</\w*>");

                for (int i = 0; i < mID.Count; i++)
                {
                    string id = mID[i].Groups[1].Value;
                    string parkingPort = mParkingPort[i].Groups[1].Value;
                    int parkingPlace = int.Parse(mParkingPlace[i].Groups[1].Value);
                    string boatSpacesRequired = mBoatSpacesRequired[i].Groups[1].Value;
                    int maxSpeedNots = int.Parse(mMaxSpeedNots[i].Groups[1].Value);
                    decimal weight = decimal.Parse(mWeight[i].Groups[1].Value);
                    int dayAllowedEntrance = int.Parse(mDaysRemaining[i].Groups[1].Value);
                    //int dayAllowedEntrance = int.Parse(mDayAllowedEntrance[i].Groups[1].Value);
                    string type = mType[i].Groups[1].Value;
                    int special = int.Parse(mSpecial[i].Groups[1].Value);

                    Boat b = new RowBoat();
                    switch (type)
                    {
                        case "Rowboat":
                            {
                                b.Type = TypeBoat.Rowboat;
                                ((RowBoat)b).Passengers = special;
                                break;
                            }
                        case "Motorboat":
                            {
                                b = new MotorBoat();
                                b.Type = TypeBoat.Motorboat;
                                ((MotorBoat)b).HorsePower = special;
                                break;
                            }
                        case "Sailsboat":
                            {
                                b = new SailsBoat();
                                b.Type = TypeBoat.Sailsboat;
                                ((SailsBoat)b).BoatLenght = special;
                                break;
                            }
                        case "Cargoboat":
                            {
                                b = new CargoBoat();
                                b.Type = TypeBoat.Cargoboat;
                                ((CargoBoat)b).NumberContainers = special;
                                break;
                            }
                    }

                    switch (parkingPort)
                    {
                        case "North":
                            {
                                b.ParkingPort = PortSide.North;
                                break;
                            }
                        case "South":
                            {
                                b.ParkingPort = PortSide.South;
                                break;
                            }
                        default:
                            break;
                    }

                    b.ID = id;
                    b.ParkingPlace = parkingPlace;
                    b.BoatSpacesRequired = boatSpacesRequired;
                    b.MaxSpeedNots = maxSpeedNots;
                    b.Weight = weight;
                    b.DaysRemaining = dayAllowedEntrance;
                    //b.DayAllowedEntrance = dayAllowedEntrance;

                    port.boats.Add(b);
                }

                MatchCollection mc = Regex.Matches(file, @"Day Rejected:\d*\s*BoatID:\S*"); //Day Rejected:2   BoatID:S-UBY ||
                for (int i = 0; i < mc.Count; i++)
                {
                    rejectedBoats.Add(mc[i].Value);
                }

                Print();
            }

            return fileLocated;
        }

        private void SaveStatusToFile()
        {
            // Save to disk.
            StreamWriter sw = File.CreateText("state.sav");

            sw.WriteLine($"<day>{Day}</day>");

            sw.WriteLine($"<docksNorth>{new String(port.docksNorth)}</docksNorth>");
            sw.WriteLine($"<docksSouth>{new String(port.docksSouth)}</docksSouth>");

            sw.WriteLine($"<boats>");
            foreach (Boat b in port.boats.OrderBy(b => b.ParkingPlace))
            {
                sw.WriteLine($"\t<ID>{b.ID}</ID>");
                sw.WriteLine($"\t<ParkingPort>{b.ParkingPort}</ParkingPort>");
                sw.WriteLine($"\t<ParkingPlace>{b.ParkingPlace}</ParkingPlace>");
                sw.WriteLine($"\t<BoatSpacesRequired>{b.BoatSpacesRequired}</BoatSpacesRequired>");
                sw.WriteLine($"\t<MaxSpeedNots>{b.MaxSpeedNots}</MaxSpeedNots>");
                sw.WriteLine($"\t<Weight>{b.Weight}</Weight>");
                sw.WriteLine($"\t<DaysRemaining>{b.DaysRemaining}</DaysRemaining>");
                //sw.WriteLine($"\t<DayAllowedEntrance>{b.DayAllowedEntrance}</DayAllowedEntrance>");
                sw.WriteLine($"\t<Type>{b.Type}</Type>");

                switch (b.Type)
                {
                    case TypeBoat.Rowboat:
                        {
                            sw.WriteLine($"\t<Passengers>{((RowBoat)b).Passengers}</Passengers>");
                            break;
                        }
                    case TypeBoat.Motorboat:
                        {
                            sw.WriteLine($"\t<HorsePower>{((MotorBoat)b).HorsePower}</HorsePower>");
                            break;
                        }
                    case TypeBoat.Sailsboat:
                        {
                            sw.WriteLine($"\t<BoatLenght>{((SailsBoat)b).BoatLenght}</BoatLenght>");
                            break;
                        }
                    case TypeBoat.Cargoboat:
                        {
                            sw.WriteLine($"\t<NumberContainers>{((CargoBoat)b).NumberContainers}</NumberContainers>");
                            break;
                        }
                    case TypeBoat.Katamaran:
                        {
                            sw.WriteLine($"\t<AmountBeds>{((Katamaran)b).AmountBeds}</AmountBeds>");
                            break;
                        }
                }
                sw.WriteLine();
            }
            sw.WriteLine($"</boats>");

            sw.WriteLine($"<rejectedBoats>");
            foreach (string s in rejectedBoats)
            {
                sw.WriteLine(s);
            }
            sw.WriteLine($"</rejectedBoats>");

            sw.Close();
        }

        private void ClearLists()
        {
            incomingBoats.Clear();
            incomingWin.Items.Clear();
            listNorthWin.Items.Clear();
            listSouthWin.Items.Clear();
        }

        /*
         * Before checking in a boat, we must check out those that are ready to go, 
         * and try to fill those spaces (like missing teeth) left.
         */
        private void CheckOutBoats()
        {
            var qBoats = port.boats
                    .Where(b => b.CheckForDeparture()) // This method returns true if the remaining days is equal to 0.
                    .OrderBy(b => b.ParkingPlace);     // Let's take them in order.

            foreach (Boat b in qBoats)
            {
                port.ReleaseBoat(b);
            }
        }

        private void CheckInBoats()
        {
            foreach (Boat b in incomingBoats)
            {
                bool accepted = port.CheckSpaceFor(b);

                if (accepted) port.boats.Add(b);
                else rejectedBoats.Add($"Day Rejected:{Day,-4}BoatID:{b.ID,-6}");
            }
        }

        private List<Boat> CreateBoats(int boats)
        {
            List<Boat> incoming = new List<Boat>();

            for (int i = 0; i < boats; i++)
            {
                int typeOfBoat = (new Random()).Next(0, 5);
                switch (typeOfBoat)
                {
                    case (int)TypeBoat.Rowboat:
                        {
                            RowBoat r = new RowBoat(true);
                            r.Type = TypeBoat.Rowboat;
                            incoming.Add(r);
                            break;
                        }
                    case (int)TypeBoat.Motorboat:
                        {
                            MotorBoat m = new MotorBoat(true);
                            m.Type = TypeBoat.Motorboat;
                            incoming.Add(m);
                            break;
                        }
                    case (int)TypeBoat.Sailsboat:
                        {
                            SailsBoat s = new SailsBoat(true);
                            s.Type = TypeBoat.Sailsboat;
                            incoming.Add(s);
                            break;
                        }
                    case (int)TypeBoat.Cargoboat:
                        {
                            CargoBoat c = new CargoBoat(true);
                            c.Type = TypeBoat.Cargoboat;
                            incoming.Add(c);
                            break;
                        }
                    case (int)TypeBoat.Katamaran:
                        {
                            Katamaran k = new Katamaran(true);
                            k.Type = TypeBoat.Katamaran;
                            incoming.Add(k);
                            break;
                        }
                    default:
                        break;
                }
            }

            return incoming;
        }

        private void ClickNext(object sender, RoutedEventArgs e)
        {
            Day++;
            
            nextButton.Content = "Go to day " + (Day + 1);

            ClearLists();

            // Every day arrives to port between 1 and 10 boats.
            incomingBoats = CreateBoats((new Random()).Next(1, 10));

            // We decrease by 1 the days available in every boat in the port.
            // When this number reaches 0. It will be time to leave.
            DecreaseDayInParkingBoats();

            // Are there any boat to check out?
            CheckOutBoats();

            // Now it is time to check in.
            CheckInBoats();

            // Print the information in the application.
            Print();
        }

        private void DecreaseDayInParkingBoats()
        {
            foreach (Boat b in port.boats)
            {
                b.DaysRemaining--;
            }
        }

        private void PrintResume()
        {
            resumeWin.Items.Clear();

            int amountRow = (port.boats.Where(b => b.Type == TypeBoat.Rowboat)).Count();
            int amountMotor = (port.boats.Where(b => b.Type == TypeBoat.Motorboat)).Count();
            int amountSails = (port.boats.Where(b => b.Type == TypeBoat.Sailsboat)).Count();
            int amountCargo = (port.boats.Where(b => b.Type == TypeBoat.Cargoboat)).Count();
            int amountKata = (port.boats.Where(b => b.Type == TypeBoat.Katamaran)).Count();
            decimal totalWeight = port.boats.Sum(b => b.Weight);
            double promedioVelocidad = port.boats.Average(b => b.MaxSpeedNots);
            int availablePlacesNorth = 0;
            int availablePlacesSouth = 0;
            foreach (char c in port.docksNorth)
            {
                if (c == 'A') availablePlacesNorth++;
            }
            foreach (char c in port.docksSouth)
            {
                if (c == 'A') availablePlacesSouth++;
            }
            int amountRejectedBoats = rejectedBoats.Count;

            resumeWin.Items.Add($"{"Amount Rowboats: ",-24}{amountRow,-5}");
            resumeWin.Items.Add($"{"Amount Motorboats: ",-24}{amountMotor,-5}");
            resumeWin.Items.Add($"{"Amount Sailsboats: ",-24}{amountSails,-5}");
            resumeWin.Items.Add($"{"Amount Cargoboats: ",-24}{amountCargo,-5}");
            resumeWin.Items.Add($"{"Amount Katamarans: ",-24}{amountKata,-5}");

            resumeWin.Items.Add($"{"Total weight of boats: ",-24}{totalWeight,-5:.00}");
            resumeWin.Items.Add($"{"Average velocity: ",-24}{promedioVelocidad,-5:.00}");
            resumeWin.Items.Add($"{"Availability North: ",-24}{availablePlacesNorth,-5}");
            resumeWin.Items.Add($"{"Availability South: ",-24}{availablePlacesSouth,-5}");
            resumeWin.Items.Add($"{"Rejected boats: ",-24}{amountRejectedBoats,-5}");
        }

        private void ClickSave(object sender, RoutedEventArgs e)
        {
            SaveStatusToFile();

            MessageBox.Show("Saved", "Save Status", MessageBoxButton.OK);
        }

        private void ClickClear(object sender, RoutedEventArgs e)
        {
            port = new Port();
            resumeWin.Items.Clear();
            rejectedBoats.Clear();
            ClearLists();
            ClearPort();
        }
    }
}
