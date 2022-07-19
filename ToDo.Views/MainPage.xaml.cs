using System.Collections.Generic;
using System.Linq;
using ToDo.API;
using ToDo.ViewModels;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ToDo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainPageViewModel _mainPageVM;

        public List<string> _taskDescriptions { get => _mainPageVM.Tasks.Select(task => task.Description).ToList(); }

        public MainPage()
        {
            InitializeComponent();
            _mainPageVM = new MainPageViewModel();
        }

        private void SearchQueryChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && args.CheckCurrent())
            {
                _mainPageVM.FilterTasks(sender.Text);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await _mainPageVM.Initialize((ITaskController)e.Parameter);
        }
    }
}
