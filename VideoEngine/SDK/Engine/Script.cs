using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Engine
{
    public class Script : YagirLogic
    {
        GameObject wall = null;
        float x, y;
        public void Start() //Только при переходе на сцену.
        {
            wall = FindObjectByName("wall");
            
        }


        public void Update() //Каждый тик таймера
        {
            if (!IsMouseOver(wall))
            {
                x += 0.1f;
                y += 0.1f;
            }
            if (!wall.Collision(wall, new Vector(-1f, 0))) SetPosition(new Vector(Math.Sin(x) * 50, Math.Cos(y) * 50), wall);

            if (Env.GetKey(System.Windows.Input.Key.Space))
            {
                Destroy(wall);
            }
        }
    }
}
