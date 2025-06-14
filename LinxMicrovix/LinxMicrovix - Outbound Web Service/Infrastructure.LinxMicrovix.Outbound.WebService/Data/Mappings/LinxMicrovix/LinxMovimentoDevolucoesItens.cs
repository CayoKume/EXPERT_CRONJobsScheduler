using Infrastructure.IntegrationsCore.Data.Schemas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;
using Domain.IntegrationsCore.Entities.Enums;
using Infrastructure.IntegrationsCore.Data.Extensions;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Data.Mappings.LinxMicrovix
{
    public class LinxMovimentoDevolucoesItensMap : IEntityTypeConfiguration<LinxMovimentoDevolucoesItens>
    {
        public void Configure(EntityTypeBuilder<LinxMovimentoDevolucoesItens> builder)
        {
            var schema = SchemaContext.GetSchema(typeof(LinxMovimentoDevolucoesItens));

            builder.ToTable("LinxMovimentoDevolucoesItens");

            if (schema == "linx_microvix_erp")
            {
                builder.HasKey(e => e.id_movimento_devolucoes_itens);
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

            builder.Property(e => e.id_movimento_devolucoes_itens)
                .HasColumnType("int");

            builder.Property(e => e.portal)
                .HasColumnType("int");

            builder.Property(e => e.identificador_venda)
                .HasProviderColumnType(EnumTableColumnType.UUID);

            builder.Property(e => e.cnpj_emp)
                .HasColumnType("varchar(14)");

            builder.Property(e => e.identificador_devolucao)
                .HasProviderColumnType(EnumTableColumnType.UUID);

            builder.Property(e => e.codigoproduto)
                .HasColumnType("bigint");

            builder.Property(e => e.transacao_origem)
                .HasColumnType("int");

            builder.Property(e => e.transacao_devolucao)
                .HasColumnType("int");

            builder.Property(e => e.qtde_devolvida)
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.timestamp)
                .HasColumnType("bigint");
        }
    }
}
