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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace midpro
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class details : Page
    {
        private ViewModel.dakaVM VM;
        private ViewModel.commentVM _VM;
        private string nickname;

        public details()
        {
            this.InitializeComponent();
            _VM = new ViewModel.commentVM();
        }
        private async void initCommentVM()
        {
            HttpClient client = new HttpClient();
            try
            {
                string url = Config.config.prefix + "/comments?id=" + VM.SelectedItem.id;

                var res = await client.GetAsync(url);
                string content = await res.Content.ReadAsStringAsync();
                var obj = JObject.Parse(content);

                _VM.AddItem(obj);
            }
            catch (Exception ex)
            {
                var i = new MessageDialog("怕是你没连网哦").ShowAsync();
            }
        }

        private async void getUser()
        {
            try
            {
                HttpClient client = new HttpClient();
                var _result = await client.GetAsync(Config.config.prefix + "/user");
                string _content = await _result.Content.ReadAsStringAsync();

                var obj = JObject.Parse(_content);
                // avatar.ImageSource = new BitmapImage(new Uri(obj["avatar"].ToString()));
                // nickname.Text = obj["nickname"].ToString();
                nickname = obj["nickname"].ToString();
            }
            catch (Exception ex)
            {
                await new MessageDialog("怕是你没有联网哦").ShowAsync();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            VM = ((ViewModel.dakaVM)e.Parameter);
            if (VM.SelectedItem != null)
            {
                avatar.ImageSource = VM.SelectedItem.avatar;
                content.Text = VM.SelectedItem.content;
                name.Text = VM.SelectedItem.nickname;
                image.Source = VM.SelectedItem.src;
                createdTime.Text = VM.SelectedItem.createdTime;
                initCommentVM();
                getUser();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            xxxx.Visibility = xxxx.Visibility.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void Comment_Click(object sender, RoutedEventArgs e)
        {
            string content = commentBox.Text;
            if (content != "")
            {
                try
                {
                    string url = Config.config.prefix + "/comments";
                    HttpClient client = new HttpClient();
                    Model.comment obj = new Model.comment(nickname, VM.SelectedItem.id, content);
                    var temp = JsonConvert.SerializeObject(obj);
                    var _content = new StringContent(temp);
                    _content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var res = await client.PostAsync(url, _content);
                    string result = await res.Content.ReadAsStringAsync();
                    var resJson = JObject.Parse(result);
                    if (resJson["status"].ToString().Equals("200"))
                    {
                        this.initCommentVM();
                        commentBox.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog("怕是你没连网哦").ShowAsync();
                }
            } else
            {
                await new MessageDialog("不能为空").ShowAsync();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
