﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using shtik.Actions;
using Shtik.Rendering.Markdown;

namespace shtik
{
    public static class Routes
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("shtik.Properties.Resources", typeof(Routes).Assembly);

        public static void Router(IRouteBuilder routes)
        {
            routes.MapGet("normalize.css", (request, response, data) =>
            {
                response.ContentType = "text/css; charset=utf-8";
                return response.Body.WriteAsync(Embedded.Web.normalize_css);
            });

            routes.MapGet("theme.css", (request, response, data) =>
            {
                response.ContentType = "text/css; charset=utf-8";
                return response.Body.WriteAsync(Embedded.Web.theme_css);
            });

            routes.MapGet("shtik.js", (request, response, data) =>
            {
                response.ContentType = "text/javascript; charset=utf-8";
                return response.Body.WriteAsync(Embedded.Web.shtik_js);
            });

            routes.MapGet("fonts/{font}", (request, response, data) =>
            {
                if (data.Values.TryGetString("font", out string font))
                {
                    var bytes = GetFont(font);
                    if (bytes == ArraySegment<byte>.Empty)
                    {
                        response.StatusCode = 404;
                        return Task.CompletedTask;
                    }
                    response.ContentType = font.EndsWith(".woff") ? "application/font-woff" : "font/woff2";
                    return response.Body.WriteAsync(bytes);
                }
                response.StatusCode = 404;
                return Task.CompletedTask;
            });

            routes.MapGet("{index}", new GetSlideAction(routes.ServiceProvider).Invoke);
        }

        private static ArraySegment<byte> GetFont(string name)
        {
            switch (name)
            {
                case "fira-sans-v7-latin-700.woff":
                    return Embedded.Web.fonts_fira_sans_v7_latin_700_woff;
                case "fira-sans-v7-latin-700.woff2":
                    return Embedded.Web.fonts_fira_sans_v7_latin_700_woff2;
                case "fira-sans-v7-latin-700italic.woff":
                    return Embedded.Web.fonts_fira_sans_v7_latin_700italic_woff;
                case "fira-sans-v7-latin-700italic.woff2":
                    return Embedded.Web.fonts_fira_sans_v7_latin_700italic_woff2;
                case "lato-v13-latin-regular.woff":
                    return Embedded.Web.fonts_lato_v13_latin_regular_woff;
                case "lato-v13-latin-regular.woff2":
                    return Embedded.Web.fonts_lato_v13_latin_regular_woff2;
                case "lato-v13-latin-italic.woff":
                    return Embedded.Web.fonts_lato_v13_latin_italic_woff;
                case "lato-v13-latin-italic.woff2":
                    return Embedded.Web.fonts_lato_v13_latin_italic_woff2;
                default:
                    return ArraySegment<byte>.Empty;
            }
        }
    }
}
