using GameBI.Model;
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


            for (int i = 0; i < gameScene.mapSize.Y; i++)
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < gameScene.mapSize.X; i++)
                myGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < gameScene.objects.Count; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(gameScene.objects[i].texture));

                var button = new Button
                {
                    Content = img,
                };
                Grid.SetColumn(button, (int)gameScene.objects[i].pos.X-1);
                Grid.SetRow(button, (int)gameScene.objects[i].pos.Y-1);

                myGrid.Children.Add(button);
            }

            Content = myGrid;
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
                    for (int i = 0; i < scene.game.objects.Count; i++)
                    {
                        gameScene.objects.Add(scene.game.objects[i]);
                    }
                }
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
