using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;
using static Engine.MainWindow;

namespace Engine
{
    public partial class MainWindow : Window
    {

        private static Action EmptyDelegate = delegate () { }; //Пустое действие

        public List<GameObject> objectonScene = new List<GameObject>();

        public string pathtoscene = @"C:\Users\Марина\OneDrive\Документы\депрессия\2 год\3 семестр\Программирование\лаба 7\MinimalisticWarGame\VideoEngine\SDK\WpfApp1\bin\Debug\scene.yagir";

        public Script script = new Script();

        public enum objectTypes { Тип1, Тип2, Кекс, Филин };
        //таймер для обновления
        public DispatcherTimer timer;
        //кол-во фпс (примерное)
        public long fps = 66;


        public MainWindow()
        {
            InitializeComponent();

            UserInterfaceCustomScale(1); //Устанавливаем DPI
            this.Dispatcher.BeginInvoke(DispatcherPriority.Render, EmptyDelegate); //Тип рендера

            Main.RenderTransformOrigin = new Point(0, 0);
            script.window = this; //В будущем  будет круче
            LoadScene(pathtoscene);
            //Создание экземпляра таймера
            timer = new DispatcherTimer();
            //Добавлем метод который будет выполняться при тике таймера
            timer.Tick += Update; ;
            //Интервал тика таймера
            timer.Interval = new TimeSpan(fps);
            //Стартуем таймер
            timer.Start();
        }
        //Покадровое обновление
        private void Update(object sender, EventArgs e)
        {
            //чекаем все объекты на сцене
            for (int i = 0; i < objectonScene.Count; i++)
            {
                //если объект инициализирован. 
                if (objectonScene[i].isInit == true)
                {
                    //Обновление
                    objectonScene[i].Update();
                }
            }
            script.Update();
        }

        public void LoadScene(string path)
        {
            //Проверяем существование файла
            if (File.Exists(path))
            {
                objectonScene.Clear();
                //СОздаём новый экххемляр XmlSerializer с типом сцены
                XmlSerializer s = new XmlSerializer(typeof(Scene));
                //Создаём поток с нашим полным путём. Открывем для чтения.
                using (Stream reader = new FileStream(pathtoscene, FileMode.Open))
                {
                    //Присваиваем элементы reader которые мы преобразовали в scene - к всем объектам на сцене.

                    objectonScene.Clear();
                    Scene scene = (Scene)s.Deserialize(reader);
                    for (int i = 0; i < scene.objects.Count; i++)
                    {
                        objectonScene.Add(new GameObject());
                        objectonScene[objectonScene.Count - 1].Init(scene.objects[i], this);
                    }
                }
            }
            script.Start();
        }
        private void UserInterfaceCustomScale(double customScale)
        {
            // Устанавливаем размер контента
            this.LayoutTransform = new ScaleTransform(customScale, customScale, 0, 0);
            Width *= customScale;
            Height *= customScale;

            // Устанавливаем окно в центр
            var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Top = (screenHeight - Height) / 2;
            Left = (screenWidth - Width) / 2;
        }


    }


}
