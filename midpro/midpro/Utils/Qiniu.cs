using Newtonsoft.Json.Linq;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midpro.Utils
{
    class Qiniu
    {
        public Qiniu() { }
        public static async Task<string> UploadData(byte[] data, string fileName, string username)
        {
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            // var data1 = DateTime.Today;
            Mac mac = new Mac("ApcMQFLH84Ra_Oh_oiSrj8x03YaeAL-JYq-EAEwI", "fGjF7mr002ucd5buvqud4QutECYdSqBpmYCJnxQc");
            string bucket = "zzzzzlf";
            string saveKey = username + fileName;

            // byte[] data = System.IO.File.ReadAllBytes("D:/QFL/1.mp3");
            //byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello World!");
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket + ":" + saveKey;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            FormUploader fu = new FormUploader();
            HttpResult result = await fu.UploadDataAsync(data, saveKey, token);
            JObject jobj = JObject.Parse(result.Text);
            // var i = new MessageDialog(jobj["key"].ToString()).ShowAsync();
            return "http://okuww23ih.bkt.clouddn.com/" + jobj["key"].ToString();
        }
    }
}
