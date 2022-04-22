using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        Rect playerHitBox;
        Rect groundHitBox;
        Rect obstacleHitBox;
        Rect barrelHitBox;

        bool jumping;

        int force = 20;
        int speed = 5;

        Random rnd = new Random();

        bool gameOver;
        double spriteIndex = 0;

        ImageBrush playerSprite = new ImageBrush();
        ImageBrush backgroundSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();
        ImageBrush barrelSprite = new ImageBrush();

        int[] obstaclePostion = { 320, 310, 300, 305, 315};
        int[] obstaclePostion2 = { 950, 955, 960, 1020};
        int[] barrelPostion2 = { 985, 1045 };

        int score = 0;

        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/background.gif"));


            background.Fill = backgroundSprite;
            background2.Fill = backgroundSprite;

            StartGame();

        }

        private void GameEngine(object sender, EventArgs e)
        {

            Canvas.SetLeft(background, Canvas.GetLeft(background) - 4);
            Canvas.SetLeft(background2, Canvas.GetLeft(background2) - 4);

            if (Canvas.GetLeft(background) < -1262)
            { 
                Canvas.SetLeft(background, Canvas.GetLeft(background2) + background2.Width);
            }


            if (Canvas.GetLeft(background2) < -1262)
            {
                Canvas.SetLeft(background2, Canvas.GetLeft(background) + background.Width);
            }

            Canvas.SetTop(player, Canvas.GetTop(player) + speed);
            Canvas.SetLeft(obstacle, Canvas.GetLeft(obstacle) - 12);
            Canvas.SetLeft(barrel, Canvas.GetLeft(barrel) - 12);

            scoretext.Content = "Score: " + score;

            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width -15, player.Height);
            obstacleHitBox = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width -2, obstacle.Height);
            barrelHitBox = new Rect(Canvas.GetLeft(barrel), Canvas.GetTop(barrel), barrel.Width, barrel.Height);
            groundHitBox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width, ground.Height);

            if (playerHitBox.IntersectsWith(groundHitBox))
            {
                speed = 0;
                Canvas.SetTop(player, Canvas.GetTop(ground) - player.Height);
                jumping = false;

                spriteIndex += .5;

                if (spriteIndex > 8)
                {

                    spriteIndex = 1;

                }

                RunSprite(spriteIndex);
            }


            if ( jumping == true)
            {
                speed = -9;

                force -= 1;
            }
            else
            {
                speed = 12;
            }


            if (force < 0)
            {
                jumping = false;
            }


            if (Canvas.GetLeft(obstacle) < -50)
            {
                Canvas.SetLeft(obstacle, obstaclePostion2[rnd.Next(0, obstaclePostion2.Length)]);

                Canvas.SetTop(obstacle, obstaclePostion[rnd.Next(0, obstaclePostion.Length)]);

                score += 1;
            }
            if (Canvas.GetLeft(barrel) < -50)
            {
                Canvas.SetLeft(barrel, barrelPostion2[rnd.Next(0, barrelPostion2.Length)]);

                Canvas.SetTop(barrel, 325);

                score += 1;
            }


            if(playerHitBox.IntersectsWith(obstacleHitBox) || playerHitBox.IntersectsWith(barrelHitBox))
            {
                gameOver = true;

                gameTimer.Stop();
            }
            
            if (gameOver == true)
            {
                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/runner_death.gif"));

                scoretext.Content = "Score: " + score + " Press enter to run again";
            }




        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && gameOver == true)
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && jumping == false && Canvas.GetTop(player) > 260)
            {
                jumping = true;
                force = 15;
                speed = -12;

                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_02.gif"));
            }
        }

        private void StartGame()
        {
            Canvas.SetLeft(background, 0);
            Canvas.SetLeft(background2, 1262);

            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 140);

            Canvas.SetLeft(obstacle, 950);
            Canvas.SetTop(obstacle, 310);

            Canvas.SetLeft(barrel, 1500);
            Canvas.SetTop(barrel, 325);

            RunSprite(1);

            obstacleSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/obstacle.png"));
            obstacle.Fill = obstacleSprite;

            barrelSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Radioactive_barrel.png"));
            barrel.Fill = barrelSprite;

            jumping = false;
            gameOver = false;
            score = 0;

            scoretext.Content = "Score: " + score;

            gameTimer.Start();


        }


        private void RunSprite(double i)
        {

            switch (i)
            {
                case 1:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_01.gif"));
                    break;
                case 2:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_02.gif"));
                    break;
                case 3:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_03.gif"));
                    break;
                case 4:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_04.gif"));
                    break;
                case 5:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_05.gif"));
                    break;
                case 6:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_06.gif"));
                    break;
                case 7:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_07.gif"));
                    break;
                case 8:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_08.gif"));
                    break;

            }

            player.Fill = playerSprite;


        }
    }
}
