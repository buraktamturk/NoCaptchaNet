using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tamturk.NoCaptcha {
    public class NoCaptchaValidator : IDisposable {
        public string public_key;

        protected string private_key;

        HttpClient client;

        public NoCaptchaValidator(string private_key, string public_key = null) {
            this.public_key = public_key;
            this.private_key = private_key;

            client = new HttpClient();
        }

        public async Task ValidateAsync(string response) {
            if (response == null) {
                throw new ArgumentNullException(paramName: nameof(response), message: "response is required");
            }

            using (var m = new HttpRequestMessage(HttpMethod.Post, "https://www.google.com/recaptcha/api/siteverify")) { 
                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("secret", private_key));
                keyValues.Add(new KeyValuePair<string, string>("response", response));
                m.Content = new FormUrlEncodedContent(keyValues);

                using (HttpResponseMessage _response = await client.SendAsync(m)) {
                    if(!_response.IsSuccessStatusCode) {
                        throw new HttpRequestException();
                    }

                    var result = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(await _response.Content.ReadAsStringAsync());
                    if(!result.success) {
                        throw new CaptchaException();
                    }
                }
            }
        }

        public static async Task ValidateAsync(string private_key, string public_key, string response) {
            using(var validator = new NoCaptchaValidator(private_key, public_key)) {
                await validator.ValidateAsync(response);
            }
        }
        
        public void Dispose() {
            client.Dispose();
        }
    }
}
