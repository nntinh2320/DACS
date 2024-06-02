using DACS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DACS.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<CTCacChat> CTCacChats { get; set; }
    public DbSet<DongSong> DongSongs { get; set; }
    public DbSet<PhieuLayMau> PhieuLayMaus { get; set; }
    public DbSet<ViTriLayMau> ViTriLayMaus { get; set; }
    public DbSet<ApplicationUser> User { get; set; }
}
