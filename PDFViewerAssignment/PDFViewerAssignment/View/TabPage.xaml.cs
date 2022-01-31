using Windows.UI.Xaml.Controls;
using winui = Microsoft.UI.Xaml.Controls;
using PDFViewerAssignment.View;
using PDFViewerAssignment.ViewModel;

namespace PDFViewerAssignment.View
{
    public sealed partial class TabPage : Page
    {
        TabPageViewModel mViewModel;

        // Init
        public TabPage()
        {
            this.InitializeComponent();
            this.DataContext = mViewModel = new TabPageViewModel();

            if (mViewModel.CMDOpenFile.CanExecute(null))
            {
                mViewModel.CMDOpenFile.Execute(null);
            }
        }
    }
}
