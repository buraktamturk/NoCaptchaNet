using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamturk.NoCaptcha;

namespace Tamturk.AspNetCore.NoCaptcha {
    public class ReCaptchaRequstValidator {
        public HttpContext context;
        public ReCaptchaValidator validator;
        public ReCaptchaOptions options;

        public ReCaptchaRequstValidator(HttpContextAccessor context, ReCaptchaValidator validator, ReCaptchaOptions options) {
            this.context = context.HttpContext;
            this.validator = validator;
            this.options = options;
        }

        public async Task ValidateAsync() {
            string response = null, challenge = null;

            if (options.customTokenProvider != null) {
                if(!options.customTokenProvider(context.Request, out response, out challenge)) {
                    if (response == null) {
                        throw new CaptchaException("no challenge provided");
                    }
                }
            } else {
                response = context.Request.Headers["G-Token-Response"];
                challenge = context.Request.Headers["G-Token-Challenge"];
                if (response == null && context.Request.HasFormContentType) {
                    response = context.Request.Form["recaptcha_response_field"];
                    challenge = context.Request.Form["recaptcha_challenge_field"];
                }
                
                if (response == null || challenge == null) {
                    throw new CaptchaException("no challenge provided");
                }
            }
            
            await validator.ValidateAsync(response, challenge, context.Connection.RemoteIpAddress.ToString());
        }
    }
}
