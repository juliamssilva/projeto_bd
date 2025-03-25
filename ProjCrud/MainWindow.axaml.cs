using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace ProjCrud
{
    public partial class MainWindow : Window
    {
        private List<Livro> livros = new List<Livro>(); // Inicializar para evitar warnings

        public MainWindow()
        {
            InitializeComponent();
            AtualizarLista();
        }

        private void AtualizarLista()
        {
            livros = livroDAO.Ler();
            lstLivros.Items.Clear(); // Limpar a coleção existente
            
            foreach (var livro in livros)
            {
                lstLivros.Items.Add(livro);
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTitulo.Text) && !string.IsNullOrWhiteSpace(txtAutor.Text) &&
                !string.IsNullOrWhiteSpace(txtEditora.Text) && int.TryParse(txtAno.Text, out int ano))
            {
                var novoLivro = new Livro 
                { 
                    Titulo = txtTitulo.Text, 
                    Autor = txtAutor.Text, 
                    Editora = txtEditora.Text, 
                    Ano = ano 
                };
                
                livroDAO.Criar(novoLivro);
                AtualizarLista();
                LimparCampos();
            }
        }

        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            if (lstLivros.SelectedItem is Livro livro && int.TryParse(txtAno.Text, out int ano))
            {
                livro.Titulo = txtTitulo.Text ?? string.Empty;
                livro.Autor = txtAutor.Text ?? string.Empty;
                livro.Editora = txtEditora.Text ?? string.Empty;
                livro.Ano = ano;
                
                livroDAO.Atualizar(livro);
                AtualizarLista();
            }
        }

        private void Deletar_Click(object sender, RoutedEventArgs e)
        {
            if (lstLivros.SelectedItem is Livro livro)
            {
                livroDAO.Deletar(livro.Id);
                AtualizarLista();
                LimparCampos();
            }
        }

        private void LstLivros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstLivros.SelectedItem is Livro livro)
            {
                txtTitulo.Text = livro.Titulo;
                txtAutor.Text = livro.Autor;
                txtEditora.Text = livro.Editora;
                txtAno.Text = livro.Ano.ToString();
            }
        }
        
        private void LimparCampos()
        {
            txtTitulo.Text = string.Empty;
            txtAutor.Text = string.Empty;
            txtEditora.Text = string.Empty;
            txtAno.Text = string.Empty;
        }

        private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            string termoPesquisa = txtPesquisa.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(termoPesquisa))
            {
                var resultadosPesquisa = livroDAO.Pesquisar(termoPesquisa);
                ExibirResultadosPesquisa(resultadosPesquisa);
            }
        }
        
        private void MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            AtualizarLista();
            txtPesquisa.Text = string.Empty;
        }
        
        private void ExibirResultadosPesquisa(List<Livro> resultados)
        {
            lstLivros.Items.Clear();
            
            foreach (var livro in resultados)
            {
                lstLivros.Items.Add(livro);
            }
        }
    }
}