using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;

namespace Sup20_12.View
{
    /// <summary>
    /// Interaction logic for SubmarineUC.xaml
    /// </summary>
    public partial class SubmarineUC : UserControl
    {
        public int PlacedBoats { get; set; } = 1;

        public SubmarineUC()
        {
            InitializeComponent();
        }
        public SubmarineUC(System.Windows.Shapes.Rectangle rectangle)
        {
            InitializeComponent();
            this.rectangleSub = rectangle;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (PlacedBoats > 0)
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DataObject data = new DataObject();
                    data.SetData(DataFormats.StringFormat, rectangleSub.Fill.ToString());
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
            else
            {
                rectangleSub.Visibility = Visibility.Hidden;
            }
        }
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                StreamResourceInfo shipCurs = Application.GetResourceStream(new Uri("Assets/Cursor/submarineImgTest3.cur", UriKind.Relative));
                Mouse.SetCursor(new Cursor(shipCurs.Stream));
            }
            e.Handled = true;
        }
    }
}
