using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace midpro
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class signup : Page
    {
        public signup()
        {
            this.InitializeComponent();
        }

        private void userLogin_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(login));
        }

        private async void userSignup_Click(object sender, RoutedEventArgs e)
        {
            string _nickname = nickname.Text;
            string _username = username.Text;
            string _password = password.Password;
            if (_nickname.Equals("") || _username.Equals("") || _password.Equals(""))
            {
                await new MessageDialog("不能为空").ShowAsync();
                return;
            }

            HttpClient client = new HttpClient();
            try
            {
                string url = Config.config.prefix + "/user";
                var user = new Model.user(_username, _nickname, "http://img1.famulei.com/b/929239/p/162/1000380952218.jpg", _password);
                var temp = JsonConvert.SerializeObject(user);

                var content = new StringContent(temp);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await client.PostAsync(url, content);
                string result = await res.Content.ReadAsStringAsync();
                var resJson = JObject.Parse(result);
                if (resJson["status"].ToString().Equals("200"))
                {
                    await new MessageDialog(resJson["message"].ToString()).ShowAsync();
                    Frame.Navigate(typeof(login));
                }
                else
                {
                    await new MessageDialog(resJson["message"].ToString()).ShowAsync();
                }
            }
            catch(Exception ex)
            {
                var i = new MessageDialog("怕是你没有连网噢").ShowAsync();
            }

        }
    }
}
