using GameBI.Model;
using GameBI.Model.GameObjects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
using Object = GameBI.Model.Object;

namespace GameUI
{
    public partial class MainWindow : Window
    {
        public Game gameScene = new Game();

        public string pathtoscene = @"C:\Users\Программирование\лаба 7\MinimalisticWarGame\Engine\bin\Debug\scenegame.xml";
        public Button prevButton;

        public Button[,] buttons;
        public MainWindow()
        {
            InitializeComponent();
            LoadScene();


            Grid myGrid = new Grid();
            myGrid.Width = 450;
            myGrid.Height = 400;
            myGrid.HorizontalAlignment = HorizontalAlignment.Left;
            myGrid.VerticalAlignment = VerticalAlignment.Top;
            myGrid.ShowGridLines = true;

            buttons = new Button[((int)gameScene.mapSize.Y), ((int)gameScene.mapSize.X)];

            for (int i = 0; i < gameScene.mapSize.Y; i++)
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < gameScene.mapSize.X; i++)
                myGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < gameScene.mapSize.Y; i++)
                for (int j = 0; j < gameScene.mapSize.X; j++)
                {
                    var button1 = new Button
                    {
                        Background = Brushes.White,
                    };
                    Grid.SetColumn(button1, i);
                    Grid.SetRow(button1, j);
                    button1.Click += new RoutedEventHandler(ButtonClick);
                    buttons[i, j] = button1;

                    myGrid.Children.Add(button1);
                }

            if (gameScene.isGameEnd)
            {
                Deserializer();
            }
            var btn = gameScene.StartGame();

            UpdateMap();
            foreach (var b in btn)
            {
                buttons[((int)b.X), ((int)b.Y)].Background = Brushes.Yellow;
            }

