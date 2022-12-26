using GameBI.Model;
using GameBI.Model.ActiveAbility;
using GameBI.Model.GameObjects;
using GameBI.Model.PassiveAbility;
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

            gameScene.activeAbilities.Add(new Heal("лечение", 1, 2));
            gameScene.activeAbilities.Add(new AttackIncrease("увеличение атаки", 3, 2));

            gameScene.passiveAbilities.Add(new PassiveHeal("пассивное лечение", 1, 1));
            gameScene.passiveAbilities.Add(new PassiveAttackIncrease("пассивное увеличение атаки", 3, 2));

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
                var Ab = new List<string>();
                for (int i = 0; i < listBox1.Items.Count; i++)
                    Ab.Add(listBox1.Items[i].ToString());

                var ch = new Character(NameBox.Text, int.Parse(HP.Text), int.Parse(Damage.Text),
                    int.Parse(DistanceMove.Text), int.Parse(DistanceAttack.Text), TextureBox.Text, 
                    new Vector(int.Parse(X.Text), int.Parse(Y.Text)), gameScene.activeAbilities.Where(aa => Ab.Contains(aa.Name)).ToList(),
                    gameScene.passiveAbilities.Where(pa => Ab.Contains(pa.Name)).ToList());
                gameScene.AddCharacterPlayerOne(ch);

                var ch2 = new Character(NameBox.Text, int.Parse(HP.Text), int.Parse(Damage.Text),
                    int.Parse(DistanceMove.Text), int.Parse(DistanceAttack.Text), TextureBox.Text,
                    new Vector(int.Parse(X.Text), int.Parse(Y.Text)), gameScene.activeAbilities.Where(aa => Ab.Contains(aa.Name)).ToList(),
                    gameScene.passiveAbilities.Where(pa => Ab.Contains(pa.Name)).ToList());

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
                if (gameScene.objects[listBox.SelectedIndex].GetType() == typeof(Character))
                {
                    listBox1.Items.Clear();
                    HP.Text = gameScene.charactersPlayerOne
                        .Find(c => c.Name.Equals(gameScene.objects[listBox.SelectedIndex].Name)).HP.ToString();
                    Damage.Text = gameScene.charactersPlayerOne
                        .Find(c => c.Name.Equals(gameScene.objects[listBox.SelectedIndex].Name)).damage.ToString();
                    DistanceAttack.Text = gameScene.charactersPlayerOne
                        .Find(c => c.Name.Equals(gameScene.objects[listBox.SelectedIndex].Name)).distanceAttack.ToString();
                    DistanceMove.Text = gameScene.charactersPlayerOne
                        .Find(c => c.Name.Equals(gameScene.objects[listBox.SelectedIndex].Name)).distanceMove.ToString();
                    var lists = gameScene.charactersPlayerOne
                        .Find(c => c.Name.Equals(gameScene.objects[listBox.SelectedIndex].Name)).passiveAbilities
                        .Select(pa => pa.Name).ToList();
                    lists.AddRange(gameScene.charactersPlayerOne
                        .Find(c => c.Name.Equals(gameScene.objects[listBox.SelectedIndex].Name)).ActiveAbilities
                        .Select(pa => pa.Name).ToList());
                    foreach (var list in lists)
                        listBox1.Items.Add(list);
                }
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
                    gameScene = scene.game;
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
                gameScene.charactersPlayerOne.Remove(gameScene.charactersPlayerOne
                    .Find(c => c.Name.Equals(listBox.Items[listBox.SelectedIndex])));
                gameScene.charactersPlayerTwo.Remove(gameScene.charactersPlayerTwo
                    .Find(c => c.Name.Equals(listBox.Items[listBox.SelectedIndex])));

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

        private void plusPassiveAb_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Add(listBoxPassiveAbility.Items[listBoxPassiveAbility.SelectedIndex].ToString());
        }

        private void plusActiveAb_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Add(listBoxActiveAbility.Items[listBoxActiveAbility.SelectedIndex].ToString());
        }

        private void minusAb_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
    }
}
