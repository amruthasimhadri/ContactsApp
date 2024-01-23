using Newtonsoft.Json;
namespace ContactsApp.Models
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _configuration;

        public ApiService(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrl = _configuration["baseurl"];


        }

        private string BuildApiUrl(string endpoint)
        {
            return $"{_baseUrl}/api/Contact/{endpoint}";
        }
        public bool FindUser(string username)
        {
            string apiUrl = BuildApiUrl($"FindUser?username={username}");
            var response = _httpClient.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<bool>().Result;
            }
            return false;
        }

        public bool TestPassword(LoginModel user)
        {
            string apiUrl = BuildApiUrl("TestPassword");
            var response = _httpClient.PostAsJsonAsync(apiUrl, user).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<bool>().Result;
            }
            return false;
        }

        public int GetUserId(string username)
        {
            string apiUrl = BuildApiUrl($"GetUserId?username={username}");
            var response = _httpClient.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<int>().Result;
            }
            return -1;
        }

        public List<ContactList> GetContacts(int userId)
        {
            string apiUrl = BuildApiUrl($"GetContacts?userId={userId}");
            var response = _httpClient.GetAsync(apiUrl).Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            List<ContactList> contacts = JsonConvert.DeserializeObject<List<ContactList>>(jsonContent);
            return contacts;
        }

        public Contact GetContactModel()
        {
            string apiUrl = BuildApiUrl("GetContactModel");
            var response = _httpClient.GetAsync(apiUrl).Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            Contact contact = JsonConvert.DeserializeObject<Contact>(jsonContent);
            return contact;
        }

        public void AddContact(AddContact contact)
        {
            string apiUrl = BuildApiUrl("AddContact");
            var response = _httpClient.PostAsJsonAsync(apiUrl, contact).Result;
        }
        public void EditContact(Contact contact)
        {
            string apiUrl = BuildApiUrl("EditContact");
            var response = _httpClient.PostAsJsonAsync(apiUrl, contact).Result;
        }

        public Contact GetEditContactModel(int Id)
        {
            string apiUrl = BuildApiUrl($"GetEditContactModel?Id={Id}");
            var response = _httpClient.GetAsync(apiUrl).Result;
            response.EnsureSuccessStatusCode();
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            Contact contact = JsonConvert.DeserializeObject<Contact>(jsonContent);
            return contact;
        }
        public void DeleteContact(int Id)
        {
            string apiUrl = BuildApiUrl($"DeleteContact?Id={Id}");
            var response = _httpClient.GetAsync(apiUrl).Result;
            
        }
        //public void ImportFromExcel(MultipartFormDataContent content)
        //{

        //    string apiUrl = BuildApiUrl("ImportFromExcel");
        //    var response = _httpClient.PostAsJsonAsync(apiUrl, content).Result;


        //}
        public async Task ImportFromExcel(IFormFile file)
        {
            try
            {
                string apiUrl = BuildApiUrl("ImportFromExcel");
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                    using (var request = new HttpRequestMessage(HttpMethod.Post, apiUrl))
                    {
                        request.Content = content;

                        using (var response = await _httpClient.SendAsync(request))
                        {
                            // Handle the response as needed
                            // e.g., check if it was successful, handle errors, etc.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }
        public void ExportToExcel(int userId)
        {
            try
            {
                string apiUrl = BuildApiUrl($"ExportToExcel?userId={userId}");
                var response = _httpClient.GetAsync(apiUrl).Result;
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }



    }
}
                                                                                                                                                                                                               