using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamturk.NoCaptcha;

namespace Tamturk.AspNetCore.NoCaptcha {
    public static class ReCaptchaExtensions {
        public static IServiceCollection UseNoCaptchaValidator(this IServiceCollection services, string private_key, string public_key = null, ReCaptchaOptions options = null) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            return services
                    .AddSingleton(new ReCaptchaValidator(private_key, public_key))
                    .AddSingleton(options ?? new ReCaptchaOptions())
                    .AddScoped<ReCaptchaRequestValidator>();
        }
    }
}
