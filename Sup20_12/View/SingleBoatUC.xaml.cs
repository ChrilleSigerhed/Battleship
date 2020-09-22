using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Sup20_12.View
{
    /// <summary>
    /// Interaction logic for SingleBoatUC.xaml
    /// </summary>
    public partial class SingleBoatUC : UserControl
    {
        public int PlacedBoats { get; set; } = 3;
        
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
            if(PlacedBoats > 0)
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DataObject data = new DataObject();
                    data.SetData(DataFormats.StringFormat, rectangleUI.Fill.ToString());
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
            if(PlacedBoats == 2)
            {
                rectangleUI.Visibility = Visibility.Hidden;
            }
            if(PlacedBoats == 1)
            {
                rectangle2.Visibility = Visibility.Hidden;
            }
            if(PlacedBoats == 0)
            {
                rectangle3.Visibility = Visibility.Hidden;
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
           // base.OnGiveFeedback(e);
            
           // These Effects values are set in the drop target's
           // DragOver event handler.

           if (e.Effects.HasFlag(DragDropEffects.Move)) //&& e.OriginalSource is DESTROYER ---> byter skepp
           {
               StreamResourceInfo sriCurs = Application.GetResourceStream(new Uri("Assets/Cursor/destroyerImg.cur", UriKind.Relative));
               Mouse.SetCursor(new Cursor(sriCurs.Stream));
                
           }
           e.Handled = true;
           //else
           //{
           //    Mouse.SetCursor(Cursors.No);
           //}
        }
    }
}
