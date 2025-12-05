using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using Face.Domain.Common;
using Face.Domain.Frames;
using Face.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Face.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IMediator _mediator;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Frame> Frames => Set<Frame>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Make sure EF Core completely ignores domain events
            builder.Ignore<DomainEvent>();
            builder.Ignore<IDomainEvent>();

            builder.Entity<Frame>(b =>
            {
                b.HasKey(f => f.Id);

                b.OwnsOne(f => f.FrameId, fb =>
                {
                    fb.Property(p => p.Value)
                        .HasColumnName("FrameId")
                        .IsRequired()
                        .HasMaxLength(128);
                });

                b.Property(f => f.TimestampUtc)
                    .IsRequired();

                b.Property(f => f.Source)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);

                b.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(64);

                b.HasIndex(u => u.UserName)
                    .IsUnique();

                b.HasData(
                    new User
                    {
                        Id = 1,
                        UserName = "admin",
                        PasswordHash =
                            "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                        Role = "Admin"
                    },
                    new User
                    {
                        Id = 2,
                        UserName = "user",
                        PasswordHash =
                            "04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb",
                        Role = "User"
                    }
                );
            });
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchDomainEventsAsync(cancellationToken);
            return result;
        }

        private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
        {
            var entitiesWithEvents = ChangeTracker
                .Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
            }
        }
    }
}
