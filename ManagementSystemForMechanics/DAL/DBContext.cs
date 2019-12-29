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

        public DbSet<VehicleMark> VehiclesMarks { get; set; }
        public DbSet<MotorType> FuelTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public event EventHandler SaveChangesEventHandler;
        public event EventHandler SaveChangesAsyncEventHandler;

        public DBContext() : base("AutoServisManagementSystem")
        {
            SaveChangesEventHandler += DBContext_SaveChangesEventHandler;
            SaveChangesAsyncEventHandler += DBContext_SaveChangesAsyncEventHandler;
            Configure();
            Initialize();
        }

        private void Configure()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        private void Initialize()
        {
            Database.SetInitializer(new DBContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(p => p.Name == "ID").Configure(p => p.IsKey());
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));



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


    }
}
