using Infrastructure.IntegrationsCore.Data.Schemas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.IntegrationsCore.Entities.Enums;
using Infrastructure.IntegrationsCore.Data.Extensions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Data.Mappings.LinxCommerce
{
    public class B2CConsultaClientesSaldoMap : IEntityTypeConfiguration<B2CConsultaClientesSaldo>
    {
        public void Configure(EntityTypeBuilder<B2CConsultaClientesSaldo> builder)
        {
            var schema = SchemaContext.GetSchema(typeof(B2CConsultaClientesSaldo));

            builder.ToTable("B2CConsultaClientesSaldo");

            if (schema == "linx_microvix_commerce")
            {
                builder.HasKey(e => new { e.cod_cliente_erp, e.empresa });
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

            builder.Property(e => e.saldo)
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.cod_cliente_erp)
                .HasColumnType("int");

            builder.Property(e => e.empresa)
                .HasColumnType("int");

            builder.Property(e => e.timestamp)
                .HasColumnType("bigint");

            builder.Property(e => e.portal)
                .HasColumnType("int");
        }
    }
}
