using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingMusic.Shared.Models
{
    public class Music
    {
        public int MusicId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Artist { get; set; }

        public string Duration { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LaunchDate { get; set; }

        [StringLength(255)]
        public string RecordLabel { get; set; }
        // en base a esto necesito una tabla en sql server
    }
}
