using datatablegenerator.Common;
using datatablegenerator.Common.Ado;
using datatablegenerator.Common.Utils;
using datatablegenerator.Enums;
using datatablegenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace datatablegenerator.ViewModels
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private AdoProviderBase _adoProvider;
        public ConnectModel _ConnectModel { get; set; } = new ConnectModel();
        public DBCheckModel _DBCheckModel { get; set; } = new DBCheckModel();
        public ExcelTemplateModel _ExcelTemplateModel { get; set; } = new ExcelTemplateModel();

        private ObservableCollection<TableModel> _TableModelList = new ObservableCollection<TableModel>();
        public ObservableCollection<TableModel> TableModelList
        {
            get { return _TableModelList; }
            set
            {
                if (_TableModelList != value)
                {
                    _TableModelList = value;
                    OnPropertyChanged(nameof(TableModelList));
                }
            }
        }

        #region 连接/测试    
        /// <summary>
        /// 连接/测试 Command
        /// </summary>
        private ICommand? _ConnectCommand;
        public ICommand? ConnectCommand
        {
            get
            {
                return _ConnectCommand ?? (_ConnectCommand = new RelayCommand<object?>(ConnectionDB));
            }
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public void ConnectionDB(object? commandParameter)
        {
            var connectionString = GetConnectionString();
            _ConnectModel.DataBaseName = GetDatabaseName(connectionString);

            var provider = AdoFactory.GetProvider(_ConnectModel.DataSourceType, connectionString);
            var isok = provider.Connection();
            if (!isok)
            {
                MessageBox.Show("连接失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            UpdateConnectStatus(isok);
            _adoProvider = provider;
            MessageBox.Show("连接成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="connTxt">连接字符串</param>
        /// <returns></returns>
        private string GetDatabaseName(string connTxt)
        {
            return connTxt.Split(";").ToList().TakeLast(2).First().Split("=").ToList().TakeLast(1).First();
        }
        /// <summary>
        /// 修改连接状态
        /// </summary>
        /// <param name="status"></param>
        public void UpdateConnectStatus(bool status)
        {
            if (status)
            {
                _ConnectModel.ConnectStatus = status;
                _ConnectModel.ConnectStatusText = "已连接";
                _ConnectModel.ConnectStatusForeground = "Green";
            }
            else
            {
                _ConnectModel.ConnectStatus = status;
                _ConnectModel.ConnectStatusText = "未连接";
                _ConnectModel.ConnectStatusForeground = "Red";
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public string GetConnectionString()
        {
            if (_DBCheckModel.IsMySQL)
            {
                _ConnectModel.DataSourceType = DataSourceType.MySQL;
                return _ConnectModel.ConnectionString;
            }

            if (_DBCheckModel.IsMSSQL)
            {
                _ConnectModel.DataSourceType = DataSourceType.MSSQL;
                return _ConnectModel.ConnectionString + ";trustServerCertificate=true";
            }
            return _ConnectModel.ConnectionString;
        }

        #endregion


        #region 模版解析

        /// <summary>
        /// 模版解析 Command
        /// </summary>
        private ICommand? _ExcelAnalysisCommand;
        public ICommand? ExcelAnalysisCommand
        {
            get
            {
                return _ExcelAnalysisCommand ?? (_ExcelAnalysisCommand = new RelayCommand<object>(ExcelAnalysis));
            }
        }

        public void ExcelAnalysis(object parameter)
        {
            //Excel 模版数据 
            TextRange tr = new TextRange(((RichTextBox)parameter).Document.ContentStart, ((RichTextBox)parameter).Document.ContentEnd);
            var excelTemplateText = tr.Text;
            if (string.IsNullOrWhiteSpace(excelTemplateText))
            {
                MessageBox.Show("空内容不能解析！");
                return;
            }

            //excel 文本内容
            string[] excelTxtArr = excelTemplateText.Split("\r\n").ToArray();

            //表头信息
            var tableHeadList = excelTxtArr[0].Split("\t").Where(r => r != "").ToList();

            ///检查表头名称
            foreach (var item in tableHeadList)
            {
                if (!TableTemplateGroup.Exist(item))
                {
                    MessageBox.Show(@$"表头【{item}】,名称错误！");
                    return;
                }
            }

            List<TableModel> tableModel = new List<TableModel>();
            foreach (var item in excelTxtArr.Skip(1))
            {
                TableModel execlTemplateModel = new TableModel();
                string[] tempStrArr = item.Split("\t").ToArray();

                if (tempStrArr.Length < execlTemplateModel.GetType().GetProperties().Count())
                {
                    break;
                }
                //TODO:待优化
                // 欢迎大家指点 ^-^
                for (int i = 0; i < tempStrArr.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            execlTemplateModel.Name = tempStrArr[i];
                            break;
                        case 1:
                            execlTemplateModel.Type = tempStrArr[i];
                            break;
                        case 2:
                            execlTemplateModel.Required = tempStrArr[i] == "*" ? true : false;
                            break;
                        case 3:
                            execlTemplateModel.Default = tempStrArr[i];
                            break;
                        case 4:
                            execlTemplateModel.Constraint = tempStrArr[i];
                            break;
                        case 5:
                            execlTemplateModel.Remacks = tempStrArr[i];
                            break;
                        default:
                            break;
                    }
                }
                _TableModelList.Add(execlTemplateModel);
            }
        }

        #endregion


        #region 清除

        /// <summary>
        /// 清除
        /// </summary>
        private ICommand? _ClearCommand;
        public ICommand? ClearCommand
        {
            get
            {
                return _ClearCommand ?? (_ClearCommand = new RelayCommand<object>(Clear));
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="parameter"></param>
        public void Clear(object parameter)
        {
            TextRange tr = new TextRange(((RichTextBox)parameter).Document.ContentStart, ((RichTextBox)parameter).Document.ContentEnd);
            tr.Text = string.Empty;
            TableModelList = new ObservableCollection<TableModel>();
            _ConnectModel.TableName = string.Empty;
        }
        #endregion


        #region

        /// <summary>
        /// 确认提交
        /// </summary>
        private ICommand? _SubmitCommand;
        public ICommand? SubmitCommand
        {
            get
            {
                return _SubmitCommand ?? (_SubmitCommand = new RelayCommand<object>(Submit));
            }
        }

        /// <summary>
        /// 确认提交
        /// </summary>
        /// <param name="parameter"></param>
        public async void Submit(object parameter)
        {
            if (!_ConnectModel.ConnectStatus)
            {
                MessageBox.Show("请先连接数据库！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //表名
            string tableName = _ConnectModel.TableName.Trim();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                MessageBox.Show($"请先输入表名！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var isExist = await _adoProvider.ExistTable(tableName);
            if (isExist)
            {
                MessageBox.Show($"该数据表:{tableName}已存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var result = await _adoProvider.GeneraterTable(_ConnectModel.DataBaseName, tableName, TableModelList);
            if (!result)
            {
                MessageBox.Show($"{result.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            MessageBox.Show($"【{tableName}】表，生成成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Clear(parameter);
        }

        #endregion


        #region

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
