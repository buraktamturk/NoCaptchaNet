using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamturk.NoCaptcha;

namespace Tamturk.AspNetCore.NoCaptcha {
    public class NoCaptchaRequstValidator {
        public HttpContext context;
        public NoCaptchaValidator validator;
        public NoCaptchaOptions options;

        public NoCaptchaRequstValidator(HttpContextAccessor context, NoCaptchaValidator validator, NoCaptchaOptions options) {
            this.context = context.HttpContext;
            this.validator = validator;
            this.options = options;
        }

        public async Task ValidateAsync() {
            string response = null;

            if(options.customTokenProvider != null) {
                response = options.customTokenProvider(context.Request);
            } else {
                response = context.Request.Headers["G-Token-Response"];
                if(response == null && context.Request.HasFormContentType) {
                    response = context.Request.Form["g-token-response"];
                }
            }

            if(response == null) {
                throw new CaptchaException("no channelge provided");
            }

            await validator.ValidateAsync(response);
        }
    }
}
