

namespace CRUD.Application.Optionss
{
    public class AccessLogOptions
    {

        public bool Enabled { get; set; } = true;
        public string[] ExcludedPaths { get; set; } = new[]  { "/swagger", "/health" };
    }
}
