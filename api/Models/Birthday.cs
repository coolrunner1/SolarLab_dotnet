using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Birthday
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateOnly Date { get; set; }
        [AllowNull]
        public string FileName { get; set; }
        [AllowNull]
        public byte[] FileData { get; set; }
    }
}