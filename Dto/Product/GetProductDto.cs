using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Dto.Product
{
    public class GetProductDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required double Price { get; set; }
        public double? Sale { get; set; }
        public required int Quantity { get; set; }
        public required string Category { get; set; }
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.

        [NotMapped]
        public List<string> ImagesId { get; set; }
    }
}
