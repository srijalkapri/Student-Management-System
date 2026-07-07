

namespace CRUD.Domain.Models
{
    public class AcessLog
    {
        public long Id { get; set; }
        public int ? UserId { get; set; }

        public string ? UserName { get ; set; }

        public string ApiPath { get; set; } = default!;
        
        public string HttpMethod { get; set; } = default!;

        public int StatusCode { get; set; }

        public DateTime CreatedAtUtc { get; set; } 


    }
}
