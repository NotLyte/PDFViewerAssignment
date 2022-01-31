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
    // IConvertible for string file extensions, specifically for PDFs
    class DocumentType : IConvertible
    {
        string documentType;

        public DocumentType(string documentType)
        {
            this.documentType = documentType;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            if (documentType == ".pdf" || documentType == ".fdf" || documentType == ".xfdf")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public string ToString(IFormatProvider provider)
        {
            return documentType;
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
    }

    class TabPageViewModel : BaseViewModel
    {
        #region Initializations
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
        }
        #endregion

        #region OpenFile and Convert
        /// <summary>
        /// Opens file picker and filters for pdfs and ms docs
        /// </summary>
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
            DocumentType docType = new DocumentType(fileType);
            return System.Convert.ToBoolean(docType);
        }

        /// <summary>
        /// Attempts to open PDFs or MS Docs, if it is a MS Doc then convert it to pdf, then display pdf
        /// </summary>
        /// <param name="file"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
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
        #endregion
    }
}
