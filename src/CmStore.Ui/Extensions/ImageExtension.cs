using CmStore.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CmStore.Ui.Extensions
{
    public static class ImageExtension
    {
        public static string GetUrlImageProduto(this IHtmlHelper html, Produto produto)
        {
            var pathImagens = "/imagens";
            var imageDefault = "img-not-found.jpg";

            if (produto != null && !string.IsNullOrWhiteSpace(produto.Imagem))
            {
                return $"{pathImagens}/{produto.Imagem}";
            }

            return $"{pathImagens}/{imageDefault}";  
        }
    }
}
