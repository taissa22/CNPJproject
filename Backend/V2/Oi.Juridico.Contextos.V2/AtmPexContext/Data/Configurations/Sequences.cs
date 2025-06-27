using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.AtmPexContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oi.Juridico.Contextos.V2.AtmPexContext.Data.Configurations
{
    public partial class AgendFechAtmPexConfiguration
    {
        partial void OnConfigurePartial(EntityTypeBuilder<AgendFechAtmPex> entity)
        {
            entity.Property(x => x.Id)
               .ValueGeneratedOnAdd();
        }
    }

    public partial class AgendFechAtmPexUfConfiguration
    {
        partial void OnConfigurePartial(EntityTypeBuilder<AgendFechAtmPexUf> entity)
        {
            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
