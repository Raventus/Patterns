using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMethod
{
    /// <summary>
    /// Шаблонный метод определяет основу алгоритма и позволяет подклассам переопределять некоторые шаги алгоритма, не изменяя его структуры в целом
    /// Каркас, в который наследники могут подставить реализацию недостающих элементов
    /// Более чётко определяет контракт между базовым и классом наследником
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GameObject gameMonopoly = new Monopoly();
            GameObject gameChess = new Chess();
            gameMonopoly.PlayOneGame(5);
            gameMonopoly.PlayOneGame(2);
        }
    }


    /// <summary>
    /// В классической реализации находится внутри другого класса
    /// </summary>
        public  abstract class GameObject
        {
            protected int PlayersCount;

            abstract protected bool EndOfGame();

            abstract protected void InitializeGame();

            abstract protected void MakePlay(int player);

            abstract protected void PrintWinner();

            // Шаблонны метод (не виртуальный !!!)
            public void PlayOneGame(int playersCount)
            {
                PlayersCount = playersCount;
                InitializeGame();

                var j = 0;

                while (!EndOfGame())
                {
                    MakePlay(j);
                    j = (j + 1) % playersCount;
                }

                PrintWinner();
            }
        }

        public class Monopoly : GameObject
        {
            // Конкретная реализации частей шаблонного метода

            protected override void InitializeGame()
            {
                // Initialize Monopoly
            }

            protected override void MakePlay(int player)
            {
                // Процесс игры
            }

            protected override bool EndOfGame()
            {
                return true;
            }

            protected override void PrintWinner()
            {
                // Отображение победителя
            }

     

        }

        public class Chess : GameObject
        {

            /* Implementation of necessary concrete methods */

            protected override void InitializeGame()
            {
                // Put the pieces on the board
            }

            protected override void MakePlay(int player)
            {
                // Process a turn for the player
            }

            protected override bool EndOfGame()
            {
                return true;
                // Return true if in Checkmate or Stalemate has been reached
            }

            protected override void PrintWinner()
            {
                /// Отображение победителя
            }

        }
    }
