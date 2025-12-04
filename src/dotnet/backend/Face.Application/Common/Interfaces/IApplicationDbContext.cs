using System.Threading;
using System.Threading.Tasks;
using Face.Domain.Frames;
using Face.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Face.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Frame> Frames { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
