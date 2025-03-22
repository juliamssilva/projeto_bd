// Classe referente a um livro, com seus atributos e métodos
namespace ProjCrud
{
    public class Livro
    {
        public int Id { get; set; }

        // O string.Empty foi usado para garantir que o valor das strings não seja nulo
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Editora { get; set; } = string.Empty;
        public int Ano { get; set; }
        
        // Override do método ToString para retornar uma string com o título, autor e ano do livro
        public override string ToString()
        {
            return $"{Titulo} - {Autor} ({Ano})";
        }
    }
}