using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tamturk.NoCaptcha {
    public class ReCaptchaValidator : IDisposable {
        public string public_key;

        protected string private_key;

        HttpClient client;

        public ReCaptchaValidator(string private_key, string public_key = null) {
            this.private_key = private_key;
            this.public_key = public_key;

            client = new HttpClient();
        }

        public async Task ValidateAsync(string response, string challenge, string remote_ip) {
            if (response == null) {
                throw new ArgumentNullException(paramName: nameof(response), message: "response is required");
            }

            if (challenge == null) {
                throw new ArgumentNullException(paramName: nameof(challenge), message: "challenge is required");
            }

            if (challenge == null) {
                throw new ArgumentNullException(paramName: nameof(remote_ip), message: "remote_ip is required");
            }

            using (var m = new HttpRequestMessage(HttpMethod.Post, "https://www.google.com/recaptcha/api/verify")) {
                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("secret", private_key));
                keyValues.Add(new KeyValuePair<string, string>("response", response));
                keyValues.Add(new KeyValuePair<string, string>("challenge", challenge));
                keyValues.Add(new KeyValuePair<string, string>("remoteip", remote_ip));
                m.Content = new FormUrlEncodedContent(keyValues);

                using (HttpResponseMessage _response = await client.SendAsync(m)) {
                    if (!_response.IsSuccessStatusCode) {
                        throw new HttpRequestException();
                    }

                    var a = await _response.Content.ReadAsStringAsync();
                    if (!Convert.ToBoolean((a.Substring(0, a.IndexOf("\n"))))) {
                        throw new CaptchaException();
                    }
                }
            }
        }

        public static async Task ValidateAsync(string private_key, string public_key, string response, string challenge, string remote_ip) {
            using (var validator = new ReCaptchaValidator(private_key, public_key)) {
                await validator.ValidateAsync(response, challenge, remote_ip);
            }
        }

        public void Dispose() {
            client.Dispose();
        }
    }
}
