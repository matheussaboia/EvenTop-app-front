using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventop.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace Eventop.Services
{
    public class MockDataStore : IDataStore<Usuario>
    {
        List<Usuario> items;

        public MockDataStore()
        {
            List<Usuario> resultado = new List<Usuario>();
            items = new List<Usuario>();

            HttpClient client = new HttpClient{
                    BaseAddress = new Uri("http://eventop.azurewebsites.net/api/Usuarios")
                };

                var request = client.GetAsync("/api/Usuarios").Result;
                if (request.IsSuccessStatusCode){
                    var responseJson = request.Content.ReadAsStringAsync().Result;
                    resultado = JsonConvert.DeserializeObject<List<Usuario>>(responseJson);
                }

            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in resultado)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Usuario item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Usuario item)
        {
            var oldItem = items.Where((Usuario arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((Usuario arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Usuario> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Usuario>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}