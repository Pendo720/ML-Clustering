using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Clustering
{
    public class FieldInfo
    {
        public string Label { get; set; }
        public float Value { get; set; }
        public FieldInfo(string sLabel)
        {
            Label = sLabel;
            Value = 0f;
        }
    }

    /// <summary>
    /// Interaction logic for CrossValidatorDialog.xaml
    /// </summary>
    public partial class CrossValidatorDialog : Window
    {
        
        public List<FieldInfo> Fields { get; set; }

        public CrossValidatorDialog(List<string> fields)
        {
            InitializeComponent();
            Fields = new List<FieldInfo>();
            fields.ForEach(s => Fields.Add(new FieldInfo(s)));
            fieldlist.DataContext = Fields;
        }


        private void okbtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => fieldlist.DataContext = Fields;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //fieldValue.Text = (float)(new Random().NextDouble());

        }
    }    
}
