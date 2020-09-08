using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Sup20_12
{
    class MainWindowViewModel
    {
        public string Nickname { get; set; } = "Nickname"; // Ändrar labeln till Nickname från ViewModel - {Binding saknas}

        private Player player;// Testa skapa en person genom bindings i ChoosenPlayerPage

    }
}
