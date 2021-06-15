using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Clustering
{
    public class FieldInfo
    {
        public string Label { get; set; }
        public float Value { get; set; }

        public int Index { get; set; }
        public FieldInfo(string sLabel, int index)
        {
            Label = sLabel;
            Value = 0f;
            Index = index;
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
            Fields = fields.Select((s, i) => (s, i)).ToList().Select(p => new FieldInfo(p.s, p.i)).ToList();
            fieldlist.DataContext = Fields;
        }

        private void okbtn_Click(object sender, RoutedEventArgs e)
        {
            Enumerable.Range(0, fieldlist.Items.Count).ToList().ForEach(i =>
            {
                ListBoxItem myListBoxItem = (ListBoxItem)fieldlist.ItemContainerGenerator.ContainerFromItem(fieldlist.Items.CurrentItem);

                // Getting the ContentPresenter of myListBoxItem
                ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);

                // Finding textBlock from the DataTemplate that is set on that ContentPresenter
                DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                TextBox txtBox = (TextBox)myDataTemplate.FindName("ValueField", myContentPresenter);
                string valueTxt = txtBox.Text;
                if(valueTxt != string.Empty) { 
                    Fields.ElementAt(i).Value = float.Parse(valueTxt);
                }
                fieldlist.Items.MoveCurrentToNext();
            });

            DialogResult = true;
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            fieldlist.DataContext = Fields;

        }

        private void ValueField_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);
            e.Handled = !char.IsDigit(e.Text.Last()) && (e.Text.Select(c=>c=='.').ToList().Count == 1);
        }

    }
}
