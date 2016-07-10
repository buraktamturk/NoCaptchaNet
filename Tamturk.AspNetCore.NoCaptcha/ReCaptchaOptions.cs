using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tamturk.AspNetCore.NoCaptcha {
    public delegate bool ReCaptchaTokenProviderDelegate(HttpRequest request, out string response, out string challenge);

    public class ReCaptchaOptions {
        public ReCaptchaTokenProviderDelegate customTokenProvider;
    }
}
