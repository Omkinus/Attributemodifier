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
using System.Globalization;

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
            this.MouseDown += delegate { DragMove(); };

            Tekla.Structures.Datatype.Distance.CurrentUnitType = Tekla.Structures.Datatype.Distance.UnitType.Inch;
            Tekla.Structures.Datatype.Distance.UseFractionalFormat = true;

            combobox_bolttype.Items.Add("Site");
            combobox_bolttype.Items.Add("Workshop");

            combobox_slotted.Items.Add("Oversized");
            combobox_slotted.Items.Add("Slotted");

            combobox_plainholetype.Items.Add("Blind");
            combobox_plainholetype.Items.Add("Through");
            
            profiles.InitializeComponent();
            

        }

        /*---------------------------------------КНОПКИ----------------------------------------------------*/
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
                if (drawingObject is Tekla.Structures.Drawing.Part)
                {

                    // переключаемся с выбранного обьекта на чертеже на обьект в модели
                    Tekla.Structures.Drawing.Part part = drawingObject as Tekla.Structures.Drawing.Part;
                    Tekla.Structures.Identifier identifier = part.ModelIdentifier;
                    Tekla.Structures.Model.ModelObject ModelSideObject = _model.SelectModelObject(identifier);
                    Tekla.Structures.Model.Part modelpart = ModelSideObject as Tekla.Structures.Model.Part;

                    if (checkbox_partname.IsChecked == true)
                    {
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

                if (drawingObject is Tekla.Structures.Drawing.Bolt)
                {
                    //находим болтовую группу в модели
                    Tekla.Structures.Drawing.Bolt bolt = drawingObject as Tekla.Structures.Drawing.Bolt;
                    Tekla.Structures.Identifier identifier = bolt.ModelIdentifier;
                    Tekla.Structures.Model.ModelObject ModelSideObject = _model.SelectModelObject(identifier);
                    Tekla.Structures.Model.BoltGroup boltgroup = ModelSideObject as Tekla.Structures.Model.BoltGroup;

                    //bolt_standard
                    if (checkbox_boltstandard.IsChecked == true)
                    {
                        boltgroup.BoltStandard = boltcatalog_standard.Text;
                    }
                    //boltsize
                    if (checkbox_boltsize.IsChecked == true)
                    {
                        boltgroup.BoltSize = Convert.ToDouble(boltcatalogsize.Text);
                    }
                    //bolt type
                    if (checkbox_bolttype.IsChecked == true) 
                    {
                        switch (combobox_bolttype.SelectedItem)
                        {
                            case "Site":
                                boltgroup.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_SITE;
                                break;
                            case "Workshop":
                                boltgroup.BoltType = BoltGroup.BoltTypeEnum.BOLT_TYPE_WORKSHOP;
                                break;
                        }
                    }
                    //bolt cutlength
                    if (checkbox_cutlength.IsChecked == true) 
                    {
                        Tekla.Structures.Datatype.Distance cutlengthdistance = 
                            Tekla.Structures.Datatype.Distance.FromFractionalFeetAndInchesString(Convert.ToString(boltcutlengthtextbox.Text));

                        double cutlengthdistancedouble = cutlengthdistance.ConvertTo(Tekla.Structures.Datatype.Distance.UnitType.Millimeter);
                        boltgroup.CutLength = cutlengthdistancedouble;
                    }
                    //bolt extralength
                    if (checkbox_extralength.IsChecked == true)
                    {
                        Tekla.Structures.Datatype.Distance extralengthdistance =
                        Tekla.Structures.Datatype.Distance.FromFractionalFeetAndInchesString(Convert.ToString(boltextralengthtextbox.Text));

                        double extralengthdistancedouble = extralengthdistance.ConvertTo(Tekla.Structures.Datatype.Distance.UnitType.Millimeter);
                        boltgroup.ExtraLength = extralengthdistancedouble;
                    }

                    //hole tolerance
                    if (checkbox_tolerance.IsChecked == true)
                    {
                        Tekla.Structures.Datatype.Distance tolerance =
                        Tekla.Structures.Datatype.Distance.FromFractionalFeetAndInchesString(Convert.ToString(bolttolerancetextbox.Text));

                        double tolerancedouble = tolerance.ConvertTo(Tekla.Structures.Datatype.Distance.UnitType.Millimeter);
                        boltgroup.Tolerance = tolerancedouble;
                    }
                    //bolt hole type
                    if (checkbox_slotted.IsChecked == true)
                    {
                        switch (combobox_slotted.SelectedItem)
                        {
                            case "Slotted":
                                boltgroup.HoleType = BoltGroup.BoltHoleTypeEnum.HOLE_TYPE_SLOTTED;
                                break;
                            case "Oversized":
                                boltgroup.HoleType = BoltGroup.BoltHoleTypeEnum.HOLE_TYPE_OVERSIZED;
                                break;
                        }
                    }
                    //slotted hole x
                    if (checkbox_slotholex.IsChecked == true)
                    {
                        Tekla.Structures.Datatype.Distance slotholex =
                        Tekla.Structures.Datatype.Distance.FromFractionalFeetAndInchesString(Convert.ToString(boltslothole_x_textbox.Text));

                        double slotholex_double = slotholex.ConvertTo(Tekla.Structures.Datatype.Distance.UnitType.Millimeter);
                        boltgroup.SlottedHoleX = slotholex_double;
                    }
                    //slotted hole y
                    if (checkbox_slotholey.IsChecked == true)
                    {
                        Tekla.Structures.Datatype.Distance slotholey =
                        Tekla.Structures.Datatype.Distance.FromFractionalFeetAndInchesString(Convert.ToString(boltslothole_y_textbox.Text));

                        double slotholey_double = slotholey.ConvertTo(Tekla.Structures.Datatype.Distance.UnitType.Millimeter);
                        boltgroup.SlottedHoleY = slotholey_double;
                    }
                    //studlength
                    if (checkbox_studlength.IsChecked == true && boltgroup.BoltStandard == "STUD")
                    {
                        Tekla.Structures.Datatype.Distance studlength =
                        Tekla.Structures.Datatype.Distance.FromFractionalFeetAndInchesString(Convert.ToString(studlength_textbox.Text));

                        double studlengthdouble = studlength.ConvertTo(Tekla.Structures.Datatype.Distance.UnitType.Millimeter);
                        boltgroup.Length = studlengthdouble;
                    }
                    //plain hole type
                    if (checkbox_plainholetype.IsChecked == true && boltgroup.BoltStandard != "STUD")
                    {
                        switch (combobox_plainholetype.SelectedItem)
                        {
                            case "Blind":
                                boltgroup.PlainHoleType = BoltGroup.BoltPlainHoleTypeEnum.HOLE_TYPE_BLIND;
                                break;
                            case "Through":
                                boltgroup.PlainHoleType = BoltGroup.BoltPlainHoleTypeEnum.HOLE_TYPE_THROUGH;
                                break;
                        }
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

        //мертвое,не трогать
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //Выключатель
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //включить все галки на болтах
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            checkbox_boltsize.IsChecked = true;
            checkbox_boltstandard.IsChecked = true;
            checkbox_bolttype.IsChecked = true;
            checkbox_plainholetype.IsChecked = true;
            checkbox_studlength.IsChecked = true;
            checkbox_cutlength.IsChecked = true;
            checkbox_extralength.IsChecked = true;
            checkbox_slotted.IsChecked = true;
            checkbox_slotholex.IsChecked = true;
            checkbox_slotholey.IsChecked = true;
            checkbox_tolerance.IsChecked = true;
            
        }

        //выключить все галки на болтах
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            checkbox_boltsize.IsChecked = false;
            checkbox_boltstandard.IsChecked = false;
            checkbox_bolttype.IsChecked = false;
            checkbox_plainholetype.IsChecked = false;
            checkbox_studlength.IsChecked = false;
            checkbox_cutlength.IsChecked = false;
            checkbox_extralength.IsChecked = false;
            checkbox_slotted.IsChecked = false;
            checkbox_slotholex.IsChecked = false;
            checkbox_slotholey.IsChecked = false;
            checkbox_tolerance.IsChecked = false;

        }

        //включить все галки на партах
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            checkbox_assprefix.IsChecked = true;
            checkbox_assstartnumber.IsChecked = true;
            checkbox_partprefix.IsChecked = true;
            checkbox_partstartnumber.IsChecked = true;
            checkbox_partname.IsChecked = true;
            checkbox_partprofile.IsChecked = true;
            checkbox_finish.IsChecked = true;
            checkbox_material.IsChecked = true;
            checkbox_class.IsChecked = true;
            checkbox_comment.IsChecked = true;
            checkbox_boi.IsChecked = true;
        }

        //выключить все галки на партах
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            checkbox_assprefix.IsChecked = false;
            checkbox_assstartnumber.IsChecked = false;
            checkbox_partprefix.IsChecked = false;
            checkbox_partstartnumber.IsChecked = false;
            checkbox_partname.IsChecked = false;
            checkbox_partprofile.IsChecked = false;
            checkbox_finish.IsChecked = false;
            checkbox_material.IsChecked = false;
            checkbox_class.IsChecked = false;
            checkbox_comment.IsChecked = false;
            checkbox_boi.IsChecked = false;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
