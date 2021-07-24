using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePKG_WPF.Related_functions
{
    class Release_file
    {
        /// <summary>
        /// 释放到嵌入程序中的资源文件
        /// </summary>
        /// <param name="byDll">资源文件字节组</param>
        /// <param name="file">释放文件路径</param>
        /// <returns>文件是否释放成功</returns>
        public static bool file(byte[] byDll,string file)
        {
            //byte[] byDll = Encoding.Default.GetBytes(Properties.Resources.RePKG);//获取嵌入文件的字节数组  
            //string strPath = Environment.GetEnvironmentVariable("TMP") + @"\MC_OFF.reg";//设置释放路径
            using (FileStream fs = new FileStream(file, FileMode.Create))//开始写入文件流
            {
                fs.Write(byDll, 0, byDll.Length);
            }

            if (File.Exists(file))//检索文件是否正确被释放
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 执行释放文件
        /// </summary>
        /// <param name="temp">释放文件路径</param>
        /// <returns>释放全部成功释放</returns>
        public static bool File_List(string temp)
        {
            if(file(Properties.Resources.RePKG,temp + @"\RePKG.exe") == true && file(Encoding.Default.GetBytes(Properties.Resources.THIRD_PARTY_NOTICES),temp + @"\THIRD-PARTY-NOTICES.txt") == true )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
