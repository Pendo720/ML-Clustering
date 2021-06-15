//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace Clustering
//{
//    /// <summary>
//    /// Interaction logic for VisualConfigurationDialog.xaml
//    /// </summary>
//    public partial class VisualConfigurationDialog : Window
//    {
//        public List<PlotFeatures> Fields { get; set; }

//        public VisualConfigurationDialog(List<PlotFeatures> fields)
//        {
//            InitializeComponent();
//            Fields = fields;
//        }


//        private void okbtn_Click(object sender, RoutedEventArgs e)
//        {
//            DialogResult = true;
//        }

//        private void cancelbtn_Click(object sender, RoutedEventArgs e)
//        {
//            DialogResult = false;
//        }

//        private void Window_Loaded(object sender, RoutedEventArgs e) => fieldlist.DataContext = Fields;

//        private void fieldlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            foreach( var x in e.AddedItems) {
//            //    PlotFeatures li = (x as PlotFeatures);
//            //    if(Fields.Contains(li))
//            //    {
//            //        Fields.ToArray()[Fields.IndexOf(li)].Status = li.Status;
//            //    }
//            //    else
//            //    {
//            //        Fields.Add(li);
//            //    }
//            }
//        }
//    }

//    //public class PlotFeatures
//    //{
//    //    public PlotFeatures(string tag)
//    //    {
//    //        Direction = tag;
//    //        Options = new List<string>();
//    //    }

//    //    public string Direction { get; set; }

//    //    public int SelectionIndex { get; set; }

//    //    public List<string> Options { set; get; }
//    //}
//}
