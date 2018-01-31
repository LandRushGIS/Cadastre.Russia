using Xunit;

namespace LandRush.Cadastre.Russia.Database.Tests
{
	public class MigrationTests
	{
		[Fact]
		public void Up_ToLatestState_Complete()
		{
			var connectionString = "Server=localhost;Database=am_test"; //User Id=***;Password=***
			var announcer = new FluentMigrator.Runner.Announcers.NullAnnouncer();
			var context = new FluentMigrator.Runner.Initialization.RunnerContext(announcer);
			var options = new FluentMigrator.Runner.Processors.ProcessorOptions { PreviewOnly = false, Timeout = 60 };
			var processorFactory = new FluentMigrator.Runner.Processors.Postgres.PostgresProcessorFactory();
			using (var processor = processorFactory.Create(connectionString, announcer, options))
			{
				var runner = new FluentMigrator.Runner.MigrationRunner(typeof(Naming).Assembly, context, processor);
				runner.MigrateUp(useAutomaticTransactionManagement: true);
			}
		}
	}
}
