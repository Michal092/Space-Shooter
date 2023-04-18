using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Space_Shooter
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer boostTimer = new DispatcherTimer();
        DispatcherTimer enemyTimer = new DispatcherTimer();
        ImageBrush playerSprite = new ImageBrush();
        ImageBrush enemy1Sprite = new ImageBrush();
        ImageBrush enemy2Sprite = new ImageBrush();
        ImageBrush enemy3Sprite = new ImageBrush();
        ImageBrush enemy4Sprite = new ImageBrush();
        ImageBrush enemy5Sprite = new ImageBrush();
        List<Rectangle> itemstoremove = new List<Rectangle>();
        SoundPlayer shotSound = new SoundPlayer(@"pop.wav");
        SoundPlayer hitSound = new SoundPlayer(@"clickedpop.wav");
        Rect enemyHitBox, bulletHitBox;
        public bool shot = false;
        public bool isShot = false;
        public int live;
        public int mana;
        public int counter = 0;
        

        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(10);

            boostTimer.Tick += BoostEngine;
            boostTimer.Interval = TimeSpan.FromMilliseconds(25);

            enemyTimer.Tick += EnemyEngine;
            enemyTimer.Interval = TimeSpan.FromMilliseconds(1000);
            NewGame();
        }

        private void NewGame()
        {
            myCanvas.Focus();
            gameTimer.Start();
            boostTimer.Start();
            enemyTimer.Start();
            playerSprite.ImageSource = new BitmapImage(new Uri(@"player.png", UriKind.Relative));
            enemy1Sprite.ImageSource = new BitmapImage(new Uri(@"1.png", UriKind.Relative));
            enemy2Sprite.ImageSource = new BitmapImage(new Uri(@"2.png", UriKind.Relative));
            enemy3Sprite.ImageSource = new BitmapImage(new Uri(@"3.png", UriKind.Relative));
            enemy4Sprite.ImageSource = new BitmapImage(new Uri(@"4.png", UriKind.Relative));
            enemy5Sprite.ImageSource = new BitmapImage(new Uri(@"5.png", UriKind.Relative));
            
            live = 250;
            mana = 250;
            
         }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Input.Cursors.None;
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;
            Canvas.SetLeft(player, pX);
            Canvas.SetTop(player, pY);
        }

        private void EnemyEngine(object sender, EventArgs e)
        {
            Random generator = new Random();
            int c = generator.Next(10, 450);

            switch (counter) 
            {
                case 0:
                    Rectangle enemy1 = new Rectangle
                    {
                        Width = 40,
                        Height = 40,
                        Fill = enemy1Sprite,
                        Tag = "enemy",
                    };
                    Canvas.SetLeft(enemy1, c);
                    Canvas.SetTop(enemy1, 0);
                    myCanvas.Children.Add(enemy1);
                    counter++;
                    break;
                    case 1:
                    Rectangle enemy2 = new Rectangle
                    {
                        Width = 40,
                        Height = 40,
                        Fill = enemy2Sprite,
                        Tag = "enemy",
                    };
                    Canvas.SetLeft(enemy2, c);
                    Canvas.SetTop(enemy2, 0);
                    myCanvas.Children.Add(enemy2);
                    counter++;
                    break;
                    case 2:
                    Rectangle enemy3 = new Rectangle
                    {
                        Width = 40,
                        Height = 40,
                        Fill = enemy3Sprite,
                        Tag = "enemy",
                    };
                    Canvas.SetLeft(enemy3, c);
                    Canvas.SetTop(enemy3, 0);
                    myCanvas.Children.Add(enemy3);
                    counter++;
                    break;
                    case 3:
                    Rectangle enemy4 = new Rectangle
                    {
                        Width = 40,
                        Height = 40,
                        Fill = enemy4Sprite,
                        Tag = "enemy",
                    };
                    Canvas.SetLeft(enemy4, c);
                    Canvas.SetTop(enemy4, 0);
                    myCanvas.Children.Add(enemy4);
                    counter++;
                    break;
                    case 4:
                    Rectangle enemy5 = new Rectangle
                    {
                        Width = 40,
                        Height = 40,
                        Fill = enemy5Sprite,
                        Tag = "enemy",
                    };
                    Canvas.SetLeft(enemy5, c);
                    Canvas.SetTop(enemy5, 0);
                    myCanvas.Children.Add(enemy5);
                    counter = 0;
                    break;
            }   
        }
        private void BoostEngine(object sender, EventArgs e)
        {
            if (mana < 250)
            {
                mana += 5;
            }
            Mana.Width = mana;
          
        }

        private void GameEngine(object sender, EventArgs e)
        {
            player.Fill = playerSprite;
            if (shot && isShot)
            {
                Rectangle bullet1 = new Rectangle
                {
                    Width = 3,
                    Height = 8,
                    StrokeThickness = 3,
                    Fill = Brushes.AntiqueWhite,
                    Stroke = Brushes.Cyan,
                    Tag = "bullet",
                };
                Rectangle bullet2 = new Rectangle
                {
                    Width = 3,
                    Height = 8,
                    StrokeThickness = 3,
                    Fill = Brushes.AntiqueWhite,
                    Stroke = Brushes.Cyan,
                    Tag = "bullet",
                };

                Canvas.SetTop(bullet1, Canvas.GetTop(player) - 5);
                Canvas.SetTop(bullet2, Canvas.GetTop(player) - 5);
                Canvas.SetLeft(bullet1, Canvas.GetLeft(player) + 8);
                Canvas.SetLeft(bullet2, Canvas.GetLeft(player) + 35);
                shotSound.Play();
                myCanvas.Children.Add(bullet1);
                myCanvas.Children.Add(bullet2);
                if (mana > 60) { mana -= 50; }
               
                isShot = false;
            }

           
                   
           

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
               

                

                if ((string)x.Tag == "bullet")
                {
                    bulletHitBox = new Rect(Canvas.GetTop(x), Canvas.GetLeft(x),x.Width,x.Height);
                    Canvas.SetTop(x, Canvas.GetTop(x) - 5);
                }

               

                if ((string)x.Tag == "enemy")
                {
                    
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                    if (Canvas.GetTop(x) > 710)
                    {
                        live -= 5;
                        Live.Width = live;
                        itemstoremove.Add(x);
                    }
                }

              
                
                if (Canvas.GetTop(x) < 0)
                {
                    itemstoremove.Add(x);
                }
            }

            foreach (var z in myCanvas.Children.OfType<Rectangle>())
            {
                Rect playerHitBox = new Rect(Canvas.GetTop(player), Canvas.GetLeft(player), player.Width, player.Height);

                if ((string)z.Tag == "enemy")
                {
                    enemyHitBox = new Rect(Canvas.GetTop(z), Canvas.GetLeft(z), z.Width, z.Height);
                }

                if (enemyHitBox.IntersectsWith(bulletHitBox))
                {
                    itemstoremove.Add(z);
                }

                if (enemyHitBox.IntersectsWith(playerHitBox)) 
                {
                   
                }
            }
         

            foreach (Rectangle y in itemstoremove)
            {
                myCanvas.Children.Remove(y);
            }
        }



        private void LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            shot = true;
            isShot = true;
        }

        private void LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            shot = false;
            isShot = true;
        }
    }
}
