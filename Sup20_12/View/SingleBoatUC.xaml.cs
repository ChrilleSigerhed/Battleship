using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;

namespace Sup20_12.View
{
    /// <summary>
    /// Interaction logic for SingleBoatUC.xaml
    /// </summary>
    public partial class SingleBoatUC : UserControl
    {
        public int PlacedBoats { get; set; } = 1;

        public SingleBoatUC()
        {
            InitializeComponent();
        }
        public SingleBoatUC(System.Windows.Shapes.Rectangle rectangle)
        {
            InitializeComponent();
            this.rectangleUI = rectangle;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (PlacedBoats > 0)
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DataObject data = new DataObject();
                    data.SetData(DataFormats.StringFormat, rectangleUI.Fill.ToString());
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
            else
            {
                rectangleUI.Visibility = Visibility.Hidden;
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                StreamResourceInfo shipCurs = Application.GetResourceStream(new Uri("Assets/Cursor/destroyerImg.cur", UriKind.Relative));
                Mouse.SetCursor(new Cursor(shipCurs.Stream));
            }
            e.Handled = true;
        }
    }
}
