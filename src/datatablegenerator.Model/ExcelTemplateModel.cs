using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatablegenerator.Models
{
    public class ExcelTemplateModel : INotifyPropertyChanged
    {
        private string _TextContent;
        public string TextContent
        {
            get { return _TextContent; }
            set
            {
                if (_TextContent != value)
                {
                    _TextContent = value;
                    OnPropertyChanged(nameof(TextContent));
                }
            }
        }



        private string _ExcelPathName;
        public string ExcelPathName
        {
            get { return _ExcelPathName; }
            set
            {
                if (_ExcelPathName != value)
                {
                    _ExcelPathName = value;
                    OnPropertyChanged(nameof(ExcelPathName));
                }
            }
        }




        #region

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
