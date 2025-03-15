namespace ProjCrud
{
    public class Livro
    {
        public int Id { get; set; }
        required public string  Titulo { get; set; }
        required public string Autor { get; set; }
        required public  string Editora { get; set; }
        public int Ano { get; set; }
    }
}