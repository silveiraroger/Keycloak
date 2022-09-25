using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ClienteApi
{
    public class HavePermissionAsync : Attribute, IAsyncAuthorizationFilter
    {
        private string _processo { get; }

        public HavePermissionAsync(string processo)
        {
            _processo = processo;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var permissoes = await ObterPermissoes();
            if (!permissoes.attributes.permissoes.Contains(_processo))
            {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new JsonResult($"Você não tem acesso ao processo {_processo}");
            }
        }

        private async Task<ResponseToken> ObterTokenAdmin()
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "admin-cli"),
                new KeyValuePair<string, string>("username", "admin"),
                new KeyValuePair<string, string>("password", "Pa55w0rd"),
                new KeyValuePair<string, string>("grant_type", "password")
            };

            var content = new FormUrlEncodedContent(data);

            var client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync("http://localhost:8080/auth/realms/master/protocol/openid-connect/token", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ResponseToken>(responseBody);
        }

        private async Task<UserInfo> ObterPermissoes()
        {
            try
            {
                var token = await ObterTokenAdmin();

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");
                HttpResponseMessage response = await client.GetAsync("http://localhost:8080/auth/admin/realms/bem-realm/users/c9379b36-61b6-48ab-b4c6-80d1df3bfb2b");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<UserInfo>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);

                return null;
            }
        }
    }
}
