using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tamturk.AspNetCore.NoCaptcha {
    public class NoCaptchaOptions {
        public Func<HttpRequest, string> customTokenProvider;
    }
}
