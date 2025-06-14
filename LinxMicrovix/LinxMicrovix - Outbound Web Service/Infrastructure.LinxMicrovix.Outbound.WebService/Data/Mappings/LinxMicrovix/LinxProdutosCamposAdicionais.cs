using Infrastructure.IntegrationsCore.Data.Schemas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.IntegrationsCore.Entities.Enums;
using Infrastructure.IntegrationsCore.Data.Extensions;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Data.Mappings.LinxMicrovix
{
    public class LinxProdutosCamposAdicionaisMap : IEntityTypeConfiguration<LinxProdutosCamposAdicionais>
    {
        public void Configure(EntityTypeBuilder<LinxProdutosCamposAdicionais> builder)
        {
            var schema = SchemaContext.GetSchema(typeof(LinxProdutosCamposAdicionais));

            builder.ToTable("LinxProdutosCamposAdicionais");

            if (schema == "linx_microvix_erp")
            {
                builder.HasKey(e => new { e.cod_produto, e.campo });
                builder.Ignore(x => x.id);
            }
            else
            {
                builder.HasKey(x => x.id);

                builder.Property(e => e.id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();
            }

            builder.Property(e => e.lastupdateon)
                .HasProviderColumnType(EnumTableColumnType.DateTime);

            builder.Property(e => e.portal)
                .HasColumnType("int");

            builder.Property(e => e.cod_produto)
                .HasColumnType("bigint");

            builder.Property(e => e.campo)
                .HasColumnType("varchar(30)");

            builder.Property(e => e.valor)
                .HasColumnType("varchar(30)");

            builder.Property(e => e.timestamp)
                .HasColumnType("bigint");
        }
    }
}
