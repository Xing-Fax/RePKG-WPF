using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace RePKG_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        static string Temp_file = Environment.GetEnvironmentVariable("TMP");//获取用户临时文件夹路径
        static string Desktop_file = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//获取用户桌面文件夹路径
        static string out_file = Desktop_file + @"\RePKG输出";//默认输出文件夹
        static ArrayList Number_file = new ArrayList();//存储输入文件列表

        public MainWindow()
        {
            InitializeComponent();
            日志.Text = "[" + DateTime.Now.ToString() + "]: RePKG-WPF 程序启动  Copyright © xcz  2021";
            日志.Text += "\n[" + DateTime.Now.ToString() + "]: 获取临时文件夹：" + Temp_file;
            日志.Text += "\n[" + DateTime.Now.ToString() + "]: 获取桌面路径：" + Desktop_file;
            日志.Text += "\n[" + DateTime.Now.ToString() + "]: 程序准备就绪...";
            输出路径.Text = out_file;
        }
        //添加文件
        private void 选择_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PKG文件(*.PKG)|*.pkg";
            ofd.Multiselect = true;
            if (ofd.ShowDialog(this) == true)
            {
                foreach (string file in ofd.FileNames)
                {
                    if(Number_file.Contains(file) == false)//排除同类项
                    {
                        Number_file.Add(file);//将元素添加到数组末尾
                        文件列表.Items.Add(file);
                        日志.Text += "\n[" + DateTime.Now.ToString() + "]: 已添加文件：" + file;
                    }
                    else
                    {
                        日志.Text += "\n[" + DateTime.Now.ToString() + "]: 添加失败!原因：已存在此文件";
                    }
                }
            }
            文件下拉框.Header = "已选择" + Number_file.Count + "个对象";
        }
        //清空文件列表
        private void 清空文件_Click(object sender, RoutedEventArgs e)
        {
            Number_file.Clear();
            文件下拉框.Header = "已选择0个对象";
            文件列表.Items.Clear();
            日志.Text += "\n[" + DateTime.Now.ToString() + "]: 清除所选文件列表";
        }
        //清除日志文件
        private void 清除日志_Click(object sender, RoutedEventArgs e)
        {
            日志.Text = null;
            日志.Text = "[" + DateTime.Now.ToString() + "]: 已清空日志...";
        }
        //选择输出目录
        private void 目录_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                if (dialog.FileName != "")
                {
                    输出路径.Text = dialog.FileName;
                    out_file = dialog.FileName;
                    日志.Text += "\n[" + DateTime.Now.ToString() + "]: 重新定向输出文件夹：" + dialog.FileName;
                }
            }
            catch { }
        }
        //开始输出
        private void 开始_Click(object sender, RoutedEventArgs e)
        {
            if(Number_file .Count == 0 )
            {
                日志.Text += "\n[" + DateTime.Now.ToString() + "]: 您似乎木有选择要解压的PKG文件w(ﾟДﾟ)w";
            }
            else
            {
                日志.Text += "\n[" + DateTime.Now.ToString() + "]: 释放核心库文件...";
                Related_functions.Release_file.File_List(Temp_file);//释放Repkg.exe

                if (Directory.Exists(out_file) == false)//判断输出目录是否存在
                {
                    日志.Text += "\n[" + DateTime.Now.ToString() + "]: 输出目录不存在,重新创建目录";
                    Directory.CreateDirectory(out_file);//创建新路径
                }
                for (int i = 0; i < Number_file .Count; i ++)
                {
                    日志.Text += "\n[" + DateTime.Now.ToString() + "]: 开始执行第" + (i + 1) + "个文件";
                    日志.Text += "\n[" + DateTime.Now.ToString() + "]: 源文件：" + Number_file[i];
                    日志.Text += "\n[" + DateTime.Now.ToString() + "]: 输出路径：" + out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString());

                    using (BackgroundWorker bw = new BackgroundWorker())
                    {
                        bw.DoWork += new DoWorkEventHandler(bw_DoWork2);//建立后台
                        bw.RunWorkerAsync(Temp_file + @"\RePKG.exe extract " + Number_file[i] + " -o" + out_file + @"\" + System.IO.Path.GetFileNameWithoutExtension(Number_file[i].ToString()));//开始执行
                    }
                    日志.Text += str;
                }
            }
        }

        static string str = "";//存储返回结果
        void bw_DoWork2(object sender, DoWorkEventArgs e)//后台
        {
            str = Related_functions.CMD.RunCmd(e.Argument.ToString());
        }

        private void 打开目录__Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(out_file) == false)//判断输出目录是否存在
            {
                日志.Text += "\n[" + DateTime.Now.ToString() + "]: 输出目录不存在,重新创建目录";
                Directory.CreateDirectory(out_file);//创建新路径
                System.Diagnostics.Process.Start("explorer.exe", out_file);
            }
        }
    }
}
