namespace CRUD.Application.Optionss
{
    public class CorsSettings
    {
        public const string SectionName = "Cors";

        /// <summary>
        /// Frontend origins allowed to call the API (e.g. https://your-app.vercel.app).
        /// Set via env: Cors__AllowedOrigins__0, Cors__AllowedOrigins__1, ...
        /// </summary>
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    }
}
