using Windows.UI.Xaml.Controls;
using winui = Microsoft.UI.Xaml.Controls;
using PDFViewerAssignment.View;
using PDFViewerAssignment.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PDFViewerAssignment.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TabPage : Page
    {
        TabPageViewModel mViewModel;
        public TabPage()
        {
            this.InitializeComponent();
            this.DataContext = mViewModel = new TabPageViewModel();
        }
    }
}
