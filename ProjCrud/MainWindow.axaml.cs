using Avalonia.Controls;
using Avalonia.Interactivity;


namespace ProjCrud;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AtualizarLista()
        {
            //lstLivros.ItemsSource = null;
            lstLivros.ItemsSource = livroDAO.Ler();
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTitulo.Text) && !string.IsNullOrWhiteSpace(txtAutor.Text) &&
            !string.IsNullOrWhiteSpace(txtEditora.Text) && int.TryParse(txtAno.Text, out int ano))
            {
                livroDAO.Criar(new Livro { Titulo = txtTitulo.Text, Autor = txtAutor.Text, 
                Editora = txtEditora.Text, Ano = ano });
                AtualizarLista();
                txtTitulo.Clear();
                txtAutor.Clear();
                txtEditora.Clear();
                txtAno.Clear();
            }
        }

        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            if (lstLivros.SelectedItem is Livro livro && int.TryParse(txtAno.Text, out int ano))
            {
                if (livro != null)
                {
                    livro.Titulo = txtTitulo?.Text ?? string.Empty;  // Usando operador null-coalescing
                    livro.Autor = txtAutor?.Text ?? string.Empty;
                    livro.Editora = txtEditora?.Text ?? string.Empty;
                    livro.Ano = ano;
                    livroDAO.Atualizar(livro);
                    AtualizarLista();
                }
            }
        }

        private void Deletar_Click(object sender, RoutedEventArgs e)
        {
            if (lstLivros.SelectedItem is Livro livro)
            {
                livroDAO.Deletar(livro.Id);
                AtualizarLista();
                txtTitulo.Clear();
                txtAutor.Clear();
                txtEditora.Clear();
                txtAno.Clear();
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
    }


