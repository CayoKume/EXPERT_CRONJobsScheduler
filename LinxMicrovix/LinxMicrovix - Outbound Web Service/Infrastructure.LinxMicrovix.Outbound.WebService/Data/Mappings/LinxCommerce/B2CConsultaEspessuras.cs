using Infrastructure.IntegrationsCore.Data.Schemas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.IntegrationsCore.Entities.Enums;
using Infrastructure.IntegrationsCore.Data.Extensions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Data.Mappings.LinxCommerce
{
    public class B2CConsultaEspessurasMap : IEntityTypeConfiguration<B2CConsultaEspessuras>
    {
        public void Configure(EntityTypeBuilder<B2CConsultaEspessuras> builder)
        {
            var schema = SchemaContext.GetSchema(typeof(B2CConsultaEspessuras));

            builder.ToTable("B2CConsultaEspessuras");

            if (schema == "linx_microvix_commerce")
            {
                builder.HasKey(e => e.codigo_espessura);
                builder.Ignore(e => e.id);
            }
            else
            {
                builder.HasKey(e => e.id);

                builder.Property(e => e.id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();
            }

            builder.Property(e => e.lastupdateon)
                .HasProviderColumnType(EnumTableColumnType.DateTime);

            builder.Property(e => e.codigo_espessura)
                .HasColumnType("int");

            builder.Property(e => e.nome_espessura)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.timestamp)
                .HasColumnType("bigint");

            builder.Property(e => e.portal)
                .HasColumnType("int");
        }
    }
}
