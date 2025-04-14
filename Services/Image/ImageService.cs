using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using WebAPI.Data;

namespace WebAPI.Services.Image
{
    public class ImageService : IImageInterface
    {
        private readonly AppDbContext _context;

        public ImageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetImage(string Id)
        {
            try
            {
                var objectId = new ObjectId(Id);
                var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, objectId);
                var imageCursor = await _context.GridFSBucket.FindAsync(filter);
                var image = await imageCursor.FirstOrDefaultAsync();

                if (image == null)
                {
                    return new NotFoundResult(); // Retorna 404 se a imagem não for encontrada
                }

                var stream = new MemoryStream();
                await _context.GridFSBucket.DownloadToStreamAsync(image.Id, stream);
                stream.Seek(0, SeekOrigin.Begin);

                var contentType = image.Filename.EndsWith(".png") ? "image/png" : "image/jpeg";

                return new FileStreamResult(stream, contentType);
            }
            catch (Exception)
            {
                // Logar a exceção adequadamente
                // Retorna 500 em caso de erro
                return new ObjectResult("Erro interno ao buscar a imagem.") { StatusCode = 500 };
            }
        }

        public async Task<List<string>> CreateImages(List<IFormFile> images)
        {
            var imageIds = new List<string>();

            foreach (var image in images)
            {
                try
                {
                    using var stream = image.OpenReadStream();

                    // O _id real do GridFS é retornado por esse método
                    var id = await _context.GridFSBucket.UploadFromStreamAsync(image.FileName, stream);

                    imageIds.Add(id.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao salvar imagem '{image.FileName}': {ex.Message}");
                    // Continue com o próximo arquivo
                }
            }

            return imageIds;
        }

        public async Task<List<string>> UpdateImages(List<string> ids, List<IFormFile> updateImages)
        {
            List<string> updatedIds = new List<string>(); // Inicializando a lista
            try
            {
                await DeleteImages(ids);
                updatedIds = await CreateImages(updateImages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar imagens: {ex.Message}");
                // Retorna a lista vazia ou pode configurar a resposta conforme necessário
            }
            return updatedIds;
        }

        public async Task DeleteImages(List<string> ids)
        {
            foreach (var id in ids)
            {
                try
                {
                    if (!ObjectId.TryParse(id, out ObjectId objectId))
                    {
                        Console.WriteLine($"ID inválido: {id}");
                        continue;
                    }

                    var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
                    using var cursor = await _context.GridFSBucket.FindAsync(filter);
                    var image = await cursor.FirstOrDefaultAsync();

                    if (image != null)
                    {
                        await _context.GridFSBucket.DeleteAsync(image.Id);
                        Console.WriteLine($"Imagem {id} deletada com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine($"Imagem {id} não encontrada.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao deletar imagem {id}: {ex.Message}");
                }
            }
        }
    }
}
