using Infrastructure.IntegrationsCore.Data.Schemas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.IntegrationsCore.Entities.Enums;
using Infrastructure.IntegrationsCore.Data.Extensions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Data.Mappings.LinxCommerce
{
    public class B2CConsultaFlagsMap : IEntityTypeConfiguration<B2CConsultaFlags>
    {
        public void Configure(EntityTypeBuilder<B2CConsultaFlags> builder)
        {
            var schema = SchemaContext.GetSchema(typeof(B2CConsultaFlags));

            builder.ToTable("B2CConsultaFlags");

            if (schema == "linx_microvix_commerce")
            {
                builder.HasKey(e => e.id_b2c_flags);
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

            builder.Property(e => e.portal)
                .HasColumnType("int");

            builder.Property(e => e.id_b2c_flags)
                .HasColumnType("int");

            builder.Property(e => e.descricao)
                .HasColumnType("varchar(300)");

            builder.Property(e => e.timestamp)
                .HasColumnType("bigint");
        }
    }
}
