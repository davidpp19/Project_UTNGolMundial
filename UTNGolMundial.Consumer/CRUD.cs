using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTNGolMundial.Consumer
{
    public static class Crud<T> //La <T> indica que esta clase es genérica, lo que significa que puede trabajar con
                                //cualquier tipo de datos. Al usar <T>, se puede crear una instancia de la clase Crud para
                                //cualquier tipo de entidad (por ejemplo, Crud<Empleado>, Crud<Persona>, etc.) sin necesidad de
                                //escribir código específico para cada tipo. Esto hace que la clase sea más flexible y reutilizable,
                                //ya que puede manejar diferentes tipos de datos sin duplicar el código.
    {
        public static string Endpoint { get; set; } //Propiedad estática que representa el punto de acceso (endpoint) para las operaciones CRUD. 
                                                    //Al ser estática, esta propiedad es compartida por todas las instancias de la clase Crud<T>, lo que significa que se puede configurar una única URL de endpoint para todas las operaciones CRUD realizadas a través de esta clase.
        public static async Task<T> Create(T data)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
                    var contentBody = new StringContent(
                        Newtonsoft.Json.JsonConvert.SerializeObject(data), //Convierte el objeto 'data' a una cadena JSON utilizando la biblioteca Newtonsoft.Json. Esto es necesario para enviar los datos en el formato adecuado al servidor a través de la solicitud HTTP POST.
                        Encoding.UTF8, "application/json"
                    );
                    request.Content = contentBody;

                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                    );

                    var response = httpClient.Send(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static T ReadById(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{Endpoint}/{id}");
                var response = httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static List<T> ReadAll()
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, Endpoint);
                var response = httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static bool Update(string id, T data)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, $"{Endpoint}/{id}");
                var contentBody = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(data),
                    Encoding.UTF8, "application/json"
                );
                request.Content = contentBody;
                var response = httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static bool Delete(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/{id}");
                var response = httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }


            }
        }
    }
}
