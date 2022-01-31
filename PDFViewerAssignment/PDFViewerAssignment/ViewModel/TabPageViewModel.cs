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
    class TabPageViewModel : BaseViewModel
    {
        ToolManager _toolManagerPDF;

        public ICommand CMDOpenFile { get; set; }

        private PDFViewCtrl _pDFViewCtrl = new pdftron.PDF.PDFViewCtrl();
        public PDFViewCtrl MyPDFViewCtrl
        {
            get { return _pDFViewCtrl; }
            set { _pDFViewCtrl = value; NotifyPropertyChanged(); }
        }

        public TabPageViewModel()
        {
            // Init Commands
            CMDOpenFile = new RelayCommand(OpenFile);

            // ToolManager is initialized with the PDFViewCtrl and it activates all available tools
            _toolManagerPDF = new ToolManager(MyPDFViewCtrl);
        }

        async private void OpenFile()
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.List;

            // PDFs
            filePicker.FileTypeFilter.Add(".pdf");
            filePicker.FileTypeFilter.Add(".fdf");
            filePicker.FileTypeFilter.Add(".xfdf");

            // MS Office Docs
            filePicker.FileTypeFilter.Add(".doc");
            filePicker.FileTypeFilter.Add(".docx");
            filePicker.FileTypeFilter.Add(".xls");
            filePicker.FileTypeFilter.Add(".xlsx");
            filePicker.FileTypeFilter.Add(".ppt");
            filePicker.FileTypeFilter.Add(".pptx");
            filePicker.FileTypeFilter.Add(".pub");

            StorageFile file = await filePicker.PickSingleFileAsync();
            try
            {
                await OpenFilePDFViewer(file, FileAccessMode.ReadWrite);
            }
            catch (Exception e)
            {
                // In case something goes wrong when working with file
                var msg = new MessageDialog(e.Message);
                _ = msg.ShowAsync();
                return;
            }    
        }

        private bool IsPDFDoc(string fileType)
        {
            if (fileType == ".pdf" || fileType == ".fdf" || fileType == ".xfdf")
            {
                return true;
            }
            return false;
        }

        private async Task OpenFilePDFViewer(IStorageFile file, FileAccessMode mode)
        {
            if (file == null)
                return;

            if (IsPDFDoc(file.FileType))
            {
                // Is already a pdf doc
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
            else
            {
                // Convert Doc
                Windows.Storage.Streams.IRandomAccessStream stream;
                try
                {
                    stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                }
                catch (Exception e)
                {
                    // NOTE: If file already opened it will cause an exception
                    var msg = new MessageDialog(e.Message);
                    _ = msg.ShowAsync();
                    return;
                }

                // Convert Logic
                IFilter filter = new RandomAccessStreamFilter(stream);
                WordToPDFOptions opts = new WordToPDFOptions();
                DocumentConversion conversion = pdftron.PDF.Convert.UniversalConversion(filter, opts);

                var convRslt = conversion.TryConvert();

                if (convRslt == DocumentConversionResult.e_document_conversion_success)
                {
                    PDFDoc doc = conversion.GetDoc();
                    doc.InitSecurityHandler();

                    MyPDFViewCtrl.SetDoc(doc);
                }
            }
        }
    }
}
