using System;
using System.Collections.Generic;
using System.Linq;
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
        public FieldInfo Current { get; set; }
        public List<FieldInfo> Fields { get; set; }

        public CrossValidatorDialog(List<string> fields)
        {
            InitializeComponent();
            Fields = fields.Select(s => new FieldInfo(s)).ToList();
            fieldlist.DataContext = Fields;
        }

        private void okbtn_Click(object sender, RoutedEventArgs e)
        {
            var temp = fieldlist.Template;
            var container = temp.LoadContent();
            foreach (var lbi in fieldlist.Items)
            {
                TextBox txtBox = (TextBox)temp.FindName("ValueField", fieldlist);
                if (txtBox != null)
                {
                    FieldInfo c = new FieldInfo("Capture");
                    c.Value = float.Parse(txtBox.Text);
                }
            }
;

            DialogResult = true;
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e){

            fieldlist.DataContext = Fields;
        }
       
    }    
}
