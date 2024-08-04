using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Birthday
{
    public class UpdateBirthdayRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        [AllowNull]
        public string FileName { get; set; }
        [AllowNull]
        public byte[] FileData { get; set; }
    }
}