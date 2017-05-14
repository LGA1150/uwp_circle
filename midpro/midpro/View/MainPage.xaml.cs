using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace midpro
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ViewModel.dakaVM VM { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            this.VM = new ViewModel.dakaVM();
            this.VM.allItemsInit();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mySplit.IsPaneOpen = !mySplit.IsPaneOpen;
        }

        private void listView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            TextBlock obj = (TextBlock)e.ClickedItem;
            string expression = obj.Text.ToString();

            switch (expression)
            {
                case "主页": break;
                case "退出": logoutHandle(); break;
                case "打卡": Frame.Navigate(typeof(daKa)); break;
                case "修改信息": Frame.Navigate(typeof(setting));break;
                case "我的打卡": Frame.Navigate(typeof(myDaKas)); break;
            }
        }

        private void daka_ItemClick(object sender, ItemClickEventArgs e)
        {
            VM.SelectedItem = (Model.daKa)(e.ClickedItem);
            if (VM.SelectedItem != null)
            {
                Frame.Navigate(typeof(details), VM);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VM.allItemsInit();
        }

        private async void logoutHandle()
        {
            using (var statement = App.conn.Prepare("DELETE FROM user WHERE anchor = ?"))
            {
                statement.Bind(1, "zhulifeng");
                statement.Step();
            }
            HttpClient client = new HttpClient();
            try
            {
                var _result = await client.GetAsync(Config.config.prefix + "/logout");
                string _content = await _result.Content.ReadAsStringAsync();

                var obj = JObject.Parse(_content);
                if (obj["status"].ToString().Equals("200"))
                {
                    var i = new MessageDialog(obj["message"].ToString()).ShowAsync();
                    Frame.Navigate(typeof(login));
                }
            }
            catch(Exception ex)
            {
                var i = new MessageDialog("怕是你没有连网哦").ShowAsync();
            }
        }
    }
}
