using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

using winui = Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;

namespace PDFViewerAssignment.View
{
    public sealed partial class WelcomePage : Page
    {
        // MyPDFViewCtrl for Viewing PDFs
        pdftron.PDF.PDFViewCtrl MyPDFViewCtrl;

        public WelcomePage()
        {
            this.InitializeComponent();

            MyPDFViewCtrl = new pdftron.PDF.PDFViewCtrl();
            PDFViewBorder.Child = MyPDFViewCtrl;
            InitWelcomePage();
        }

        // Apply the README.pdf from Resources folder to the page contents
        async void InitWelcomePage()
        {
            try
            {
                pdftron.PDF.PDFDoc doc = new pdftron.PDF.PDFDoc("Resources/README.pdf");
                MyPDFViewCtrl.SetDoc(doc);

            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "Error accessing file").ShowAsync();
            }
        }
    }
}
