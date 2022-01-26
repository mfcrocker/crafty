using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CraftyPlugin.API
{
    public class UniversalisItemData
    {
        public int[]? itemIDs { get; set; }
        public UniversalisItem[]? items { get; set; }
        public int worldID { get; set; }
        public string? dcName { get; set; }
        public int[]? unresolvedItems { get; set; }
        public string? worldName { get; set; }
    }

    public class UniversalisItem
    {
        public int itemID { get; set; }
        public int worldID { get; set; }
        public int lastUploadTime { get; set; }
        public UniversalisListing[]? listings { get; set; }
        public UniversalisListingHistory[]? recentHistory { get; set; }
        public string? dcName { get; set; }
        public int currentAveragePrice { get; set; }
        public int currentAveragePriceNQ { get; set; }
        public int currentAveragePriceHQ { get; set; }
        public int regularSaleVelocity { get; set; }
        public int nqSaleVelocity { get; set; }
        public int hqSaleVelocity { get; set; }
        public int averagePrice { get; set; }
        public int averagePriceNQ { get; set; }
        public int averagePriceHQ { get; set; }
        public int minPrice { get; set; }
        public int minPriceNQ { get; set; }
        public int minPriceHQ { get; set; }
        public int maxPrice { get; set; }
        public int maxPriceNQ { get; set; }
        public int maxPriceHQ { get; set; }
        public string? worldName { get; set; }
    }

    public class UniversalisListing
    {
        public int lastReviewTime { get; set; }
        public int pricePerUnit { get; set; }
        public int quantity { get; set; }
        public int stainID { get; set; }
        public string? worldName { get; set; }
        public int worldID { get; set; }
        public string? creatorName { get; set; }
        public string? creatorID { get; set; }
        public bool hq { get; set; }
        public bool isCrafted { get; set; }
        public string? listingID { get; set; }
        public bool onMannequin { get; set; }
        public int retainerCity { get; set; }
        public string? retainerID { get; set; }
        public string? retainerName { get; set; }
        public string? sellerID { get; set; }
        public int total { get; set; }
    }

    public class UniversalisListingHistory
    {
        public bool hq { get; set; }
        public int pricePerUnit { get; set; }
        public int quantity { get; set; }
        public int timestamp { get; set; }
        public string? worldName { get; set; }
        public int worldID { get; set; }
        public string? buyerName { get; set; }
        public int total { get; set; }
    }

    public class Universalis
    {
        private const string getURL = "https://universalis.app/api/";
        private string world;
        private string dataCenter;

        public Universalis(string world, string dataCenter)
        {
            this.world = world;
            this.dataCenter = dataCenter;
        }

        public async Task<UniversalisItemData> GetItemFromDataCenter(int itemID)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(getURL + dataCenter + "/" + itemID.ToString());

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            using (StreamReader streamReader = new StreamReader(stream))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    UniversalisItemData itemData = jsonSerializer.Deserialize<UniversalisItemData>(jsonReader);
                    return itemData;
                }
            }
        }
    }
}