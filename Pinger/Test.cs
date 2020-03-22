using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Pinger
{
    class Test
    {

        private readonly IConfiguration configuration;
        private readonly UriBuilder uriBuilder;

        public Test(IConfiguration configuration, UriBuilder uriBuilder)
        {
            this.configuration = configuration;
            this.uriBuilder = uriBuilder;
        }

        public void Show()
        {
            Console.WriteLine(uriBuilder.Uri);
        }
    }
}
