using System;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ToDo.Views.Controls
{
    public enum TaskState
    {
        Adding,
        Added,
        Editing
    }

    public sealed partial class EditableTask : UserControl
    {
        public static readonly DependencyProperty CurrentTaskStateProperty =
            DependencyProperty.Register("CurrentTaskStateProperty", typeof(TaskState), typeof(EditableTask), new PropertyMetadata(TaskState.Added, OnTaskStateChanged));

        public static readonly DependencyProperty HasCompletedProperty =
            DependencyProperty.Register("HasCompleted", typeof(bool), typeof(EditableTask), new PropertyMetadata(false));

        public static readonly DependencyProperty TaskDescriptionProperty =
            DependencyProperty.Register("TaskDescription", typeof(string), typeof(EditableTask), new PropertyMetadata(string.Empty));

        public EditableTask()
        {
            InitializeComponent();
        }

        public TaskState CurrentTaskState
        {
            get { return (TaskState)GetValue(CurrentTaskStateProperty); }
            set { SetValue(CurrentTaskStateProperty, value); }
        }

        public bool HasCompleted
        {
            get { return (bool)GetValue(HasCompletedProperty); }
            set { 
                if(CurrentTaskState == TaskState.Adding && value)
                {
                    Debug.Fail("Task which is not yet added cannot be complete");
                    return;
                }

                SetValue(HasCompletedProperty, value); 
            }
        }

        public string TaskDescription
        {
            get { return (string)GetValue(TaskDescriptionProperty); }
            set { 
                if(CurrentTaskState == TaskState.Adding)
                {
                    Debug.Fail("Task which is not yet added cannot have task description");
                    return;
                }

                SetValue(TaskDescriptionProperty, value); 
            }
        }

        private void UpdateState()
        {
            switch (CurrentTaskState)
            {
                case TaskState.Adding:
                    VisualStateManager.GoToState(this, "AddingTask", false);
                    break;
                case TaskState.Added:
                    VisualStateManager.GoToState(this, "AddedTask", false);
                    break;
                case TaskState.Editing:
                    VisualStateManager.GoToState(this, "EditingTask", false);
                    TaskTextbox.Focus(FocusState.Programmatic);
                    break;
                default:
                    Debug.Fail("Unhandled task state");
                    break;
            }
        }

        private void AcceptChanges()
        {
            if (!String.IsNullOrEmpty(TaskTextbox.Text))
            {
                TaskLabel.Text = TaskTextbox.Text;
                CurrentTaskState = TaskState.Added;
            }
        }

        private void DiscardChanges()
        {
            if (CurrentTaskState == TaskState.Adding)
            {
                TaskTextbox.Text = String.Empty;
            }
            else
            {
                CurrentTaskState = TaskState.Added;
                TaskTextbox.Text = TaskLabel.Text;
            }
        }

        private void OnEditableTaskLoaded(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }

        private static void OnTaskStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EditableTask editableTask = (EditableTask)d;

            editableTask.UpdateState();
        }

        private void OnTaskTextboxPreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                AcceptChanges();
            }
            else if (e.Key == VirtualKey.Escape)
            {
                DiscardChanges();
            }

        }

        private void OnTaskTextboxLostFocus(object sender, RoutedEventArgs e)
        {
            DiscardChanges();
        }

        private void OnTaskLabelTapped(object sender, TappedRoutedEventArgs e)
        {
            TaskCheckbox.IsChecked = !TaskCheckbox.IsChecked;
        }

        private void OnTaskStateUpdateButtonClicked(object sender, TappedRoutedEventArgs e)
        {
            if (CurrentTaskState == TaskState.Added)
            {
                TaskTextbox.Text = TaskLabel.Text;
                TaskTextbox.SelectionStart = TaskTextbox.Text.Length;
                CurrentTaskState = TaskState.Editing;
            }
            else
            {
                AcceptChanges();
            }
        }
    }
}
