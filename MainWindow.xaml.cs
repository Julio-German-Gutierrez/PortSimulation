using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            //InitDocksWin();

            // Check for a "status.sav" file.
            // If not found, we initialize everything at zero.
            if (!LoadStatusFromFile("status.sav"))
            {
                ClearPort();
            }
        }

        private void InitDocksWin()
        {
            for (int i = 0; i < port.docksNorth.Length; i++)
            {
                // "index = i + 32" because northGrid has 64 children (32 borders and 32 letters)
                Label label = (Label)northGrid.Children[i + 32];

                if (port.docksNorth[i] != 'A') label.Content = port.docksNorth[i]; //if not available -> print code
                else label.Content = "";                                           //otherwise let it be blank! (Available)
                
                ((Label)northGrid.Children[i + 32]).Foreground = Brushes.White;
                switch (port.docksNorth[i])
                {
                    case 'A':
                        {
                            ((Border)northGrid.Children[i]).Background = Brushes.Green;
                            break;
                        }
                    case 'R':
                        {
                            ((Border)northGrid.Children[i]).Background = Brushes.DarkOliveGreen;
                            break;
                        }
                    case 'D':
                        {
                            ((Border)northGrid.Children[i]).Background = Brushes.Olive;
                            break;
                        }
                    case 'M':
                        {
                            ((Border)northGrid.Children[i]).Background = Brushes.MediumVioletRed;
                            break;
                        }
                    case 'S':
                        {
                            ((Border)northGrid.Children[i]).Background = Brushes.DarkCyan;
                            break;
                        }
                    case 'C':
                        {
                            ((Border)northGrid.Children[i]).Background = Brushes.RoyalBlue;
                            break;
                        }
                    case 'K':
                        {
                            ((Border)northGrid.Children[i]).Background = Brushes.Goldenrod;
                            break;
                        }
                    default:
                        break;
                }

                // "index = i + 32" because northGrid has 64 children (32 borders and 32 letters)
                label = (Label)southGrid.Children[i + 32];

                if ( port.docksSouth[i] != 'A' ) label.Content = port.docksSouth[i];
                else label.Content = "";

                ((Label)southGrid.Children[i + 32]).Foreground = Brushes.White;
                switch (port.docksSouth[i])
                {
                    case 'A':
                        {
                            ((Border)southGrid.Children[i]).Background = Brushes.Green;
                            break;
                        }
                    case 'R':
                        {
                            ((Border)southGrid.Children[i]).Background = Brushes.DarkOliveGreen;
                            break;
                        }
                    case 'D':
                        {
                            ((Border)southGrid.Children[i]).Background = Brushes.Olive;
                            break;
                        }
                    case 'M':
                        {
                            ((Border)southGrid.Children[i]).Background = Brushes.MediumVioletRed;
                            break;
                        }
                    case 'S':
                        {
                            ((Border)southGrid.Children[i]).Background = Brushes.DarkCyan;
                            break;
                        }
                    case 'C':
                        {
                            ((Border)southGrid.Children[i]).Background = Brushes.RoyalBlue;
                            break;
                        }
                    case 'K':
                        {
                            ((Border)southGrid.Children[i]).Background = Brushes.Goldenrod;
                            break;
                        }
                    default:
                        break;
                }

            }
        }

        /*
         * ClearPort() basically sets everything to Zero.
         */
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
            // docksN and docksS are 2 strings that represent the state of every dock in the port.
            // This will be printed as a series of characters (one for every dock) next to the
            // Day information in the upper part of the main window.
            string docksN = "";
            string docksS = "";

            // For better visibility I will add a space between each character.
            // This: A A K K A D A M A ...
            // Reads better than:
            // This: AAKKADAMA...
            //
            // Remember that the docks are represented by char[] arrays.
            // "port.docksNorth" and "port.docksSouth" have the same length, that's
            // why I use one of them in the for loop as the limit.
            for (int i = 0; i < port.docksNorth.Length; i++)
            {
                docksN += port.docksNorth[i] + " "; // Just add a space between the Characters
                docksS += port.docksSouth[i] + " ";
            }

            // Now we print the list of docks with spaces in between for better visibility.
            dockListWin.Content = "North docks: " + docksN + "\n" +
                                  "South docks: " + docksS;

        }

        private void Print()
        {
            dayWin.Content = $"DAY: {Day}";

            // Like I said, this method simply prints the char[] with spaces in between each character.
            //DockListPrint();
            InitDocksWin();

            // Now we print the information about the North bay only.
            foreach (Boat b in port.boats
                .Where(b => b.ParkingPort == PortSide.North) // Only boats in he north bay
                .OrderBy(b => b.ParkingPlace))               // And we sort them by parking place.
            {
                string commonProperties = $"Dock: {b.ParkingPlace + 1,-3:00}" +
                                          $"{Enum.GetName(typeof(TypeBoat), b.Type),-12}" +
                                          $"ID: {b.ID,-8}" +
                                          $"Weight: {b.Weight,5} Kg   " +
                                          $"Max Speed: {b.MaxSpeedNots * 1.852,4:00.0} Km/h   ";

                // We now print the unique property based on the type of boat.
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
                            special = $"{"Num. Beds:",-12}{((Katamaran)b).AmountBeds,4}";
                            break;
                        }
                    default:
                        break;
                }

                // Time to print to screen.
                listNorthWin.Items.Add(commonProperties + special);
            }

            // Same procedure for the South bay.
            foreach (Boat b in port.boats
                .Where(b => b.ParkingPort == PortSide.South) // Now is the turn for the South bay.
                .OrderBy(b => b.ParkingPlace))               // Then again we order them by parking place.
            {
                string commonProperties = $"Dock: {b.ParkingPlace + 1,-3:00}" +
                                          $"{Enum.GetName(typeof(TypeBoat), b.Type),-12}" +
                                          $"ID: {b.ID,-8}" +
                                          $"Weight: {b.Weight,5} Kg   " +
                                          $"Max Speed: {b.MaxSpeedNots * 1.852,4:00.0} Km/h   ";

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

                listSouthWin.Items.Add(commonProperties + special);
            }

            // Now is the turn for the boats waiting to be accepted in port.
            // We print the list in the corresponding place in the application. The red quadrant.
            foreach (Boat b in incomingBoats)
            {
                string s = $"ID: {b.ID,-8}" +
                           $"Type: {Enum.GetName(typeof(TypeBoat), b.Type),-12}" +
                           $"{((!b.AcceptedInPort) ? "REJECTED" : "") }"; // If the boat was not accepted!!!
                incomingWin.Items.Add(s);
            }

            PrintResume();
        }

        /*
         * I decided to use regular expressions here and to make things a little bit easier,
         * I chose a LABEL/TAG format to save the original file.
         * That way I will only look for tags or labels and take their content.
         */
        private bool LoadStatusFromFile(string fileName)
        {
            bool fileLocated = false;

            if (File.Exists(fileName))
            {
                fileLocated = true;

                string file = File.ReadAllText(fileName);

                // We recover the day.
                Match m = Regex.Match(file, @"<day>(.*)</day>");
                Day = int.Parse(m.Groups[1].Value);

                // Now let's fill the dock's codes. North and South.
                m = Regex.Match(file, @"<docksNorth>(.*)</docksNorth>");
                port.docksNorth = m.Groups[1].Value.ToCharArray(); // and make them into char[]
                m = Regex.Match(file, @"<docksSouth>(.*)</docksSouth>");
                port.docksSouth = m.Groups[1].Value.ToCharArray();

                // Let's now recover every boat in the port. Multiple matches!
                MatchCollection mBoats = Regex.Matches(file, @"<boat>(.+?)</boat>", RegexOptions.Singleline);

                for (int i = 0; i < mBoats.Count; i++)
                {
                    string boat = mBoats[i].Groups[1].Value;

                    string id = Regex.Match(boat, @"<ID>(.*)</ID>").Groups[1].Value;
                    string parkingPort = Regex.Match(boat, @"<ParkingPort>(.*)</ParkingPort>").Groups[1].Value;
                    int parkingPlace = int.Parse(Regex.Match(boat, @"<ParkingPlace>(.*)</ParkingPlace>").Groups[1].Value);
                    string boatSpacesRequired = Regex.Match(boat, @"<BoatSpacesRequired>(.*)</BoatSpacesRequired>").Groups[1].Value;
                    int maxSpeedNots = int.Parse(Regex.Match(boat, @"<MaxSpeedNots>(.*)</MaxSpeedNots>").Groups[1].Value);
                    decimal weight = decimal.Parse(Regex.Match(boat, @"<Weight>(.*)</Weight>").Groups[1].Value);
                    int daysRemaining = int.Parse(Regex.Match(boat, @"<DaysRemaining>(.*)</DaysRemaining>").Groups[1].Value);
                    string type = Regex.Match(boat, @"<Type>(.*)</Type>").Groups[1].Value;
                    int special = int.Parse(Regex.Match(boat, @"</Type>\W*<.*>(.*)</.*>").Groups[1].Value);
                    
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
                        case "Katamaran":
                            {
                                b = new Katamaran();
                                b.Type = TypeBoat.Katamaran;
                                ((Katamaran)b).AmountBeds = special;
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
                    }

                    b.ID = id;
                    b.ParkingPlace = parkingPlace - 1;
                    b.BoatSpacesRequired = boatSpacesRequired;
                    b.MaxSpeedNots = maxSpeedNots;
                    b.Weight = weight;
                    b.DaysRemaining = daysRemaining;

                    port.boats.Add(b);
                }

                MatchCollection mc = Regex.Matches(file, @"<rejected>(.*)</rejected>");
                for (int i = 0; i < mc.Count; i++)
                {
                    rejectedBoats.Add(mc[i].Groups[1].Value);
                }

                Print();
            }

            return fileLocated;
        }

        private void SaveStatusToFile(string fileName)
        {
            // Save to disk.
            StreamWriter sw = File.CreateText(fileName);

            sw.WriteLine($"<day>{Day}</day>");
            sw.WriteLine();

            sw.WriteLine($"<docksNorth>{new string(port.docksNorth)}</docksNorth>");
            sw.WriteLine($"<docksSouth>{new string(port.docksSouth)}</docksSouth>");
            sw.WriteLine();

            sw.WriteLine($"<boats>");
            foreach (Boat b in port.boats.OrderBy(b => b.ParkingPlace))
            {
                sw.WriteLine($"\t<boat>");

                sw.WriteLine($"\t\t<ID>{b.ID}</ID>");
                sw.WriteLine($"\t\t<ParkingPort>{b.ParkingPort}</ParkingPort>");
                sw.WriteLine($"\t\t<ParkingPlace>{b.ParkingPlace + 1}</ParkingPlace>");
                sw.WriteLine($"\t\t<BoatSpacesRequired>{b.BoatSpacesRequired}</BoatSpacesRequired>");
                sw.WriteLine($"\t\t<MaxSpeedNots>{b.MaxSpeedNots}</MaxSpeedNots>");
                sw.WriteLine($"\t\t<Weight>{b.Weight}</Weight>");
                sw.WriteLine($"\t\t<DaysRemaining>{b.DaysRemaining}</DaysRemaining>");
                sw.WriteLine($"\t\t<Type>{b.Type}</Type>");

                switch (b.Type)
                {
                    case TypeBoat.Rowboat:
                        {
                            sw.WriteLine($"\t\t<Passengers>{((RowBoat)b).Passengers}</Passengers>");
                            break;
                        }
                    case TypeBoat.Motorboat:
                        {
                            sw.WriteLine($"\t\t<HorsePower>{((MotorBoat)b).HorsePower}</HorsePower>");
                            break;
                        }
                    case TypeBoat.Sailsboat:
                        {
                            sw.WriteLine($"\t\t<BoatLenght>{((SailsBoat)b).BoatLenght}</BoatLenght>");
                            break;
                        }
                    case TypeBoat.Cargoboat:
                        {
                            sw.WriteLine($"\t\t<NumberContainers>{((CargoBoat)b).NumberContainers}</NumberContainers>");
                            break;
                        }
                    case TypeBoat.Katamaran:
                        {
                            sw.WriteLine($"\t\t<AmountBeds>{((Katamaran)b).AmountBeds}</AmountBeds>");
                            break;
                        }
                }
                sw.WriteLine($"\t</boat>");
                sw.WriteLine();
            }
            sw.WriteLine($"</boats>");
            sw.WriteLine();

            sw.WriteLine($"<rejectedBoats>");
            foreach (string s in rejectedBoats)
            {
                sw.Write($"\t<rejected>{s}</rejected>");

                sw.WriteLine();
            }
            sw.WriteLine($"</rejectedBoats>");

            sw.Close();
        }

        private void ClearLists()
        {
            // Clear the list of incomingBoats and the red quadrant in the application.
            incomingBoats.Clear();
            incomingWin.Items.Clear();

            // These are the two green quadrants in the application.
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

            foreach (Boat b in qBoats) port.ReleaseBoat(b);
        }

        private void CheckInBoats()
        {
            foreach (Boat b in incomingBoats)
            {
                bool accepted = port.CheckSpaceFor(b); //CheckSpaceFor() returns TRUE if there is availability 
                                                                            //or FALSE otherwise.

                if (accepted) port.boats.Add(b); // Welcome to the port
                else rejectedBoats.Add($"Day Rejected:{Day,-4}BoatID:{b.ID}  "); // Sorry fellow!
            }
        }

        private List<Boat> CreateRandomBoats(int boats)
        {
            List<Boat> incoming = new List<Boat>(); // List to return.

            for (int i = 0; i < boats; i++)
            {
                // 0-RowBoat, 1-MotorBoat, 2-SailsBoat, 3-CargoBoat, 4-Katamaran
                TypeBoat t = (TypeBoat)Enum.GetValues(typeof(TypeBoat)) // This returns an array of the values in TypeBoat
                    .GetValue((new Random()).Next(0, 5));               // GetValue(i) retrieves by an index (much like [i]).
                                                                        // In this case "Random".

                switch (t)
                {
                    case TypeBoat.Rowboat:
                        {
                            RowBoat r = new RowBoat(true);
                            r.Type = TypeBoat.Rowboat;
                            incoming.Add(r);
                            break;
                        }
                    case TypeBoat.Motorboat:
                        {
                            MotorBoat m = new MotorBoat(true);
                            m.Type = TypeBoat.Motorboat;
                            incoming.Add(m);
                            break;
                        }
                    case TypeBoat.Sailsboat:
                        {
                            SailsBoat s = new SailsBoat(true);
                            s.Type = TypeBoat.Sailsboat;
                            incoming.Add(s);
                            break;
                        }
                    case TypeBoat.Cargoboat:
                        {
                            CargoBoat c = new CargoBoat(true);
                            c.Type = TypeBoat.Cargoboat;
                            incoming.Add(c);
                            break;
                        }
                    case TypeBoat.Katamaran:
                        {
                            Katamaran k = new Katamaran(true);
                            k.Type = TypeBoat.Katamaran;
                            incoming.Add(k);
                            break;
                        }
                }
            }

            return incoming;
        }

        private void ClickNext(object sender, RoutedEventArgs e)
        {
            InitDocksWin();

            Day++;

            nextButton.Content = "Go to day " + (Day + 1);

            ClearLists();

            // Every day arrives to port between 1 and 10 boats.
            incomingBoats = CreateRandomBoats((new Random()).Next(1, 10));

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

        // This method prints information in the resume section of the application.
        // The light blue quadrant.
        private void PrintResume()
        {
            // Let's forget about yesterday. Slate clean!
            resumeWin.Items.Clear();

            int amountRow = (port.boats.Where(b => b.Type == TypeBoat.Rowboat)).Count();
            int amountMotor = (port.boats.Where(b => b.Type == TypeBoat.Motorboat)).Count();
            int amountSails = (port.boats.Where(b => b.Type == TypeBoat.Sailsboat)).Count();
            int amountCargo = (port.boats.Where(b => b.Type == TypeBoat.Cargoboat)).Count();
            int amountKata = (port.boats.Where(b => b.Type == TypeBoat.Katamaran)).Count();

            decimal totalWeight = port.boats.Sum(b => b.Weight);
            
            double averageSpeed = port.boats.Average(b => b.MaxSpeedNots);
            
            int availablePlacesNorth = 0;
            int availablePlacesSouth = 0;
            foreach (char c in port.docksNorth) if (c == 'A') availablePlacesNorth++;
            foreach (char c in port.docksSouth) if (c == 'A') availablePlacesSouth++;

            int amountRejectedBoats = rejectedBoats.Count;

            resumeWin.Items.Add($"{"Availability North: ".ToUpper(),-24}{availablePlacesNorth,-5}");
            resumeWin.Items.Add($"{"Availability South: ".ToUpper(),-24}{availablePlacesSouth,-5}");

            resumeWin.Items.Add($"{"Amount Rowboats: ",-24}{amountRow,-5}");
            resumeWin.Items.Add($"{"Amount Motorboats: ",-24}{amountMotor,-5}");
            resumeWin.Items.Add($"{"Amount Sailsboats: ",-24}{amountSails,-5}");
            resumeWin.Items.Add($"{"Amount Cargoboats: ",-24}{amountCargo,-5}");
            resumeWin.Items.Add($"{"Amount Katamarans: ",-24}{amountKata,-5}");

            resumeWin.Items.Add($"{"Total weight of boats: ",-24}{totalWeight / 1000,-5:.00} Ton");
            resumeWin.Items.Add($"{"Average velocity: ",-24}{averageSpeed * 1.852,-5:00.00} Km/h");
            resumeWin.Items.Add($"{"Rejected boats: ",-24}{amountRejectedBoats,-5}");
        }

        private void ClickSave(object sender, RoutedEventArgs e)
        {
            SaveStatusToFile("status.sav");

            MessageBox.Show("Saved", "Save Status", MessageBoxButton.OK);
        }

        private void ClickClear(object sender, RoutedEventArgs e)
        {
            port = new Port();
            
            resumeWin.Items.Clear();
            
            rejectedBoats.Clear();

            ClearLists();
            ClearPort();
            InitDocksWin();
        }

        private void DoubleClickRejected(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView lista = (ListView)sender;
            if (lista.SelectedItem == lista.Items[9])
            {
                string rejected = "";
                foreach (string s in rejectedBoats)
                {
                    rejected += s + "\n";
                }
                MessageBox.Show(rejected);
            }
        }

        private void DoubleClickIncoming(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView lista = (ListView)sender;
            string searchID = (string)lista.SelectedItem;

            string ID = searchID.Substring(searchID.IndexOf("ID: ") + 4, 5);

            Boat b = incomingBoats.First(b => b.ID == ID);

            WinInfo wi = new WinInfo();
            wi.listInfo.Items.Add($"{"Type:",11} {b.Type}");
            wi.listInfo.Items.Add($"{"ID:", 11} {b.ID}");
            wi.listInfo.Items.Add($"{"Weight:",11} {b.Weight} Kg");
            if(b.AcceptedInPort) wi.listInfo.Items.Add($"{"Port:",11} {b.ParkingPort}");
            if(b.AcceptedInPort) wi.listInfo.Items.Add($"{"Place:",11} {b.ParkingPlace + 1}");
            wi.listInfo.Items.Add($"{"Max Speed:",11} {b.MaxSpeedNots * 1.852} Km/h");
            wi.listInfo.Items.Add("");
            if(!b.AcceptedInPort) wi.listInfo.Items.Add($"{"Response:",11} REJECTED");

            wi.Show();
        }

        private void ClickIncoming(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView lista = (ListView)sender;
            string searchID = (string)lista.SelectedItem;

            string ID = searchID.Substring(searchID.IndexOf("ID: ") + 4, 5);

            Boat b = incomingBoats.First(b => b.ID == ID);

            int index = 0;
            for (int i = 0; i < lista.Items.Count; i++)
            {
                if (((string)lista.Items[i]).Contains(ID)) index = i;
            }
            listNorthWin.SelectedItem = lista.Items[index];
        }
    }
}
