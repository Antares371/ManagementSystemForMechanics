using ManagementSystemForMechanics.Interfaces;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.DAL
{
    public class DBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Person> Persons{ get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<VehicleMark> VehiclesMarks { get; set; }
        public DbSet<VehicleModel> VehiclesModels { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleService> VehicleService { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        public event EventHandler SaveChangesEventHandler;
        public event EventHandler SaveChangesAsyncEventHandler;

        public DBContext() : base("name=AutoServisManagementSystem")
        {
            SaveChangesEventHandler += DBContext_SaveChangesEventHandler;
            SaveChangesAsyncEventHandler += DBContext_SaveChangesAsyncEventHandler;
            Configure();
            Initialize();
        }

        private void Configure()
        {
            //Configuration.LazyLoadingEnabled = true;
        }

        private void Initialize()
        {
            Database.SetInitializer(new DBContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(p => p.Name == "Id").Configure(p => p.IsKey());
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            //modelBuilder.Entity<VehicleService>()
            //   .HasMany<Service>(s => s.Services)
            //   .WithMany(c => c.VehicleServices)
            //   .Map(cs =>
            //   {
            //       cs.MapLeftKey("VehicleServiceId");
            //       cs.MapRightKey("ServiceId");
            //       cs.ToTable("rVehicleService_Service");
            //   });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {

            OnSaveChangesEventHandler(EventArgs.Empty);
            return base.SaveChanges();
        }

        public override System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            OnSaveChangesEventHandler(EventArgs.Empty);
            return base.SaveChangesAsync();
        }


        protected void OnSaveChangesEventHandler(EventArgs e)
        {
            if (SaveChangesEventHandler == null)
                return;
            SaveChangesEventHandler(this, e);
        }
        protected void OnSaveChangesAsyncEventHandler(EventArgs e)
        {
            if (SaveChangesAsyncEventHandler == null)
                return;
            SaveChangesAsyncEventHandler(this, e);
        }

        private void DBContext_SaveChangesEventHandler(object sender, EventArgs e)
        {

            UpdateCreatedModifieldTimestamp();
        }
        private void DBContext_SaveChangesAsyncEventHandler(object sender, EventArgs e)
        {
            UpdateCreatedModifieldTimestamp();
        }

        private void UpdateCreatedModifieldTimestamp()
        {
            DateTime now = DateTime.UtcNow;

            List<IEntityModel> selectedEntityList = ChangeTracker.Entries()
                .Where(x => x.Entity is IEntityModel &&
                            (x.State == EntityState.Added || x.State == EntityState.Modified))
                .Select(e => e.Entity)
                .Cast<IEntityModel>()
                .ToList();

            selectedEntityList.ForEach(model =>
            {
                if (model.Created == default(DateTime))
                {
                    model.Created = now;
                }

                model.Modyfied = now;
            });
        }

        public void RollBack()
        {

            List<DbEntityEntry> changedEntries = ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged)
                .ToList();

            foreach (DbEntityEntry entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

    }
}
