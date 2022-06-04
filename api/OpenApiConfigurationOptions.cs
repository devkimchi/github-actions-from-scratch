using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Swm.ApiApp
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
        public override bool IncludeRequestingHostName { get; set; } = false;
    }
}