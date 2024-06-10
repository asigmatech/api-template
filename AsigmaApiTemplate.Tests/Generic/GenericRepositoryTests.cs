using Microsoft.EntityFrameworkCore;
using AsigmaApiTemplate.Api.Data;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.Repositories.GenericRepositories;


namespace AsigmaApiTemplate.Tests.Generic
{
    public class GenericRepositoryTests
    {

        private async Task<ApplicationDbContext> GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            // Add test data to the context
            dbContext.AddRange(
              new WeatherForecast
              {
                  Date = DateOnly.FromDateTime(DateTime.Now),
                  TemperatureC = 20,
                  Summary = "Sunny",
              }
            );

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            using var dbContext = await GetDbContextWithData();
            var repository = new GenericRepository<WeatherForecast>(dbContext);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GenericRepository_GetByIdAsync_ReturnsEntityById()
        {
            // Arrange
            using var dbContext = await GetDbContextWithData();
            var repository = new GenericRepository<WeatherForecast>(dbContext);
            var firstWeatherForecastId = (await dbContext.WeatherForecasts.FirstAsync()).Id;

            // Act
            var result = await repository.GetByIdAsync(firstWeatherForecastId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(firstWeatherForecastId, result.Id);
        }

        [Fact]
        public async Task GenericRepository_InsertAsync_AddsNewEntity()
        {
            // Arrange
            using var dbContext = await GetDbContextWithData();
            var repository = new GenericRepository<WeatherForecast>(dbContext);
            var newWeatherForecast = new WeatherForecast
            {
                Id = Guid.NewGuid(),
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 20,
                Summary = "Sunny",
            };

            // Act
            await repository.InsertAsync(newWeatherForecast);

            // Assert
            var addedWeatherForecast = await dbContext.WeatherForecasts.FindAsync(newWeatherForecast.Id);
            Assert.NotNull(addedWeatherForecast);
            Assert.Equal(newWeatherForecast.Date, addedWeatherForecast.Date);
        }

        [Fact]
        public async Task GenericRepository_UpdateAsync_UpdatesEntity()
        {
            // Arrange
            using var dbContext = await GetDbContextWithData();
            var repository = new GenericRepository<WeatherForecast>(dbContext);
            var firstWeatherForecast = await dbContext.WeatherForecasts.FirstAsync();
            firstWeatherForecast.Summary = "Sunny Updated";

            // Act
            await repository.UpdateAsync(firstWeatherForecast);

            // Assert
            var updatedWeatherForecast = await dbContext.WeatherForecasts.FindAsync(firstWeatherForecast.Id);
            Assert.NotNull(updatedWeatherForecast);
            Assert.Equal("Sunny Updated", updatedWeatherForecast.Summary);
        }

        [Fact]
        public async Task GenericRepository_DeleteAsync_DeletesEntity()
        {
            // Arrange
            using var dbContext = await GetDbContextWithData();
            var repository = new GenericRepository<WeatherForecast>(dbContext);
            var firstWeatherForecast = await dbContext.WeatherForecasts.FirstAsync();

            // Act
            await repository.DeleteAsync(firstWeatherForecast.Id);

            // Assert
            var deletedWeatherForecast = await dbContext.WeatherForecasts.FirstOrDefaultAsync(c => c.Id == firstWeatherForecast.Id);
            Assert.NotNull(deletedWeatherForecast);
            Assert.True(deletedWeatherForecast.IsDeleted);
        }
    }
}
