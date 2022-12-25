using GameBI.Model;
using GameBI.Model.ActiveAbility;
using GameBI.Model.GameObjects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
using Object = GameBI.Model.Object;

namespace Engine
{
    public partial class MainWindow : Window
    {
        public Game gameScene;

        public MainWindow()
        {
            InitializeComponent();
            gameScene = new Game();

            foreach (var AA in gameScene.activeAbilities.Select(a => a.Name))
                listBoxActiveAbility.Items.Add(AA);

            foreach (var PA in gameScene.passiveAbilities.Select(a => a.Name))
                listBoxPassiveAbility.Items.Add(PA);
        }

        /// <summary>
        /// Создание объекта
        /// </summary>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //Создание
            if (TypeBox.SelectedIndex == 0)
            {
                string AA = "";
                if (listBoxActiveAbility.SelectedIndex != -1)
                    AA = listBoxActiveAbility.SelectedItem.ToString();

                string PA = "";
                if (listBoxPassiveAbility.SelectedIndex != -1)
                    PA = listBoxPassiveAbility.SelectedItem.ToString();

                var ch = new Character(NameBox.Text, int.Parse(HP.Text), int.Parse(Damage.Text),
                    int.Parse(DistanceMove.Text), int.Parse(DistanceAttack.Text), TextureBox.Text, 
                    new Vector(int.Parse(X.Text), int.Parse(Y.Text)), gameScene.activeAbilities.Where(aa => aa.Name.Equals(AA)).ToList(),
                    gameScene.passiveAbilities.Where(pa => pa.Name.Equals(PA)).ToList());
                gameScene.AddCharacterPlayerOne(ch);

                var ch2 = new Character(NameBox.Text, int.Parse(HP.Text), int.Parse(Damage.Text),
                    int.Parse(DistanceMove.Text), int.Parse(DistanceAttack.Text), TextureBox.Text,
                    new Vector(int.Parse(X.Text), int.Parse(Y.Text)), gameScene.activeAbilities.Where(aa => aa.Name.Equals(AA)).ToList(),
                    gameScene.passiveAbilities.Where(pa => pa.Name.Equals(PA)).ToList());

                gameScene.AddCharacterPlayerTwo(ch2);
            }
            else if (TypeBox.SelectedIndex == 1)
                gameScene.AddGameBarrier(new GameBarrier(NameBox.Text, TextureBox.Text,
                    new Vector(int.Parse(X.Text), int.Parse(Y.Text))));
            
            //Обновление списка
            UpdateList();
        }

        /// <summary>
        /// Список объектов.
        /// </summary>
        void UpdateList()
        {
            listBox.Items.Clear();
            for (int i = 0; i < gameScene.objects.Count; i++)
            {
                listBox.Items.Add(gameScene.objects[i].Name);
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                NameBox.Text = gameScene.objects[listBox.SelectedIndex].Name;
                TextureBox.Text = gameScene.objects[listBox.SelectedIndex].Texture;
                X.Text = gameScene.objects[listBox.SelectedIndex].Position.X.ToString();
                Y.Text = gameScene.objects[listBox.SelectedIndex].Position.Y.ToString();
                listBox1.Items.Clear();;
            }
        }
        /// <summary>
        /// Сериализуем
        /// </summary>
        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            //СОздаём новый экххемляр XmlSerializer с типом сцены
            XmlSerializer s = new XmlSerializer(typeof(Scene), new Type[] { typeof(Scene) });
            //Создаём поток с нашим полным путём. 
            TextWriter myWriter = new StreamWriter(System.IO.Path.GetFullPath(textBox_Copy1.Text)); ///Path.GetFullPath - Посзволяет начать путь с пути .exe файла
            Scene scene = new Scene(); // - Создаём новую сцену. 
            scene.game = gameScene; ///- Присваиваем этой сцене объекты на нашей сцене
            s.Serialize(myWriter, scene); /// Сериализуем и записываем.
            myWriter.Close(); //Закрываем поток.
        }
        /// <summary>
        /// Десериализуем -
        /// </summary>
        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName == "")
            {
                return;
            }
            //Проверяем существование файла
            if (File.Exists(dialog.FileName))
            {
                gameScene.objects.Clear();
                //СОздаём новый экххемляр XmlSerializer с типом сцены
                XmlSerializer s = new XmlSerializer(typeof(Scene));

                //Создаём поток с нашим полным путём. Открывем для чтения.
                using (Stream reader = new FileStream(textBox_Copy1.Text, FileMode.Open))
                {
                    //Пристваеваем элементы reader которые мы преобразовали в scene - к всем объектам на сцене.

                    gameScene.objects.Clear();
                    Scene scene = (Scene)s.Deserialize(reader);
                    for (int i = 0; i < scene.game.objects.Count; i++)
                    {
                        gameScene.objects.Add(scene.game.objects[i]);
                    }
                }
            }
            //Обновление списка
            UpdateList();
        }
        //Удаление
        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                gameScene.objects.RemoveAt(listBox.SelectedIndex);
                UpdateList();
            }
        }

        [Serializable]
        public class Scene
        {
            public Game game;
            public Scene()
            {


            }
        }
    }
}
