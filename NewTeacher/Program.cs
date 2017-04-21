﻿using Helpers;
using SharedForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTeacher
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew;
            using (System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            {
                if (createNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //初始化表情资源
                    GlobalResourceManager.Initialize();
                    #region 线程异常处理
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    #endregion
                    Login frm = new Login();
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new BaseForm());
                    }

                   // Application.Run(new CskinForm());
                }
                else
                {
                    MessageBox.Show("该程序己启动");
                }
            }


        }

        /// <summary>
        /// 线程异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Loger.LogMessage(e.Exception.ToString());
            MessageBox.Show(e.Exception.Message);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Loger.LogMessage(e.ExceptionObject.ToString());
            MessageBox.Show(e.ExceptionObject.ToString());

        }
    }
}