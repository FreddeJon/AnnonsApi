using System.Reflection;
using Api.Persistence;
using IAuthenticationService = Api.Services.IAuthenticationService;

namespace Api;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddNewtonsoftJson();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "RestAPI"

            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });

        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
        );
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;

            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = builder.Configuration["JwtOptions:ValidAudience"],
                    ValidIssuer = builder.Configuration["JwtOptions:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]))
                };
            });

        builder.Services.AddScoped<IAuthenticationService, Services.AuthenticationService>();


        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    }
}