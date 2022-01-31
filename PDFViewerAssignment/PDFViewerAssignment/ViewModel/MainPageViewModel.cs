using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Printing;
using Windows.ApplicationModel.DataTransfer;

using pdftron.PDF;
using pdftron.PDF.Tools;
using pdftron.PDF.Tools.Controls;
using pdftron.Filters;


namespace PDFViewerAssignment.ViewModel
{
    class MainPageViewModel : BaseViewModel
    {
        //ToolManager _toolManagerPDF;

        public ICommand CMDOpenFile { get; set; }

        public PDFViewCtrl _pDFViewCtrl = new pdftron.PDF.PDFViewCtrl();
        public PDFViewCtrl MyPDFViewCtrl
        {
            get { return _pDFViewCtrl; }
            set { _pDFViewCtrl = value; NotifyPropertyChanged(); }
        }

        public MainPageViewModel()
        {
            // Init Commands
            CMDOpenFile = new RelayCommand(OpenFile);

            // Set control background color to gray
            MyPDFViewCtrl.SetBackgroundColor(Windows.UI.Color.FromArgb(100, 49, 51, 53));

            // ToolManager is initialized with the PDFViewCtrl and it activates all available tools
            //_toolManagerPDF = new ToolManager(PDFViewCtrl);
        }

        async private void OpenFile()
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.List;
            filePicker.FileTypeFilter.Add(".pdf");

            StorageFile file = await filePicker.PickSingleFileAsync();

            await OpenFilePDFViewer(file, FileAccessMode.ReadWrite);
        }

        private async Task OpenFilePDFViewer(IStorageFile file, FileAccessMode mode)
        {
            if (file == null)
                return;

            Windows.Storage.Streams.IRandomAccessStream stream;
            try
            {
                stream = await file.OpenAsync(mode);
            }
            catch (Exception e)
            {
                // NOTE: If file already opened it will cause an exception
                var msg = new MessageDialog(e.Message);
                _ = msg.ShowAsync();
                return;
            }

            PDFDoc doc = new PDFDoc(stream);
            doc.InitSecurityHandler();

            MyPDFViewCtrl.SetDoc(doc);
        }
    }
}
