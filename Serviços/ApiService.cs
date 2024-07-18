namespace Cambios.Servicos
{

    using Cambios.Modelos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService
    {
        public async Task<Response> GetRates(string urlBase, string controller)
        {
            try
            {
                // Criar um cliente comconexão via http
                var client = new HttpClient();

                //Atribuir a API
                client.BaseAddress = new Uri(urlBase);

                //Mandar o controlador 
                var response = await client.GetAsync(controller);

                //Carrego os formatos em string para a variavel/objeto "result"
                var result = await response.Content.ReadAsStringAsync();

                //Verificar se algo correu mal
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                //Converto o Json em uma lista da classe "Rates" passando o que está dentro da variavel "result", corrigindo o bug da microsoft
                var rates = JsonConvert.DeserializeObject<List<Rates>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = rates
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}