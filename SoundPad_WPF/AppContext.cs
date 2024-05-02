using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SoundPad_WPF
{
    public class AppContext : DbContext
    {
        public DbSet<SoundDB> SoundDBs { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
