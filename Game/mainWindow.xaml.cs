﻿using GameBI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
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

        public string pathtoscene = @"C:\Users\Марина\OneDrive\Документы\депрессия\2 год\3 семестр\Программирование\лаба 7\MinimalisticWarGame\Engine\bin\Debug\scene.yagir";
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
                    buttons[i,j] = button1;

                    myGrid.Children.Add(button1);
                }

            UpdateMap();


            Content = myGrid;
            gameScene.StartGame();
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
                    //for (int i = 0; i < scene.game.objects.Count; i++)
                    //{
                    //    gameScene.objects.Add(scene.game.objects[i]);
                    //}
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
            var btn = gameScene.OnCharacterPress(buttonPosition);
            UpdateMap();
            foreach (var b in btn)
            {
                buttons[((int)b.X), ((int)b.Y)].Background = Brushes.Yellow;
            }
        }

        public void UpdateMap()
        {
            for (int i = 0; i < gameScene.mapSize.Y; i++)
                for (int j = 0; j < gameScene.mapSize.X; j++)
                {
                    buttons[j, i].Background = Brushes.White;
                    buttons[j, i].Content = null;
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
    }
}
