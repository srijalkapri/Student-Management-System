namespace CRUD.Application.Optionss
{
    public class CorsSettings
    {
        public const string SectionName = "Cors";

        /// <summary>
        /// Frontend origins allowed to call the API (e.g. https://your-app.vercel.app).
        /// Render/env options:
        /// - Cors__AllowedOrigins__0=https://app.vercel.app
        /// - Cors__AllowedOrigins=https://app.vercel.app,http://localhost:5173
        /// </summary>
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    }
}
