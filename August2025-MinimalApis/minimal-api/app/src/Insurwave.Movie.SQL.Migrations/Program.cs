using Insurwave.Extensions.Postgres.Extensions;
using Insurwave.Movie.Persistence.Postgres;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetSection("ConnectionString");
builder.Services.UsePostgres<MovieDbContext>(connectionString.Value);
var app = builder.Build();

app.Run();
