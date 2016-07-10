using System;

namespace Tamturk.NoCaptcha {
    public class CaptchaException : Exception {
        public CaptchaException() { }

        public CaptchaException(string message) : base(message) {

        }
    }
}
