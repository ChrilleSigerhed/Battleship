using Sup20_12.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
        public SingleBoatUC(Rectangle rectangle)
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
                    // Package the data.
                    DataObject data = new DataObject();
                    data.SetData(DataFormats.StringFormat, rectangleUI.Fill.ToString());

                    // Inititate the drag-and-drop operation.
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
            if(PlacedBoats == 0)
            {
                rectangleUI.Visibility = Visibility.Hidden;
            }
           
        }
    }
}
