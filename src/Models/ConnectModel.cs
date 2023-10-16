using datatablegenerator.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace datatablegenerator.Models
{
    public class ConnectModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 数据库名称
        /// </summary> 
        private string _DBName = "Microsoft ® SQL Server";
        public string DBName
        {
            get { return _DBName; }
            set
            {
                if (_DBName != value)
                {
                    _DBName = value;
                    OnPropertyChanged(nameof(DBName));
                }
            }
        }


        /// <summary>
        /// 连接字符串
        /// </summary> 
        private string _ConnectionString = "server=服务器地址或数据库实例;uid=xxx;pwd=xxxxx;database=数据库名";
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set
            {
                if (_ConnectionString != value)
                {
                    _ConnectionString = value;
                    OnPropertyChanged(nameof(ConnectionString));
                }
            }
        }


        /// <summary>
        /// 连接状态
        /// </summary> 
        private bool _ConnectStatus = false;
        public bool ConnectStatus
        {
            get { return _ConnectStatus; }
            set
            {
                if (_ConnectStatus != value)
                {
                    _ConnectStatus = value;
                    OnPropertyChanged(nameof(ConnectStatus));
                }
            }
        }

        /// <summary>
        /// 连接状态文案
        /// </summary>
        private string _ConnectStatusText = "未连接";
        public string ConnectStatusText
        {
            get { return _ConnectStatusText; }
            set
            {
                if (_ConnectStatusText != value)
                {
                    _ConnectStatusText = value;
                    OnPropertyChanged(nameof(ConnectStatusText));
                }
            }
        }

        /// <summary>
        /// 连接状态颜色
        /// </summary>        
        private string _ConnectStatusForeground = "Red";
        public string ConnectStatusForeground
        {
            get { return _ConnectStatusForeground; }
            set
            {
                if (_ConnectStatusForeground != value)
                {
                    _ConnectStatusForeground = value;
                    OnPropertyChanged(nameof(ConnectStatusForeground));
                }
            }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        private string _DataBaseName;
        public string DataBaseName
        {
            get { return _DataBaseName; }
            set
            {
                if (_DataBaseName != value)
                {
                    _DataBaseName = value;
                    OnPropertyChanged(nameof(DataBaseName));
                }
            }
        }

        /// <summary>
        /// 表名
        /// </summary>        
        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set
            {
                if (_TableName != value)
                {
                    _TableName = value;
                    OnPropertyChanged(nameof(TableName));
                }
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        private DataSourceType _DataSourceType = DataSourceType.MSSQL;
        public DataSourceType DataSourceType
        {
            get { return _DataSourceType; }
            set
            {
                if (_DataSourceType != value)
                {
                    _DataSourceType = value;
                    OnPropertyChanged(nameof(DataSourceType));
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
