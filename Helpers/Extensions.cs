using System;
using Microsoft.AspNetCore.Http;

namespace dotnetFun.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationErr(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime now)
        {
            var age = DateTime.Today.Year - now.Year;

            if (now.AddYears(age) > DateTime.Today) {
                age--;
            }

            return age;
        }
    }
}