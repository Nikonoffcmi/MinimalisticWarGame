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
using System.Xml.Serialization;
using Path = System.IO.Path;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public List<Objects> objectonScene = new List<Objects>();

        public List<Variable> tmpVarList = new List<Variable>();

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Создание объекта
        /// </summary>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //Создание

            var beh = new List<string>();
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                beh.Add(listBox1.Items[i] as string);
            }

            objectonScene.Add(new Objects { name = NameBox.Text, texture = TextureBox.Text, type = TypeBox.SelectedIndex, pos = new Vector(int.Parse(X.Text), int.Parse(Y.Text)), size = new Vector(int.Parse(SX.Text), int.Parse(SY.Text)), behaviours = beh, variables = tmpVarList});
            //Обновление списка
            UpdateList();
        }

        /// <summary>
        /// Список объектов.
        /// </summary>
        void UpdateList()
        {
            listBox.Items.Clear();
            for (int i = 0; i < objectonScene.Count; i++)
            {
                listBox.Items.Add(objectonScene[i].name);
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                NameBox.Text = objectonScene[listBox.SelectedIndex].name;
                TextureBox.Text = objectonScene[listBox.SelectedIndex].texture;
                TypeBox.SelectedIndex = objectonScene[listBox.SelectedIndex].type;
                X.Text = objectonScene[listBox.SelectedIndex].pos.X.ToString();
                Y.Text = objectonScene[listBox.SelectedIndex].pos.Y.ToString();
                SX.Text = objectonScene[listBox.SelectedIndex].size.X.ToString();
                SY.Text = objectonScene[listBox.SelectedIndex].size.Y.ToString();
                listBox1.Items.Clear();
                for (int i = 0; i < objectonScene[listBox.SelectedIndex].behaviours.Count; i++)
                {
                    listBox1.Items.Add(objectonScene[listBox.SelectedIndex].behaviours[i]);
                }
                tmpVarList = objectonScene[listBox.SelectedIndex].variables;
                UpdateVarList();
            }
        }
        /// <summary>
        /// Сериализуем
        /// </summary>
        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            //СОздаём новый экххемляр XmlSerializer с типом сцены
            XmlSerializer s = new XmlSerializer(typeof(Scene));
            //Создаём поток с нашим полным путём. 
            TextWriter myWriter = new StreamWriter(Path.GetFullPath(textBox_Copy1.Text)); ///Path.GetFullPath - Посзволяет начать путь с пути .exe файла
            Scene scene = new Scene(); // - Создаём новую сцену. 
            scene.objects = objectonScene; ///- Присваиваем этой сцене объекты на нашей сцене
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
                objectonScene.Clear();
                //СОздаём новый экххемляр XmlSerializer с типом сцены
                XmlSerializer s = new XmlSerializer(typeof(Scene));

                //Создаём поток с нашим полным путём. Открывем для чтения.
                using (Stream reader = new FileStream(textBox_Copy1.Text, FileMode.Open))
                {
                    //Пристваеваем элементы reader которые мы преобразовали в scene - к всем объектам на сцене.

                    objectonScene.Clear();
                    Scene scene = (Scene)s.Deserialize(reader);
                    for (int i = 0; i < scene.objects.Count; i++)
                    {
                        objectonScene.Add(scene.objects[i]);
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
                objectonScene.RemoveAt(listBox.SelectedIndex);
                UpdateList();
            }
        }

        private void AddPoved_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Add(textBox.Text);
        }

        private void RemPoved_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            tmpVarList.Add((bool)checkBox.IsChecked ? new Variable(textBox1.Text, textBox1_Copy.Text) : 
                new Variable(textBox1.Text, int.Parse(textBox1_Copy.Text)));
            MessageBox.Show(tmpVarList[tmpVarList.Count - 1].intv.ToString());
            UpdateVarList();
        }

        void UpdateVarList()
        {
            listBox2.Items.Clear();

            for (int i = 0; i < tmpVarList.Count; i++)
            {
                listBox2.Items.Add("" + i + " : " + tmpVarList[i].name + " : " + tmpVarList[i].strv +"/"+ tmpVarList[i].intv.ToString());
            }
        }

       
    }


    public class Scene
    {
        public List<Objects> objects;
        public Scene()
        {

        }
    }
    public class Variable{
        public string strv;
        public int intv;
        public string name;
        public Variable()
        {

        }
        public Variable(string name, string strv)
        {
            this.strv = strv;
            this.name = name;
        }
        public Variable(string name, int intv)
        {
            this.intv = intv;
            this.name = name;
        }

    }
    public class Objects {

        public string name;
        public string texture;
        public Vector pos;
        public Vector size;
        public int type;
        public List<Variable> variables = new List<Variable>();
        public List<string> behaviours = new List<string>();
        public Objects()
        {

        }
    
    }

}
