
using BookStore.Models;
using BookStore.Models.Repositories;

using Microsoft.EntityFrameworkCore;

namespace BookStore
{
  public class Startup
  {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
          this.configuration = configuration;
        }
      public void ConfigureServices(IServiceCollection services)
      {
        services.AddMvc(option=>option.EnableEndpointRouting=false);
        services.AddSingleton<IBookstoreRepository<Author>, AuthorRepository>();
        services.AddSingleton<IBookstoreRepository<Book>, BookRepository>();
        services.AddDbContext<BookstoreDbContext>(options =>
        {
          options.UseSqlServer(configuration.GetConnectionString("SqlCon"));
        });
      }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvcWithDefaultRoute();
      app.UseStaticFiles();

    }
  }
}