using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;

namespace Sup20_12.View
{
    /// <summary>
    /// Interaction logic for BattleShipUC.xaml
    /// </summary>
    public partial class BattleShipUC : UserControl
    {
        public BattleShipUC()
        {
            InitializeComponent();
        }
        public int PlacedBoats { get; set; } = 1;

        public BattleShipUC(System.Windows.Shapes.Rectangle rectangle)
        {
            InitializeComponent();
            this.rectangleBS = rectangle;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (PlacedBoats > 0)
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DataObject data = new DataObject();
                    data.SetData(DataFormats.StringFormat, rectangleBS.Fill.ToString());
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
            else
            {
                rectangleBS.Visibility = Visibility.Hidden;
            }
        }
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                StreamResourceInfo shipCurs = Application.GetResourceStream(new Uri("Assets/Cursor/battleshipImgTest2.cur", UriKind.Relative));
                Mouse.SetCursor(new Cursor(shipCurs.Stream));

            }
            e.Handled = true;
        }
    }
}
