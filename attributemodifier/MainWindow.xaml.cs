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
using Tekla.Structures.Dialog.UIControls;
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
            

            if (!_model.GetConnectionStatus() || !_drawinghandler.GetConnectionStatus())
            {
                MessageBox.Show("ВКЛЮЧИ ТЕКЛУ");
            }

            Tekla.Structures.Drawing.Drawing _currentdrawing = _drawinghandler.GetActiveDrawing();
            
            foreach (DrawingObject drawingObject in _drawinghandler.GetDrawingObjectSelector().GetSelected())
            {
                if (drawingObject is Tekla.Structures.Drawing.Part) {
                   
                      // переключаемся с выбранного обьекта на чертеже на обьект в модели
                    Tekla.Structures.Drawing.Part part = drawingObject as Tekla.Structures.Drawing.Part;
                    Tekla.Structures.Identifier identifier = part.ModelIdentifier;
                    Tekla.Structures.Model.ModelObject ModelSideObject = _model.SelectModelObject(identifier);
                    Tekla.Structures.Model.Part modelpart = ModelSideObject as Tekla.Structures.Model.Part;
                    
                    if (checkbox_partname.IsChecked == true) {
                        modelpart.Name = Nametextbox.Text;
                    }
                    if (checkbox_partprofile.IsChecked == true)
                    {
                        modelpart.Profile.ProfileString = Profiletextbox.Text;
                    }
                    if (checkbox_material.IsChecked == true)
                    {
                        modelpart.Material.MaterialString = Materialtextbox.Text;
                    }
                    if (checkbox_finish.IsChecked == true)
                    {
                        modelpart.Finish = Finishtextbox.Text;
                    }
                    if (checkbox_class.IsChecked == true)
                    {
                        modelpart.Class = Classtextbox.Text;
                    }

                    if (checkbox_partstartnumber.IsChecked == true)
                    {
                        modelpart.PartNumber.StartNumber = int.Parse(Partstartnumbertextbox.Text);
                    }
                    if (checkbox_partprefix.IsChecked == true)
                    {
                        modelpart.PartNumber.Prefix = Partprefixtextbox.Text;
                    }
                    if (checkbox_assprefix.IsChecked == true)
                    {
                        modelpart.AssemblyNumber.Prefix = Assemblyprefixtextbox.Text;
                    }
                    if (checkbox_assstartnumber.IsChecked == true)
                    {
                        modelpart.AssemblyNumber.StartNumber = int.Parse(Assemblystartnumbertextbox.Text);
                    }


                    if (checkbox_boi.IsChecked == true)
                    {
                        modelpart.SetUserProperty("USERDEFINED.BOUGHT_ITEM_NAME", Boinametextbox.Text);
                    }
                    if (checkbox_comment.IsChecked == true)
                    {
                        modelpart.SetUserProperty("USERDEFINED.comment", Commenttextbox.Text);
                    }

                    modelpart.Modify();
                    
                 }

                if (drawingObject is Tekla.Structures.Drawing.Bolt) {

                    Tekla.Structures.Drawing.Bolt bolt = drawingObject as Tekla.Structures.Drawing.Bolt;
                    Tekla.Structures.Identifier identifier = bolt.ModelIdentifier;
                    Tekla.Structures.Model.ModelObject ModelSideObject = _model.SelectModelObject(identifier);
                    Tekla.Structures.Model.BoltGroup boltgroup = ModelSideObject as Tekla.Structures.Model.BoltGroup;

                    //bolt standard
                    if (checkbox_boltstandard.IsChecked == true)
                    {
                        boltgroup.BoltStandard = boltcatalog_standard.Text;
                    }
                    
                    //boltsize
                   if (checkbox_boltsize.IsChecked == true) 
                    {
                        boltgroup.BoltSize = Convert.ToDouble(boltcatalogsize.Text);
                    }
                    

                    boltgroup.Modify();

                }
                
            }
            //cпособ чтобы обновить виды
            ContainerView sheet = _currentdrawing.GetSheet();
            DrawingObjectEnumerator allviews = sheet.GetAllViews();
            DrawingObjectSelector dos = _drawinghandler.GetDrawingObjectSelector();
            dos.UnselectAllObjects();
            foreach (var view1 in allviews)
            {
                Tekla.Structures.Drawing.View view2 = view1 as Tekla.Structures.Drawing.View;
                view2.Select();
                double scale = view2.Attributes.Scale;
                double scale2 = view2.Attributes.Scale + 1;
                view2.Attributes.Scale = scale2;
                
                view2.Modify();
                view2.Attributes.Scale = scale;
                view2.Modify();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        //функция "почистить все поля"
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
