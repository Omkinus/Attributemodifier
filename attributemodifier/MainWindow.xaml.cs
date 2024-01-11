using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tekla.Structures.Drawing;
using tsm = Tekla.Structures.Model;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.Model;
using static Tekla.Structures.Model.ReferenceModel;
using static Tekla.Structures.Drawing.StraightDimensionSet;
using Tekla.Structures.Model.UI;
using System.Collections;

namespace attributemodifier
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tsm.Model _model = new tsm.Model();
            DrawingHandler _drawinghandler = new DrawingHandler();
            Tekla.Structures.Drawing.ViewBase view = null;

            if (!_model.GetConnectionStatus() || !_drawinghandler.GetConnectionStatus())
            {
                MessageBox.Show("ВКЛЮЧИ ТЕКЛУ");
            }

            Tekla.Structures.Drawing.Drawing _currentdrawing = _drawinghandler.GetActiveDrawing();

            foreach (DrawingObject drawingObject in _drawinghandler.GetDrawingObjectSelector().GetSelected())
            {
                if (drawingObject is Tekla.Structures.Drawing.Part) {

 
                    Tekla.Structures.Drawing.Part part = drawingObject as Tekla.Structures.Drawing.Part;
                    Tekla.Structures.Identifier identifier = part.ModelIdentifier;
                    Tekla.Structures.Model.ModelObject ModelSideObject = _model.SelectModelObject(identifier);
                    Tekla.Structures.Model.Part modelpart = ModelSideObject as Tekla.Structures.Model.Part;
                    
                    if (Nametextbox.Text != "") {
                        modelpart.Name = Nametextbox.Text;
                    }

                    if (Profiletextbox.Text != "")
                    {
                        modelpart.Profile.ProfileString = Profiletextbox.Text;
                    }

                    if (Materialtextbox.Text != "")
                    {
                        modelpart.Material.MaterialString = Materialtextbox.Text;
                    }

                    if (Finishtextbox.Text != "")
                    {
                        modelpart.Finish = Finishtextbox.Text;
                    }
                    if (Classtextbox.Text != "")
                    {
                        modelpart.Class = Classtextbox.Text;
                    }



                    if (Partstartnumbertextbox.Text != "")
                    {
                        modelpart.PartNumber.StartNumber = Int32.Parse(Partstartnumbertextbox.Text);
                    }
                    if (Partprefixtextbox.Text != "")
                    {
                        modelpart.PartNumber.Prefix = Partprefixtextbox.Text;
                    }
                    if (Assemblyprefixtextbox.Text != "")
                    {
                        modelpart.AssemblyNumber.Prefix = Assemblyprefixtextbox.Text;
                    }
                    if (Assemblystartnumbertextbox.Text != "")
                    {
                        modelpart.AssemblyNumber.StartNumber = Int32.Parse(Assemblystartnumbertextbox.Text);
                    }


                    /*if (Boinametextbox.Text != "")
                    {
                        modelpart.Finish = Finishtextbox.Text;
                    }
                    if (Commenttextbox.Text != "")
                    {
                        modelpart.Finish = Finishtextbox.Text;
                    }*/



                    modelpart.Modify();
                   
                }
            }
          
          
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Partstartnumbertextbox.Text = "";
            Partprefixtextbox.Text = "";
            Assemblyprefixtextbox.Text = "";
            Assemblystartnumbertextbox.Text = "";

            Nametextbox.Text = "";
            Profiletextbox.Text = "";
            Finishtextbox.Text = "";
            Classtextbox.Text = "";
            Materialtextbox.Text = "";

            Boinametextbox.Text = "";
            Commenttextbox.Text = "";
            
        }
    }
}
