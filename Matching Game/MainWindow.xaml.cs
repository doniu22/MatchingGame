using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Matching_Game.Classes;
using System.IO;

namespace Matching_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer speech = new SpeechSynthesizer();
        public List<Image> cards = new List<Image>();
        public List<BitmapImage> graphics2cards = new List<BitmapImage>();
        private List<Player> playersList = new List<Player>();
        private Timer startGameTimer = new Timer();
        private Timer gameTimerCounter = new Timer();
        private int gameTimer = 0;
        private int timeToStart = 5;
        private int fbi_index, sbi_index = 0;
        private int counter = 0;
        private int pairCounter = 0;
        private int scoredPoints = 0;
        private bool startgame = false;

        private String[] goodWords = { "Dobrze!", "Wspaniale!", "Tak dalej!", "Cudownie!" };
        private String[] badWords = { "Zły wybór","Nie tym razem!", "Pudło!", "Spróbuj jeszcze raz!" };
        private int goodWordsCounter = -1;
        private int badWordsCounter = -1;
       
        
        
        

        public MainWindow()
        {
            
            startGameTimer.Interval = 1000;
            startGameTimer.Elapsed += new ElapsedEventHandler(this.startGameTimer_Tick);
            gameTimerCounter.Interval = 1000;
            gameTimerCounter.Elapsed += new ElapsedEventHandler(this.gameTimerCounter_Tick);
            InitializeComponent();
            InitializeBestScores();
            InitializeGrapics();
            CreateCards();
            speech.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
            speech.Rate = +2;

        }

        public async void InitializeBestScores()
        {

            using (StreamReader sr = new StreamReader(@"Data/scores.csv"))
            {

                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] dataString = line.Split(';');
                    Player player = new Player(dataString[0], Int32.Parse(dataString[1]));
                    playersList.Add(player);
                }

                for (int i = 0; i < playersList.Count; i++)
                    bestScoresList.Items.Add((i + 1) + " : " + playersList[i].NickName + "      ->      " + playersList[i].ScoresPoints);


                sr.Close();
            }

            
        
        }

        private void saveData()
        {
            StreamWriter sw = new StreamWriter(@"Data/scores.csv");

            string line;

            for (int i = 0; i < playersList.Count; i++)
            {
                line = playersList[i].NickName + ";" + playersList[i].ScoresPoints;
                sw.WriteLine(line);
            }

            sw.Flush();
            sw.Close();


        }

        private void ActualizeBestScoresList(Player newPlayer)
        {
            bool flaga = true;
            for (int i = 0; i < playersList.Count; i++)
                if (flaga)
                    if (playersList[i].ScoresPoints < newPlayer.ScoresPoints)
                    {
                        flaga = false;
                        playersList.Insert(i, newPlayer);
                    }

            bestScoresList.Items.Clear();

            for(int i=0;i<playersList.Count;i++)
                bestScoresList.Items.Add((i+1)+" : " + playersList[i].NickName + "  - >  " + playersList[i].ScoresPoints);

            saveData();


        }

        public void InitializeGrapics()
        {

            graphics2cards.Add(new BitmapImage(new Uri(@"Data/arbuz.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/arbuz.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/avocado.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/avocado.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/banan.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/banan.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/gruszka.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/gruszka.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/jabłko.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/jabłko.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/liczi.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/liczi.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/pomarańcza.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/pomarańcza.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/śliwka.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/śliwka.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/truskawka.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/truskawka.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/winogrono.png", UriKind.RelativeOrAbsolute)));
            graphics2cards.Add(new BitmapImage(new Uri(@"Data/winogrono.png", UriKind.RelativeOrAbsolute)));            

        }

        public void CreateCards()
        {
             for (int j = 0; j < 4; j++)
                for (int i = 0; i < 5; i++)
                {
                    Image obrazek = new Image();
                    obrazek.Height = 130;
                    obrazek.Width = 100;
                    obrazek.Source = new BitmapImage(new Uri(@"Data/cover.png", UriKind.RelativeOrAbsolute));
                    obrazek.Margin = new Thickness(20 + i * 115, 16 + j * 146, 0, 0);
                    cards.Add(obrazek);
                    CardPanel.Children.Add(obrazek);

                }

        }

        public void RandomizeImages()
        {
            List<BitmapImage> tempList = new List<BitmapImage>(graphics2cards);

            int iterator = 0;
            while(tempList.Count!=0)
            {
                Random rnd = new Random();
                int  pozycja = rnd.Next(0, tempList.Count);
                graphics2cards[iterator] = tempList[pozycja];
                tempList.RemoveAt(pozycja);
                iterator++;

            }

        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nickNameBox.Text != "")
            {
                
                gameTimerCounter.Stop();
                gameTimerCounter.Close();
                gameTimer = 0;
                timeToStart = 5;
                goodWordsCounter = -1;
                badWordsCounter = -1;
                scoredPoints = 0;
                pairCounter = 0;
                GameTimeLabel.Dispatcher.Invoke(() => { GameTimeLabel.Content = gameTimer.ToString() + "s"; });
                TimeToStartLabel.Dispatcher.Invoke(() => { TimeToStartLabel.Content = timeToStart.ToString() + "s"; });
                pointsLabel.Dispatcher.Invoke(() => { pointsLabel.Content = scoredPoints.ToString(); });

                RandomizeImages();

                startgame = false;
                
                for (int i = 0; i < cards.Count; i++)
                    cards[i].Source = graphics2cards[i];

                startGameTimer.Start();

            }
            else
            {
                if(nickNameBox.Text=="")
                   Task.Factory.StartNew(() => speech.Speak("Wprowadź nick!"));
                else
                   Task.Factory.StartNew(() => speech.Speak("Nick jest już zajęty!"));
            }


        }

        private void startGameTimer_Tick(object sender, EventArgs e)
        {
            if (timeToStart > 0)
            {
                timeToStart--;
                TimeToStartLabel.Dispatcher.Invoke(() => { TimeToStartLabel.Content = timeToStart.ToString() + "s"; });
            }
            else
            {
                startGameTimer.Stop();
                startGameTimer.Close();
               

                for (int i = 0; i < cards.Count; i++)
                    cards[i].Dispatcher.Invoke(() => { cards[i].Source = new BitmapImage(new Uri(@"Data/cover.png", UriKind.RelativeOrAbsolute)); });

                Task.Factory.StartNew(() => speech.Speak("Powodzenia!"));
                speech.Rate = +4;
                gameTimerCounter.Start();
                startgame = true;
                


            }
        }

        private void gameTimerCounter_Tick(object sender, EventArgs e)
        {
            gameTimer++;

            GameTimeLabel.Dispatcher.Invoke(() => { GameTimeLabel.Content = gameTimer.ToString() + "s"; });
           
        }

        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            double x = e.GetPosition(CardPanel).X;
            double y = e.GetPosition(CardPanel).Y;

            if (startgame)
            {
                for (int j = 0; j < 4; j++)
                    for (int i = 0; i < 5; i++)
                    {
                        if (x >= 20 + i * 115 && x <= 20 + i * 115 + 100 && y >= 16 + j * 146 && y <= 16 + j * 146 + 130)
                        {

                            if(cards[j * 5 + i].Source.ToString() == "pack://application:,,,/Data/cover.png")
                            {                         
                                cards[j * 5 + i].Source = null;
                                cards[j * 5 + i].Source = new BitmapImage(graphics2cards[j * 5 + i].UriSource);

                                counter++;

                                if (counter == 1)
                                {
                                    fbi_index = j * 5 + i;
                                }
                                else if (counter == 2)
                                {
                                    sbi_index = j * 5 + i;
                                    if (fbi_index == sbi_index)
                                        counter--;
                                    else
                                        counter = 0;

                                    if (counter == 0 && (!graphics2cards[sbi_index].UriSource.ToString().Equals(graphics2cards[fbi_index].UriSource.ToString())))
                                    {
                                        Task.Factory.StartNew(() => setBackground(fbi_index, sbi_index));
                                        scoredPoints -= 5;
                                        badWordsCounter++;
                                        pointsLabel.Dispatcher.Invoke(() => { pointsLabel.Content = scoredPoints.ToString(); });
                                        Task.Factory.StartNew(() => speech.Speak(badWords[badWordsCounter % 4]));
                                        

                                    }
                                    else
                                    {
                                        pairCounter++;
                                        scoredPoints += 20;
                                        goodWordsCounter++;
                                        pointsLabel.Dispatcher.Invoke(() => { pointsLabel.Content = scoredPoints.ToString(); });

                                        Task.Factory.StartNew(() => speech.Speak(goodWords[goodWordsCounter % 4]));
                                        
                                        if (pairCounter == 10)
                                        {
                                            scoredPoints += 120 - gameTimer;
                                            pointsLabel.Dispatcher.Invoke(() => { pointsLabel.Content = scoredPoints.ToString(); });
                                            startgame = false;
                                            gameTimerCounter.Stop();
                                            Player newPlayer = new Player(nickNameBox.Text, scoredPoints);
                                            ActualizeBestScoresList(newPlayer);
                                            speech.Rate = +2;
                                            Task.Factory.StartNew(() => speech.Speak("Dobra robota! Gra zakończona! Zgromadziłeś "+scoredPoints+" punktów. Twój czas to "+gameTimer+" sekund. Gratulacje!"));
                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
            }
        }

        private void setBackground(int firstindex, int secondindex)
        {
            for (int aa = 1; aa < 150000000; aa++) ;
            cards[firstindex].Dispatcher.Invoke(() => { cards[firstindex].Source = new BitmapImage(new Uri(@"Data/cover.png", UriKind.RelativeOrAbsolute)); });
            cards[secondindex].Dispatcher.Invoke(() => { cards[secondindex].Source = new BitmapImage(new Uri(@"Data/cover.png", UriKind.RelativeOrAbsolute)); });
            
        }

    }
}
