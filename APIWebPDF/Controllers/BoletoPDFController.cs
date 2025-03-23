using Microsoft.AspNetCore.Mvc;

namespace APIWebPDF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoletoPDFController : ControllerBase
    {
        private const string DiretórioBoletos = "Boletos";
        private const string UsuarioAutorizado = "admin";
        private const string SenhaAutorizada = "senha123";

        public BoletoPDFController()
        {
            if (!Directory.Exists(DiretórioBoletos))
                Directory.CreateDirectory(DiretórioBoletos);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] UploadRequest request)
        {
            if (!Autenticar(request.Username, request.Password))
                return Unauthorized("Usuário ou senha inválidos.");

            try
            {
                // Verifica se a string Base64 é válida
                if (string.IsNullOrWhiteSpace(request.Base64))
                    return BadRequest("A string Base64 não pode estar vazia.");

                byte[] pdfBytes;
                try
                {
                    pdfBytes = Convert.FromBase64String(request.Base64);
                }
                catch (FormatException)
                {
                    return BadRequest("Formato Base64 inválido.");
                }

                // Formatar a data corretamente (yyyyMMddHHmmss)
                string formattedDate = DateTime.Now.ToString("dd-MM-yyyyHHmmss");

                // Criar o nome do arquivo (Removendo espaços desnecessários)
                string safeFileName = $"{request.CNPJ}_{formattedDate}_{request.FileName}".Replace(" ", "_");

                // Caminho completo do arquivo
                string filePath = Path.Combine(DiretórioBoletos, safeFileName);

                // Salvar o arquivo
                await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

                // Gerar a URL correta para acesso
                string fileUrl = $"{Request.Scheme}://{Request.Host}/files/{safeFileName}";

                return Ok(new { message = "Upload realizado com sucesso!", url = fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar arquivo: {ex.Message}");
            }
        }


        private bool Autenticar(string username, string password)
        {
            return username == UsuarioAutorizado && password == SenhaAutorizada;
        }
    }
}

public class UploadRequest
{
    public string CNPJ { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public string FileName { get; set; }
    public string Base64 { get; set; }
}