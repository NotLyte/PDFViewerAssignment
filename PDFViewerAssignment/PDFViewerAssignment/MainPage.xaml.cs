using Windows.UI.Xaml.Controls;
using winui = Microsoft.UI.Xaml.Controls;
using PDFViewerAssignment.View;
using PDFViewerAssignment.ViewModel;

namespace PDFViewerAssignment
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // Init Demo Mode!
            pdftron.PDFNet.Initialize();
            // Add resources search path - for fonts when converting MS Office docs
            pdftron.PDFNet.AddResourceSearchPath("./Resources");

            AddWelcomeTab();
        }

        // Add Welcome Tab and Page
        private void AddWelcomeTab()
        {
            var newTab = new winui.TabViewItem();
            newTab.IconSource = new winui.SymbolIconSource() { Symbol = Symbol.Home };
            newTab.Header = "Welcome";

            Frame frame = new Frame();
            newTab.Content = frame;
            frame.Navigate(typeof(WelcomePage));

            MyTabView.TabItems.Add(newTab);
        }

        // Add a new Tab to the TabView
        private void TabView_AddTabButtonClick(winui.TabView sender, object args)
        {
            var newTab = new winui.TabViewItem();
            newTab.IconSource = new winui.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "New Document";

            Frame frame = new Frame();
            newTab.Content = frame;
            frame.Navigate(typeof(TabPage));

            sender.TabItems.Add(newTab);

            MyTabView.SelectedIndex = MyTabView.TabItems.Count - 1;
        }

        // Remove the requested tab from the TabView
        private void TabView_TabCloseRequested(winui.TabView sender, winui.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }
    }
}
