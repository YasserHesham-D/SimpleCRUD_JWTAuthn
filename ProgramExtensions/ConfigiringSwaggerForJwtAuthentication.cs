using Microsoft.OpenApi.Models;

namespace SimpleCRUD_JWTAuthn.ProgramExtensions
{
    public static class ConfigiringSwaggerForJwtAuthentication
    {
        public static void AddSwaggerGenJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                           
               // o.SwaggerDoc("v1", new OpenApiInfo()
              //  {
                 //   Version = "v1",
                 //   Title = "test api",
                 //   Description = "adasdsad",
                 //   Contact = new OpenApiContact()
                  //  {
                      //  Name = "al Mohamady",
                      //  Email = "ahmed@gmail.com",
                      //  Url = new Uri("https://mydomain.com")
                  //  }
              //  });

                // this is to add auth configuration to swagger
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT Key"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                       new OpenApiSecurityScheme()
                       {
                          Reference = new OpenApiReference()
                          {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                          },
                          Name = "Bearer",
                          In = ParameterLocation.Header
                       },
                       new List<string>()
                    }
                });
            });
        }
    }
}
