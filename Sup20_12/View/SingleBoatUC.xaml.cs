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
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            // If the DataObject contains string data, extract it.
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                string dataString = (string)e.Data.GetData(DataFormats.Bitmap);

                // If the string can be converted into a Brush,
                // convert it and apply it to the ellipse.
                ImageConverter converter = new ImageConverter();
                if (converter.IsValid(dataString))
                {
                    ImageBrush newFill = (ImageBrush)converter.ConvertFromString(dataString);
                    rectangleUI.Fill = newFill;

                    // Set Effects to notify the drag source what effect
                    // the drag-and-drop operation had.
                    // (Copy if CTRL is pressed; otherwise, move.)
                    if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    {
                        e.Effects = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                }
            }
            e.Handled = true;
        }
    }
}
