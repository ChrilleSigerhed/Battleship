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
        public ICommand CheckButtonIndex { get; set; }
        public ICommand ShootCanon { get; set; }
        public Dictionary<string, bool> ButtonsAndShips { get; set; } = new Dictionary<string, bool>();
        public int Ships { get; set; } = 3;
        public ObservableCollection<SquaresInGrid> PlayerButtonsInGame { get; set; } = new ObservableCollection<SquaresInGrid>();
        public GameWindowViewModel()
        {
            FillListOfPlayerButtons();
            CheckButtonIndex = new RelayPropertyCommand(AddShipToList);
            ShootCanon = new RelayPropertyCommand(CheckHitOrMiss);
        }
        private void FillListOfPlayerButtons()
        {
            for (int i = 0; i < 24; i++)
            {
                SquaresInGrid square = new SquaresInGrid(i,"");
                PlayerButtonsInGame.Add(square);
            }
        }
        public void CheckHitOrMiss(string parameter)
        {
            int stringParameter = int.Parse(parameter);
            if (ButtonsAndShips.ContainsKey(parameter))
            {
                PlayerButtonsInGame[stringParameter].HitOrMiss = "Träff";
            }
            else
            {
                PlayerButtonsInGame[stringParameter].HitOrMiss = "Miss";
            }
        }
        public void AddShipToList(string button) // Placerar ut 3 skepp och skapar en lista där Knappen blir "Nyckeln" och skeppet en bool som visar True;
        {
            if(Ships == 0)
            {
                MessageBox.Show("Du har placerat alla ships");
            }
            else if (!ButtonsAndShips.ContainsKey(button))
            {
               ButtonsAndShips.Add(button, true);
               Ships--;
            }
            else
            {
              MessageBox.Show("Du har redan en båt på den platsen");
            }
        }
    }
}
