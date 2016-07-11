using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamturk.NoCaptcha;

namespace Tamturk.AspNetCore.NoCaptcha {
    public static class NoCaptchaExtensions {
        public static IServiceCollection UseNoCaptchaValidator(this IServiceCollection services, string private_key, string public_key = null, NoCaptchaOptions options = null) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            return services
                    .AddSingleton(new NoCaptchaValidator(private_key, public_key))
                    .AddSingleton(options ?? new NoCaptchaOptions())
                    .AddScoped<NoCaptchaRequestValidator>();
        }
    }
}
