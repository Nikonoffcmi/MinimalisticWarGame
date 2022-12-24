using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Engine.MainWindow;

namespace Engine
{

    /// <summary>
    /// Класс содержащий все главные процедуры и функции.
    /// </summary>
    public class YagirLogic
    {
        /// <summary>
        /// Главное окно
        /// </summary>
        public MainWindow window;

        /// <summary>
        /// Загрузка сцены по пути
        /// </summary>
        /// <param name="path">Путь к сцене</param>
        public void LoadScene(string path)
        {
            window.LoadScene(path);
        }
        /// <summary>
        /// Поиск объекта на сцене по имени.
        /// </summary>
        /// <param name="name">Имя объекта</param>
        /// <returns></returns>
        public GameObject FindObjectByName(string name)
        {
            return window.objectonScene.Find(x => x.name.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Поиск всех объектов на сцене по имени
        /// </summary>
        /// <param name="name">Имя объекта</param>
        /// <returns></returns>
        public List<GameObject> FindAllObjectByName(string name)
        {
            return window.objectonScene.FindAll(x => x.name.ToLower() == name.ToLower());
        }
        /// <summary>
        /// Перемещает обхект в заданном направлении
        /// </summary>
        /// <param name="object">Обхект</param>
        /// <param name="dir">Направление</param>
        public void TranslateObject(GameObject @object, Vector dir)
        {
            @object.pos.X += dir.X;
            @object.pos.Y += dir.Y;
        }
        /// <summary>
        /// Уничтожает объект
        /// </summary>
        /// <param name="object">Объект</param>
        public void Destroy(GameObject @object)
        {
            if (@object != null)
            {
                if (@object.sprite != null)
                {
                    window.Main.Children.Remove(@object.sprite);
                }
                window.objectonScene.Remove(@object);
            }
        }

        /// <summary>
        /// задаёт позицию
        /// </summary>
        /// <param name="pos">Позиция</param>
        /// <param name="object">Объект</param>
        public void SetPosition(Vector pos, GameObject @object)
        {
            @object.pos = pos;
        }
        /// <summary>
        /// Найти объект с конкретным поведединем
        /// </summary>
        /// <param name="Beh">Поведение</param>
        /// <returns>GameObject</returns>
        public GameObject FindByBeh(string Beh)
        {
            return window.objectonScene.Find(x => x.behaviours.Contains(Beh));
        }
         /// <summary>
         /// Найти все объекты с нужным поведением
         /// </summary>
         /// <param name="Beh">Поведение</param>
         /// <returns></returns>
        public List<GameObject> FindAllByBeh(string Beh)
        {
            return window.objectonScene.FindAll(x => x.behaviours.Contains(Beh));
        }

        /// <summary>
        /// Найти объект с конкретным типом
        /// </summary>
        /// <param name="types">Тип</param>
        /// <returns>GameObject</returns>
        public GameObject FindByType(objectTypes types)
        {
            return window.objectonScene.Find(x => x.type == types);
        }

         /// <summary>
         /// Найти все объекты с нужным типом
         /// </summary>
         /// <param name="types"></param>
         /// <returns></returns>
        public List<GameObject> FindAllByTypes(objectTypes types)
        {
            return window.objectonScene.FindAll(x => x.type == types);
        }


        public bool IsMouseOver(GameObject @object)
        {
            if (@object.sprite != null)
            {
                return @object.sprite.IsMouseOver;
            }
            return false;
        }
    }


    /// <summary>
    /// Класс сцены
    /// </summary>
    public class Scene
    {
        public List<Objects> objects;
        public Scene()
        {

        }
    }
    /// <summary>
    /// Класс игрового объекта
    /// </summary>
    public class GameObject
    {
        public Rectangle sprite;
        public string name;
        public string texture;
        public Vector pos;
        public Vector size;
        public MainWindow.objectTypes type;
        public bool isInit = false;
        public float rot = 0;
        public MainWindow window;
        public List<string> behaviours;
        public List<Variable> variables;
        public EventHandler onCollide;
        public double Angle(Point origin, Point target) //Расчитываем угол по пифагору
        {
            Vector vecTo = target - origin;
            vecTo.Normalize();
            double dotAngle = -vecTo.Y;
            double angle = Math.Acos(dotAngle);
            angle = angle * 180 / Math.PI;
            if (vecTo.X > 0)
                return angle;
            else
                return -angle;
        }
        public void Init(Objects @object, MainWindow window) //@object - объект который мы получаем из файла, window наше окно 
        {
            //присваиваем переданные переменные в переменные объекта
            name = @object.name;
            this.window = window;
            texture = @object.texture;
            pos = @object.pos;
            size = @object.size;
            type = (objectTypes)@object.type;
            behaviours = @object.behaviours;
            variables = @object.variables;
            ///
            ///Создаём ПРЯМОУГОЛЬНИК 
            Rectangle rect = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = size.Y,
                Width = size.X,
                Margin = new Thickness(pos.X, pos.Y, 0, 0),
                Fill = new ImageBrush(new BitmapImage(new Uri(texture)))
            }; //ImageBrush позволяет поместить картинку как фон
            sprite = rect; //Записывем на rect

            window.Main.Children.Add(sprite); //Добавляем Sprite на сцену
            //конец инициализации
            //onCollide += (a, e) => { MessageBox.Show("Саске лучший!!!"); };
            isInit = true;
        }
        public void Update()
        {
            if (sprite == null) return;

            sprite.Margin = new Thickness(pos.X, pos.Y, 0, 0); //Меняем растояние от границ окна. 
            if (behaviours.Contains("Look"))
            {
                RotateTransform rotateTransform = new RotateTransform(Angle(new Point(sprite.Margin.Left + (sprite.Width / 2), //Поворот спрайта
                     sprite.Margin.Top + (sprite.Height / 2)), Mouse.GetPosition(window.Main)) - 90, sprite.Width / 2, sprite.Height / 2);
                rot = (float)Angle(new Point(sprite.Margin.Left + (sprite.Width / 2), sprite.Margin.Top + (sprite.Height / 2)), //Записываем грудус поворота 
                    Mouse.GetPosition(window.Main)) - 90;
                sprite.RenderTransform = rotateTransform; //Прменяем поворот
            }

            if (behaviours.Contains("Move"))
            {
                var tmpspeed = variables.Find(x => x.name == "Speed");
                float speed = tmpspeed != null ? tmpspeed.intv : 1;   //Скорость
                if (Env.GetKey(Key.W))//Если нажата W то:
                {
                    if (!Collision(this, new Vector(0, speed)))
                    {
                        pos += new Vector(0, -speed);//Меняем растояние от границ окна. 
                    }
                }
                if (Env.GetKey(Key.S))
                {
                    if (!Collision(this, new Vector(0, -speed)))
                    {
                        pos += new Vector(0, speed);
                    }
                }
                if (Env.GetKey(Key.A))
                {
                    if (!Collision(this, new Vector(speed, 0)))
                    {
                        pos += new Vector(-speed, 0);
                    }
                }
                if (Env.GetKey(Key.D))
                {
                    if (!Collision(this, new Vector(-speed, 0))) // Если - то вектор + и иначе
                    {
                        pos += new Vector(speed, 0);
                    }
                }
            }


            if (behaviours.Contains("Trigger"))
            {
            }
        }


        public bool Collision(GameObject instantiate, Vector move) //Функция проверяющая пересечания. Возращает да или нет
        {
            bool collide = false; //Взаимодействет ли?
            for (int i = 0; i < window.objectonScene.Count; i++) // Проверяем каждый obj на сцене
            {
                if (window.objectonScene[i].sprite != instantiate.sprite) //Если у него есть спрайт
                {
                    if (instantiate.behaviours.Contains("Solid") || instantiate.behaviours.Contains("Move") || instantiate.behaviours.Contains("Trigger")) //Если это игрок или Solid
                    {
                        var spriteC = window.objectonScene[i].sprite; //Проверяем объект пересечения на спрайт
                        if (spriteC != null) //Если он есть
                            collide = new Rect(sprite.Margin.Left, sprite.Margin.Top, sprite.Width, sprite.Height).IntersectsWith(new Rect(spriteC.Margin.Left + move.X, spriteC.Margin.Top + move.Y, spriteC.Width, spriteC.Height));
                        //Используем метод IntersectsWith который проверит 2 объекта и скажет нам Да или Нет

                        if (collide) // Если да то ЫДА иначе НЕТ
                        {
                            instantiate.onCollide?.Invoke(this, null); //Вызываем эвент для касания
                            if (window.objectonScene[i].behaviours.Contains("Trigger"))
                            {
                                window.objectonScene[i].onCollide?.Invoke(this, null);
                                return false;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
    /// <summary>
    /// Класс переменной (Ключа)
    /// </summary>
    public class Variable
    {
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
    /// <summary>
    /// Класс прототипа объекта.
    /// </summary>
    public class Objects
    {

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
