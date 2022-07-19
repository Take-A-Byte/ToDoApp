using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDo.ViewModels;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ToDo.Views.Controls
{
    public sealed partial class EditableTask : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(EditableTaskViewModel), typeof(EditableTask), new PropertyMetadata(null));

        private DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(2500) };

        public EditableTask()
        {
            InitializeComponent();
            _timer.Tick += OnInputErrorTeachingTipTimerTicked;
        }

        public EditableTaskViewModel ViewModel
        {
            get { return (EditableTaskViewModel)GetValue(ViewModelProperty); }
            set
            {
                if (ViewModel != null)
                {
                    ViewModel.IncorrectDescription -= ShowErrorTeachingTip;
                    ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
                }

                if (value != null)
                {
                    value.IncorrectDescription += ShowErrorTeachingTip;
                    value.PropertyChanged += OnViewModelPropertyChanged;
                }
                SetValue(ViewModelProperty, value);
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.CurrentTaskState):
                    UpdateState();
                    break;
            }
        }

        private void SetProgramticFocusOnTextbox()
        {
            TaskTextbox.SelectionStart = TaskTextbox.Text.Length;
            TaskTextbox.Focus(FocusState.Programmatic);
        }

        private void UpdateState()
        {
            switch (ViewModel.CurrentTaskState)
            {
                case TaskState.Adding:
                    VisualStateManager.GoToState(this, "AddingTask", false);
                    break;
                case TaskState.Added:
                    VisualStateManager.GoToState(this, "AddedTask", false);
                    break;
                case TaskState.Editing:
                    VisualStateManager.GoToState(this, "EditingTask", false);
                    SetProgramticFocusOnTextbox();
                    break;
                default:
                    Debug.Fail("Unhandled task state");
                    break;
            }
        }

        private void OnInputErrorTeachingTipTimerTicked(object sender, object e)
        {
            _timer.Stop();
            InputErrorTeachingTip.IsOpen = false;
        }

        private void ShowErrorTeachingTip(string errorMessage)
        {
            InputErrorTeachingTip.Subtitle = errorMessage;
            InputErrorTeachingTip.IsOpen = true;
            _timer.Start();
            SetProgramticFocusOnTextbox();
        }

        private void OnEditableTaskLoaded(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }

        private async void OnTaskTextboxPreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                await ViewModel.AcceptChanges();
            }
            else if (e.Key == VirtualKey.Escape)
            {
                ViewModel.DiscardChanges();
            }
        }

        private void OnTaskTextboxLostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.DiscardChanges();
        }

        private void OnTaskLabelTapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.HasCompleted = !ViewModel.HasCompleted;
        }

        private async void OnTaskStateUpdateButtonClicked(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.CurrentTaskState == TaskState.Added)
            {
                ViewModel.SwitchToEdit();
            }
            else
            {
                await ViewModel.AcceptChanges();
            }
        }
    }
}