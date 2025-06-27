using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.V2.AtmJecContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oi.Juridico.Contextos.V2.AtmJecContext.Data.Configurations
{
    public partial class AgendFechAtmJecConfiguration
    {
        partial void OnConfigurePartial(EntityTypeBuilder<AgendFechAtmJec> entity)
        {
            entity.Property(x => x.AfajId)
               .ValueGeneratedOnAdd();
        }
    }

    public partial class FechamentoAtmJecConfiguration
    {
        partial void OnConfigurePartial(EntityTypeBuilder<FechamentoAtmJec> entity)
        {
            entity.Property(x => x.FajId)
                .ValueGeneratedOnAdd();
        }
    }
}
