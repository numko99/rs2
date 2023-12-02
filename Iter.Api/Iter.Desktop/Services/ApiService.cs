using Flurl;
using Flurl.Http;
using Iter.Desktop.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Policy;

namespace Iter.UI.Desktop.Services
{
    public class ApiService
    {
        private string _route = null;
        public static string Token { get; set; }

        public ApiService(string route)
        {
            _route = route;
        }

        private bool HandleError(FlurlHttpException ex)
        {
            if (ex.Call.Response.StatusCode == 401)
            {
                MessageBox.Show("Authentication failed.");
                return true;
            }
            else if (ex.Call.Response.StatusCode == 403)
            {
                MessageBox.Show("You do not have permission to access this resource.");
                return true;
            }
            else if (ex.Call.Response.StatusCode == 500)
            {
                MessageBox.Show("Something went wrong");
                return true;
            }
            return false;
        }

        public async Task<T> Get<T>() where T : class
        {
            try
            {
                var result = await $"{Settings.Default.APIUrl}/{_route}"
                .WithOAuthBearerToken(Token)
                .GetJsonAsync<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                HandleError(ex);
                return default(T);
            }
        }

        public async Task<T> GetSearch<T>(string? filter) where T : class
        {
            try
            {
                var result = await $"{Settings.Default.APIUrl}/{_route}?searchTerm={filter}"
                    .SetQueryParam(filter)
                    .WithOAuthBearerToken(Token)
                    .GetJsonAsync<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                HandleError(ex);
                return default(T);
            }
        }


        public async Task<T> Get<T>(string url) where T : class
        {
            try
            {
                var result = await $"{Settings.Default.APIUrl}/{_route}/{url}"
                .WithOAuthBearerToken(Token)
                .GetJsonAsync<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                HandleError(ex);
                return default(T);
            }
        }

        public async Task<TResponse> Get<TResponse>(string url, Dictionary<string, object?>? queryParams = null)
        {
            try
            {
                var apiUrl = $"{Settings.Default.APIUrl}/{_route}/{url}";

                if (queryParams != null && queryParams.Count > 0)
                {
                   apiUrl = apiUrl.SetQueryParams(queryParams);
                }
                var result = await apiUrl
                    .WithOAuthBearerToken(Token)
                    .GetJsonAsync<TResponse>();

                return result;
            }
            catch (FlurlHttpException ex)
            {
                throw;
            }
        }

        public async Task<T> Login<T>(object request) where T : class
        {
            try
            {
                var result = await $"{Settings.Default.APIUrl}/{_route}"
                .PostJsonAsync(request).ReceiveJson<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                throw;
            }
        }

        public async Task<T> Insert<T>(string url, object request) where T : class
        {
            try
            {
                var result = await $"{Settings.Default.APIUrl}/{_route}/{url}"
                 .WithOAuthBearerToken(Token)
                .PostJsonAsync(request).ReceiveJson<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                HandleError(ex);
                return default(T);
            }
        }

        public async Task<T> Update<T>(object request, object id = null) where T : class
        {
            try
            {
                var result = await $"{Settings.Default.APIUrl}/{_route}/update?id={id}"
                    .WithOAuthBearerToken(Token)
                    .PutJsonAsync(request).ReceiveJson<T>();

                return result;
            }
            catch (FlurlHttpException ex)
            {
                HandleError(ex);
                return default(T);
            }
        }

        public async Task<T> GetById<T>(object id) where T : class
        {
            try
            {
                var result = await $"{Settings.Default.APIUrl}/{_route}/details?id={id}"
                         .WithOAuthBearerToken(Token).GetJsonAsync<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                HandleError(ex);
                return default(T);
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                await $"{Settings.Default.APIUrl}/{_route}/delete?id={id}"
                         .WithOAuthBearerToken(Token).DeleteAsync();
            }
            catch (FlurlHttpException ex)
            {
                HandleError(ex);
            }
        }
    }
}