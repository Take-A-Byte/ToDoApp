using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ToDo.Views.Controls
{
    public sealed partial class EditableCheckbox : UserControl
    {
        private bool _isEditing;

        public EditableCheckbox()
        {
            InitializeComponent();
        }

        private bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;

                    if (_isEditing)
                    {
                        CheckboxLabel.Visibility = Visibility.Collapsed;
                        CheckboxTextbox.Visibility = Visibility.Visible;
                        CheckboxTextbox.Focus(FocusState.Programmatic);
                        CheckboxTextChangeIcon.Symbol = Symbol.Accept;
                    }
                    else
                    {
                        CheckboxLabel.Visibility = Visibility.Visible;
                        CheckboxTextbox.Visibility = Visibility.Collapsed;
                        CheckboxTextChangeIcon.Symbol = Symbol.Edit;
                    }
                }
            }
        }

        private void CheckboxTextbox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                CheckboxLabel.Text = CheckboxTextbox.Text;
                IsEditing = false;
            } else if(e.Key == VirtualKey.Escape)
            {
                IsEditing = false;
                CheckboxTextbox.Text = CheckboxLabel.Text;
            }

        }

        private void CheckboxLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToDoCheckbox.IsChecked = !ToDoCheckbox.IsChecked;
        }

        private void CheckboxTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
            CheckboxTextbox.Text = CheckboxLabel.Text;
        }

        private void Button_Click(object sender, TappedRoutedEventArgs e)
        {
            if (IsEditing)
            {
                IsEditing = false;
                CheckboxLabel.Text = CheckboxTextbox.Text;
            }
            else
            {
                CheckboxTextbox.Text = CheckboxLabel.Text;
                IsEditing = true;
            }
        }
    }
}
