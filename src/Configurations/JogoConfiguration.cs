using FCG.CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.CatalogAPI.Configurations
{
    public class JogoConfiguration: IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("jogos");

            // Define chave primária
            builder.HasKey(x => x.Id);

            // Força autoincremento começando em 1 e pulando de 1 em 1
            builder.Property(p => p.Id)
                .HasColumnType("INT")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1);

            builder.Property(p => p.Nome).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(p => p.Descricao).HasColumnType("VARCHAR(MAX)").IsRequired();
            builder.Property(p => p.Preco).HasColumnType("DECIMAL(18,2)").IsRequired();
            builder.Property(p => p.Situacao).HasColumnType("VARCHAR(10)").IsRequired();
        }
    }
}
