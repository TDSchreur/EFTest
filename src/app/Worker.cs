using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App
{
    public class Worker : BackgroundService
    {
        private readonly DataContext _dataContext;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IHostApplicationLifetime hostApplicationLifetime,
            DataContext dataContext,
            ILogger<Worker> logger)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _dataContext = dataContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await _dataContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.Persons", stoppingToken).ConfigureAwait(false);

            Person dennis = new()
                            {
                                FirstName = "Dennis", LastName = "Schreur",
                            };
            Person tess = new()
                          {
                              FirstName = "Tess", LastName = "Schreur",
                          };
            Person daan = new()
                          {
                              FirstName = "Daan", LastName = "Schreur",
                          };

            IDbContextTransaction transaction = await _dataContext.BeginTransaction().ConfigureAwait(false);

            _dataContext.Persons.Add(dennis);

            await transaction.CreateSavepointAsync("a", stoppingToken).ConfigureAwait(false);

            _dataContext.Persons.Add(tess);

            await transaction.RollbackToSavepointAsync("a", stoppingToken).ConfigureAwait(false);

            _dataContext.Persons.Add(daan);

            await _dataContext.SaveChangesAsync(stoppingToken).ConfigureAwait(false);

            await _dataContext.CommitTransaction(transaction).ConfigureAwait(false);

            _hostApplicationLifetime.StopApplication();
        }
    }
}
