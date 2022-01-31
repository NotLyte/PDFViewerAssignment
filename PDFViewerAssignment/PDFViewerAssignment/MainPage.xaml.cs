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
        }

        // Add a new Tab to the TabView
        private void TabView_AddTabButtonClick(winui.TabView sender, object args)
        {
            var newTab = new winui.TabViewItem();
            newTab.IconSource = new winui.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "New Document";

            // The Content of a TabViewItem is often a frame which hosts a page.
            Frame frame = new Frame();
            newTab.Content = frame;
            frame.Navigate(typeof(TabPage));

            sender.TabItems.Add(newTab);
        }

        // Remove the requested tab from the TabView
        private void TabView_TabCloseRequested(winui.TabView sender, winui.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }
    }
}
