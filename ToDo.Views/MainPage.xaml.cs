using ToDo.API;
using ToDo.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ToDo.Views
{
    public sealed partial class MainPage : Page
    {
        MainPageViewModel _mainPageVM;

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
