using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace змейка_версия_1._0
{
    public partial class Form : System.Windows.Forms.Form
    {
        Point[] snake; //массив точек, из которых состоит змейка, Point стандартный класс
        Point[] snake_2;
        Point fruit;
        int snake_length;
        int snake2_length;
        int direction; //1 лево, 2 право, 3 верх, 4 низ
        int direction_2;
        public Form()
        {
            InitializeComponent();
            snake = new Point[200]; //макс.длина змейки 200 пикселей, один сегмент змейки 10 пикселей > макс. 20 ячеек
            snake_2 = new Point[200];
            snake_length = 5; //начальная длина (5 ячеек - 50 пикселей)
            snake2_length = 5;
            direction = 3;
            direction_2 = 3;
            for (int i = 0; i < 5; i++) //задаю начальное положение змейки
            {
                snake[i].X = 100; //поле 500 на 500 пикселей, одна ячейка 10 на 10, общее поле 50 на 50 ячеек
                snake[i].Y = 100 + i * 10; //сейчас положение вертикальное, х фиксированный, y меняется
                snake_2[i].X = 200; //поле 500 на 500 пикселей, одна ячейка 10 на 10, общее поле 50 на 50 ячеек
                snake_2[i].Y = 200 + i * 10;
            }
            fruit.X = 10; //стартовые координаты фрукта, дальше через рандом
            fruit.Y = 10;

        }
        
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            //реализация движения
            //нужно переместить каждую точку, начиная с конца, на один вперед, то есть координаты последнего пикселя теперь равны координатам бывшего предпоследнего...
            //...координаты второго теперь равным прошлым координатам первого, а координаты первого (нулевого по индексам) будут новыми
            for (int i = 198; i >= 0; i--)
            { 
                snake[i + 1].X = snake[i].X; //для 198: коор 199 = коор 198, последнему пикселю присваиваются коор предпоследнего
                snake[i + 1].Y = snake[i].Y;
 
            } //последняя интерация: коор 1 = коор 0, то есть нулевой пиксель не заполнен

            for (int i = 198; i >= 0; i--)
            {
                snake_2[i + 1].X = snake_2[i].X;
                snake_2[i + 1].Y = snake_2[i].Y;
            }
            if (direction == 1) //как определить координаты головы змейки? условно координаты головы отличаются от координат шеи в зависимости от направления 
            {
                snake[0].X = snake[1].X - 10; //смещение влево на 10 пикслей, то есть одну ячейку змеи
                snake[0].Y = snake[1].Y;
            } else if (direction == 2)
            {
                snake[0].X = snake[1].X + 10; 
                snake[0].Y = snake[1].Y;    
            }
            else if (direction == 3)
            {
                snake[0].X = snake[1].X; 
                snake[0].Y = snake[1].Y - 10; //!! оказывается ось у не как в математике, а перевернутая
            }
            else if (direction == 4 )
            {
                snake[0].X = snake[1].X; 
                snake[0].Y = snake[1].Y + 10 ;
            }
            else if (direction_2 == 1) //как определить координаты головы змейки? условно координаты головы отличаются от координат шеи в зависимости от направления 
            {
                snake_2[0].X = snake_2[1].X - 10; //смещение влево на 10 пикслей, то есть одну ячейку змеи
                snake_2[0].Y = snake_2[1].Y;
            }
            else if (direction_2 == 2)
            {
                snake_2[0].X = snake_2[1].X + 10;
                snake_2[0].Y = snake_2[1].Y;
            }
            else if (direction_2 == 3)
            {
                snake_2[0].X = snake_2[1].X;
                snake_2[0].Y = snake_2[1].Y - 10;
            }
            else if (direction_2 == 4)
            {
                snake_2[0].X = snake_2[1].X;
                snake_2[0].Y = snake_2[1].Y + 10;
            }
            //теперь нужно нарисовать саму змейку
            SolidBrush br1 = new SolidBrush(Color.Blue); //класс для заливки области одним цветом
            for (int i = 0; i < snake_length; i++) 
            { //PaintEventArgs это класс, у которого есть методы графики, позволяющие рисовать в форме 
                e.Graphics.FillEllipse(br1, snake[i].X, snake[i].Y, 10, 10); //отрисовала змейку 10*10 одна часть змейки, всего 5   
                
            }
            for (int i = 0; i < snake2_length; i++)
            { //PaintEventArgs это класс, у которого есть методы графики, позволяющие рисовать в форме 
                
                e.Graphics.FillEllipse(br1, snake_2[i].X, snake_2[i].Y, 10, 10);
            }

            //и нарисовать фрукт
            SolidBrush br2 = new SolidBrush(Color.Red); 
            e.Graphics.FillEllipse(br2, fruit.X, fruit.Y, 10, 10); //закрасила одну ячейку 10*10 пикселей   

            //процесс поедания - голова змеи достигает координат фрукта - фрукт исчезает - змея увеличивается
            if (snake[0].X == fruit.X && snake[0].Y == fruit.Y)
            {
                snake_length += 1;
                Random used_random;
                used_random= new Random();
                fruit.X = used_random.Next(0, 30) * 10; //next вернет случайное число до 50(т.к.поле 50*50 ячеек), *10 т.к.одна ячейка 10*10 пикселей
                fruit.Y = used_random.Next(0, 30) * 10;
                score.Text = Convert.ToString(Convert.ToInt32(score.Text) + 10);
                
            }

            //столкновение с бортиками
            //отрицательная коор головы - значит ушла влево/вверх, >50 - ушла вниз/вправо 
            //if (p[0].X < 0 || p[0].Y < 0 || p[0].X > 50 || p[0].Y > 50)
            //{
            //    len = 5;
                //    direction = 3;
                //    for (int i = 0; i < 5; i++) //возврат в стартовую позицию
                //    {
                //        p[i].X = 100; 
                //        p[i].Y = 100 + i * 10; 
                //    }
                }
            //}

        private void timer_Tick(object sender, EventArgs e)
        {
            panel.Invalidate(); //метод для обновления/перерисовки поля
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        { //метод для управления змейкой через стрелочки
            // KeyCode - свойство класса KeyEventArgs для получения кода нажатой клавиши
            if (e.KeyCode == Keys.A) //keys.left/right/up/down- задает коды клавиш срелок на клавиатуре
            {
                direction = 1;
            } else if (e.KeyCode == Keys.D) 
            {
                direction = 2;
            } else if (e.KeyCode == Keys.W)
            {
                direction = 3;
            } else if (e.KeyCode == Keys.S)
            {
                direction = 4;
            }
                if (e.KeyCode == Keys.Left) //keys.left/right/up/down- задает коды клавиш срелок на клавиатуре
                {
                    direction_2 = 1;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    direction_2 = 2;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    direction_2 = 3;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    direction_2 = 4;
                }
        }

        private void score_Click(object sender, EventArgs e)
        {
          
        }
    }
}
//ПРОБЛЕМА
//почему то поле отрисовывается не 50 на 50 ячеек, а меньше вроде как 37 на 40, хотя размер панели 500 на 500, а формы вообще больше
//поэтому фрукты появляются иногда за пределами
//плюс из-за этого непонятно как прописывать ограничение на столкновение с бортиками
