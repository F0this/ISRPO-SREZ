using System.Linq;
using System.Text.Json.Serialization;

namespace Library.Models
{
    public partial class Reader
    {
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = null;
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = null;
        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; } = null;
        [JsonPropertyName("photo")]
        public byte[] Photo { get; set; } = null;
    }

    public partial class Reader
    {
        public string Initials
        {
            get
            {
                return $"{this.LastName} {this.FirstName.First()}.{this.MiddleName.FirstOrDefault()}.";
            }
        }
        public string FullName
        {
            get
            {
                return $"{this.LastName} {this.FirstName}.{this.MiddleName}.";
            }
        }
    }
}