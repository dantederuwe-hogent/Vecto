using System;
using Vecto.Core.Entities;

namespace Vecto.Application.Sections
{
    public static class SectionMapper
    {
        public static Section MapToSection(this SectionDTO dto)
        {
            return dto.SectionType switch
            {
                nameof(PackingSection) => new PackingSection() { Name = dto.Name },
                nameof(TodoSection) => new TodoSection() { Name = dto.Name },
                _ => throw new ArgumentException("Section type is not valid", nameof(dto.SectionType))
            };
        }
    }
}
