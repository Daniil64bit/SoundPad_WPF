using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundPad_WPF
{
    [Table("SoundPad_DB")]
    public class SoundDB
    {
        [Key]
        public int Sound_ID { get; set; }
        public string Sound_Link { get; set; }
        public string Sound_Key { get; set; }

        public SoundDB() { }

        public SoundDB(string Sound_Link, string Sound_Key, int Sound_ID) 
        {
            this.Sound_ID = Sound_ID;
            this.Sound_Link = Sound_Link;
            this.Sound_Key = Sound_Key;
        }
    }
}
