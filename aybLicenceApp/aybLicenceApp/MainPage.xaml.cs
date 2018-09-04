using Newtonsoft.Json;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aybLicenceApp
{
	public partial class MainPage : ContentPage
	{

        ResponseLicence postInfo = new ResponseLicence();

        public MainPage()
		{
			InitializeComponent();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Information", consumeApi(), "OK");
        }

        public string consumeApi()
        {
            string idCommerce = txtCommerce.Text;
            string deviceid = CrossDeviceInfo.Current.Id;

            Task<ResponseLicence> task = Task.Run<ResponseLicence>(async () => await saveLicence(deviceid, idCommerce));
            return task.Result.ResponseMessage;
        }


        public async Task<ResponseLicence> saveLicence(string deviceID, string commerceID)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Application.Current.Resources["wapiUrl"].ToString());
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.GetAsync("/api/Licence/" + deviceID + "/" + commerceID);

                    return JsonConvert.DeserializeObject<ResponseLicence>(await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                ResponseLicence objResponse = new ResponseLicence();
                objResponse.ResponseCode = "001";
                objResponse.ResponseMessage = "Error consuming api: " + ex.Message;
                return objResponse;
            }
        }

    }
}
