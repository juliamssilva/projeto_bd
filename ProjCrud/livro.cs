namespace ProjCrud
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Editora { get; set; } = string.Empty;
        public int Ano { get; set; }
        
        public override string ToString()
        {
            return $"{Titulo} - {Autor} ({Ano})";
        }
    }
}