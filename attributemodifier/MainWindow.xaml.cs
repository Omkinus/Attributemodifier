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
                        modelpart.PartNumber.StartNumber = int.Parse(Partstartnumbertextbox.Text);
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
                        modelpart.AssemblyNumber.StartNumber = int.Parse(Assemblystartnumbertextbox.Text);
                    }


                    if (Boinametextbox.Text != "")
                    {
                        modelpart.SetUserProperty("USERDEFINED.BOUGHT_ITEM_NAME", Boinametextbox.Text);
                    }
                    if (Commenttextbox.Text != "")
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
                    if (boltcatalog_standard.SelectionBoxItemStringFormat != boltgroup.BoltStandard)
                    {
                        boltgroup.BoltStandard = boltcatalog_standard.Text;
                    }
                    
                    //boltsize
                   if (Convert.ToDouble(boltcatalogsize.Text) != boltgroup.BoltSize) 
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

        
    }
}
