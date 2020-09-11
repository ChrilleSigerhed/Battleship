using Sup20_12.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sup20_12.ViewModels
{
   public class GameWindowViewModel : BaseViewModel
    {
        public ICommand PlaceShip { get; set; }
        public ICommand CheckIfShip { get; set; }
        public int Ships { get; set; } = 3;
        public ObservableCollection<GameGrid> PlayerButtonsInGame { get; set; }  = new ObservableCollection<GameGrid>();
        public ObservableCollection<GameGrid> ComputerButtonsInGame { get; set; } = new ObservableCollection<GameGrid>();
        public Dictionary<string, bool> PlayerShips { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> ComputerShips { get; set; } = new Dictionary<string, bool>();

        public GameEngine gameEngine;
        public GameWindowViewModel()
        {
            gameEngine = new GameEngine();
            CreatePlayerGrid();
            CreateComputerGrid();
            FillComputerShips();
            PlaceShip = new RelayPropertyCommand(FillListOfShips);
            CheckIfShip = new RelayPropertyCommand(CheckHitOrMiss);
        }
        public void CreatePlayerGrid()
        {
            for (int i = 0; i < 25; i++)
            {
                GameGrid square = new GameGrid(i, "");
                PlayerButtonsInGame.Add(square);
            }

        }
        public void CreateComputerGrid()
        {
            for (int i = 0; i < 25; i++)
            {
                GameGrid square = new GameGrid(i, "");
                ComputerButtonsInGame.Add(square);
            }

        }

        public void FillComputerShips()
        {
            Random random;
            int numberFromRandom;
            for (int i = 0; i < 3; i++)
            {
                random = new Random();
                numberFromRandom = random.Next(0, 24);
                while (ComputerShips.ContainsKey(numberFromRandom.ToString()))
                {
                    numberFromRandom = random.Next(0, 24);
                }
                ComputerShips.Add(numberFromRandom.ToString(), true);
            }

        }

        public void FillListOfShips(string button)
        {
            if(gameEngine.FillPlayerShips(button) == true)
            {
                Ships--;
            } else
            {
                MessageBox.Show("Du har placerat ut alla skepp. Spelet kan nu börja!");
            }
            
        }
        public void CheckHitOrMiss(string button)
        {

            if (ComputerShips.ContainsKey(button))
            {
                ComputerShips.Remove(button);
                int buttonToNumber = int.Parse(button);
                ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Träff";

            }
            else
            {
                int buttonToNumber = int.Parse(button);
                ComputerButtonsInGame[buttonToNumber].HitOrMiss = "Miss";
            }
            ComputerButtonsInGame[0].HitOrMiss = "hej";
        }

    }
}
