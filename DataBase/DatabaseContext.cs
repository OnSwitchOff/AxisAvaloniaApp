using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;

namespace DataBase
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base()
        {
            Database.EnsureCreated();
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public bool DataBaseCreated { get; private set; }

        public DbSet<Entities.ApplicationLog.ApplicationLog> ApplicationLogs => Set<Entities.ApplicationLog.ApplicationLog>();
        public DbSet<Entities.Documents.Document> Documents => Set<Entities.Documents.Document>();
        public DbSet<Entities.Exchanges.Exchange> Exchanges => Set<Entities.Exchanges.Exchange>();
        public DbSet<Entities.Items.Item> Items => Set<Entities.Items.Item>();
        public DbSet<Entities.ItemsCodes.ItemCode> ItemsCodes => Set<Entities.ItemsCodes.ItemCode>();
        public DbSet<Entities.ItemsGroups.ItemsGroup> ItemsGroups => Set<Entities.ItemsGroups.ItemsGroup>();
        public DbSet<Entities.OperationDetails.OperationDetail> OperationDetails => Set<Entities.OperationDetails.OperationDetail>();
        public DbSet<Entities.OperationHeader.OperationHeader> OperationHeaders => Set<Entities.OperationHeader.OperationHeader>();
        public DbSet<Entities.Partners.Partner> Partners => Set<Entities.Partners.Partner>();
        public DbSet<Entities.PartnersGroups.PartnersGroup> PartnersGroups => Set<Entities.PartnersGroups.PartnersGroup>();
        public DbSet<Entities.PaymentTypes.PaymentType> PaymentTypes => Set<Entities.PaymentTypes.PaymentType>();
        public DbSet<Entities.Serializations.Serialization> Serializations => Set<Entities.Serializations.Serialization>();
        public DbSet<Entities.Settings.Setting> Settings => Set<Entities.Settings.Setting>();
        public DbSet<Entities.Store.Store> Stores => Set<Entities.Store.Store>();
        public DbSet<Entities.VATGroups.VATGroup> Vatgroups => Set<Entities.VATGroups.VATGroup>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                foreach (var option in optionsBuilder.Options.Extensions)
                {
                    if (option is SqliteOptionsExtension optionsExtension && !string.IsNullOrEmpty(optionsExtension.ConnectionString))
                    {
                        var connectionStringBuilder = new SqliteConnectionStringBuilder()
                        {
                            DataSource = optionsExtension.ConnectionString,
                        };

                        optionsBuilder.UseSqlite(connectionStringBuilder.ToString());
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
