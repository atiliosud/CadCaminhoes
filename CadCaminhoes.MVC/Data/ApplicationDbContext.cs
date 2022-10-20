using CadCaminhoes.MVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CadCaminhoes.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Caminhao>()
                .HasOne(b => b.CreateBy)
                .WithMany()
                .HasForeignKey(ub => ub.CreateById)
                .OnDelete(DeleteBehavior.ClientNoAction);

            base.OnModelCreating(builder);
        }

        public DbSet<Caminhao> Caminhoes { get; set; }
    }
}