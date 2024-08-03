using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGame.DAL;

public class AppDbContext: DbContext
{

    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}