            Main.Children.Add(myGrid);
            Serializer();
        }

        public void LoadScene()
        {
            //Проверяем существование файла
            if (File.Exists(pathtoscene))
            {
                gameScene.objects.Clear();
                //СОздаём новый экххемляр XmlSerializer с типом сцены
                XmlSerializer s = new XmlSerializer(typeof(Scene));
                //Создаём поток с нашим полным путём. Открывем для чтения.
                using (Stream reader = new FileStream(pathtoscene, FileMode.Open))
                {
                    //Присваиваем элементы reader которые мы преобразовали в scene - к всем объектам на сцене.
                    gameScene.objects.Clear();
                    Scene scene = (Scene)s.Deserialize(reader);
                    gameScene = scene.game;
                }
            }
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.Background = Brushes.White;
            Button pressedButton = sender as Button;
            prevButton = pressedButton;

            var buttonLocation = pressedButton.PointToScreen(new Point(0, 0));
            var buttonPosition = new Vector((int)((buttonLocation.X - Left) / pressedButton.ActualWidth),
                (int)((buttonLocation.Y - Top) / pressedButton.ActualHeight));

            var allch = new List<Character>(gameScene.charactersPlayerOne);
            allch.AddRange(gameScene.charactersPlayerTwo);
            if (pressedButton.Content != null && allch.Select(c => c.Position).Contains(buttonPosition))
            {
                labelName.Content = allch.Find(c => c.Position == buttonPosition).Name.ToString();
                labelHP.Content = allch.Find(c => c.Position == buttonPosition).HPModified.ToString();
                labelDamage.Content = allch.Find(c => c.Position == buttonPosition).DamageModified.ToString();
                labelDistanceMove.Content = allch.Find(c => c.Position == buttonPosition).distanceMove.ToString();
                labelDistanceAttack.Content = allch.Find(c => c.Position == buttonPosition).distanceAttack.ToString();
                var lists = gameScene.CharacterAbilities(buttonPosition);
                listBox.Items.Clear();
                foreach (var list in lists)
                {
                    listBox.Items.Add(list);
                }
            }
            else
            {
                labelName.Content = null;
                labelHP.Content = null;
                labelDamage.Content = null;
                labelDistanceMove.Content = null;
                labelDistanceAttack.Content = null;
                listBox.Items.Clear();
            }


            var btn = gameScene.OnCharacterPressMove(buttonPosition);
            UpdateMap();
            foreach (var b in btn)
            {
                buttons[((int)b.X), ((int)b.Y)].Background = Brushes.Yellow;
            }

            if (gameScene.isGameEnd)
                GameStartOver();

            Serializer();
        }

        public void UpdateMap()
        {
            for (int i = 0; i < gameScene.mapSize.Y; i++)
                for (int j = 0; j < gameScene.mapSize.X; j++)
                {
                    buttons[j, i].Background = Brushes.White;
                    buttons[j, i].Content = null;
                }

            var bars = gameScene.objects.Where(b => b.GetType() == typeof(GameBarrier)).ToList();
            foreach (var bar in bars)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(bar.Texture));

                buttons[((int)bar.Position.X), ((int)bar.Position.Y)].Content = img;
            }

            for (int i = 0; i < gameScene.charactersPlayerOne.Count; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(gameScene.charactersPlayerOne[i].Texture));

                buttons[(int)gameScene.charactersPlayerOne[i].Position.X,
                    (int)gameScene.charactersPlayerOne[i].Position.Y].Content = img;
            }

            for (int i = 0; i < gameScene.charactersPlayerTwo.Count; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(gameScene.charactersPlayerTwo[i].Texture));

                buttons[(int)gameScene.charactersPlayerTwo[i].Position.X,
                     (int)gameScene.charactersPlayerTwo[i].Position.Y].Content = img;
            }
        }
        public class Scene
        {
            public Game game;
            public Scene()
            {
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                var l = listBox.Items[listBox.SelectedIndex].ToString();

                var buttonLocation = prevButton.PointToScreen(new Point(0, 0));
                var buttonPosition = new Vector((int)((buttonLocation.X - Left) / prevButton.ActualWidth),
                    (int)((buttonLocation.Y - Top) / prevButton.ActualHeight));

                gameScene.ActiveAbilitiesDistans(l);
                var btn = gameScene.ActiveAbilitiesDistans(l);
                UpdateMap();
                foreach (var b in btn)
                {
                    buttons[((int)b.X), ((int)b.Y)].Background = Brushes.Yellow;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = gameScene.SkipTurn();
            UpdateMap();
            foreach (var b in btn)
            {
                buttons[((int)b.X), ((int)b.Y)].Background = Brushes.Yellow;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var btn = gameScene.CancelingSelectedCharacter();
            UpdateMap();
            foreach (var b in btn)
            {
                buttons[((int)b.X), ((int)b.Y)].Background = Brushes.Yellow;
            }
        }

        private void Serializer()
        {
            //СОздаём новый экххемляр XmlSerializer с типом сцены
            XmlSerializer s = new XmlSerializer(typeof(Scene), new Type[] { typeof(Scene) });
            //Создаём поток с нашим полным путём. 
            TextWriter myWriter = new StreamWriter(System.IO.Path.GetFullPath("Save.yagir")); ///Path.GetFullPath - Посзволяет начать путь с пути .exe файла
            Scene scene = new Scene(); // - Создаём новую сцену. 
            scene.game = gameScene; ///- Присваиваем этой сцене объекты на нашей сцене
            s.Serialize(myWriter, scene); /// Сериализуем и записываем.
            myWriter.Close(); //Закрываем поток.
        }

        private void Deserializer()
        {
            //Проверяем существование файла
            if (File.Exists("Save.yagir"))
            {
                gameScene.objects.Clear();
                //СОздаём новый экххемляр XmlSerializer с типом сцены
                XmlSerializer s = new XmlSerializer(typeof(Scene));

                //Создаём поток с нашим полным путём. Открывем для чтения.
                using (Stream reader = new FileStream("Save.yagir", FileMode.Open))
                {
                    //Пристваеваем элементы reader которые мы преобразовали в scene - к всем объектам на сцене.

                    gameScene.objects.Clear();
                    Scene scene = (Scene)s.Deserialize(reader);
                    gameScene = scene.game;
                }
            }            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GameStartOver();
        }

        private void GameStartOver()
        {
            LoadScene();
            UpdateMap();
            gameScene.isGameEnd = true;

            var btn = gameScene.StartGame();
            foreach (var b in btn)
            {
                buttons[((int)b.X), ((int)b.Y)].Background = Brushes.Yellow;
            }

            Serializer();
        }
    }
}
