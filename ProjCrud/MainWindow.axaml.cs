using Avalonia.Controls;
using System;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace ProjCrud
{
    public partial class MainWindow : Window
    {
        private List<Livro> livros = new List<Livro>(); // Inicializar para evitar warnings

        public MainWindow()
        {
            try 
            {
                System.Diagnostics.Debug.WriteLine("Iniciando MainWindow");
                InitializeComponent();
                System.Diagnostics.Debug.WriteLine("InitializeComponent concluído");
                AtualizarLista();
                System.Diagnostics.Debug.WriteLine("AtualizarLista concluído");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao inicializar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        private void AbrirClienteWindow_Click(object sender, RoutedEventArgs e)
        {
            var clienteWindow = new ClienteWindow();
            clienteWindow.Show();
        }

        private void AbrirVendedorWindow_Click(object sender, RoutedEventArgs e)
        {
            var vendedorWindow = new VendedorWindow();
            vendedorWindow.Show();
        }

        private void AbrirCompraWindow_Click(object sender, RoutedEventArgs e)
        {
            var compraWindow = new CompraWindow();
            compraWindow.Show();
        }


        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar se todos os campos necessários estão preenchidos e são válidos
                if (!string.IsNullOrWhiteSpace(txtTitulo.Text) && !string.IsNullOrWhiteSpace(txtAutor.Text) &&
                    !string.IsNullOrWhiteSpace(txtEditora.Text) && int.TryParse(txtAno.Text, out int ano) &&
                    !string.IsNullOrWhiteSpace(txtCategoria.Text) && decimal.TryParse(txtPreco.Text, out decimal preco) &&
                    int.TryParse(txtEstoque.Text, out int estoque))
                {
                    var novoLivro = new Livro 
                    { 
                        Titulo = txtTitulo.Text, 
                        Autor = txtAutor.Text, 
                        Editora = txtEditora.Text, 
                        Ano = ano,
                        Categoria = txtCategoria.Text,
                        Preco = preco,
                        Estoque = estoque
                    };
                    
                    // Adicionar o livro no banco de dados
                    livroDAO.Criar(novoLivro);
                    
                    // Atualizar a lista de livros e limpar os campos
                    AtualizarLista();
                    LimparCampos();
                    
                    // Opcional: mostrar uma mensagem de sucesso
                    System.Diagnostics.Debug.WriteLine("Livro adicionado com sucesso!");
                }
                else
                {
                    // Log para dados inválidos
                    System.Diagnostics.Debug.WriteLine("Não foi possível adicionar o livro: dados inválidos ou incompletos.");
                    
                    // Você pode adicionar um TextBlock na interface para mostrar erros
                    // txtError.Text = "Por favor, preencha todos os campos corretamente.";
                    // txtError.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                // Log detalhado do erro para diagnóstico
                System.Diagnostics.Debug.WriteLine($"Erro ao adicionar livro: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Se houver uma exceção interna, também registre
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                // Você pode adicionar um TextBlock na interface para mostrar erros
                // txtError.Text = $"Erro ao adicionar livro: {ex.Message}";
                // txtError.IsVisible = true;
            }
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

 
        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            if (lstLivros.SelectedItem is Livro livro && int.TryParse(txtAno.Text, out int ano))
            {
                livro.Titulo = txtTitulo.Text ?? string.Empty;
                livro.Autor = txtAutor.Text ?? string.Empty;
                livro.Editora = txtEditora.Text ?? string.Empty;
                livro.Ano = ano;
                livro.Categoria = txtCategoria.Text ?? string.Empty;
                livro.Preco = decimal.TryParse(txtPreco.Text, out decimal preco) ? preco : 0;
                livro.Estoque = int.TryParse(txtEstoque.Text, out int estoque) ? estoque : 0;
                
                livroDAO.Atualizar(livro);
                AtualizarLista();
                LimparCampos();
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
                txtCategoria.Text = livro.Categoria;
                txtPreco.Text = livro.Preco.ToString(); //
                txtEstoque.Text = livro.Estoque.ToString();
            }
        }
        
        private void LimparCampos()
        {
            txtTitulo.Text = string.Empty;
            txtAutor.Text = string.Empty;
            txtEditora.Text = string.Empty;
            txtAno.Text = string.Empty;
            txtCategoria.Text = string.Empty;
            txtPreco.Text = string.Empty;
            txtEstoque.Text = string.Empty;
            lstLivros.SelectedItem = null; // Limpar seleção
        }

        private void Pesquisar_Click_Estoque(object sender, RoutedEventArgs e)
        {
            string termoPesquisa = txtPesquisarEstoque.Text?.Trim() ?? string.Empty;

            if (int.TryParse(termoPesquisa, out int termoPesquisaInt))
            {
                // Chamar o método de pesquisa com o termo convertido para int
                var resultadosPesquisa = livroDAO.PesquisarEstoque(termoPesquisaInt);
                ExibirResultadosPesquisa(resultadosPesquisa);
            }
        }

        private void Pesquisar_Click_Titulo(object sender, RoutedEventArgs e)
        {
            string termoPesquisa = txtPesquisarTitulo.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(termoPesquisa))
            {
                var resultadosPesquisa = livroDAO.PesquisarTitulo(termoPesquisa);
                ExibirResultadosPesquisa(resultadosPesquisa);
            }
        }

        private void Pesquisar_Click_Categoria(object sender, RoutedEventArgs e)
        {
            string termoPesquisa = txtPesquisarCategoria.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(termoPesquisa))
            {
                var resultadosPesquisa = livroDAO.PesquisarCategoria(termoPesquisa);
                ExibirResultadosPesquisa(resultadosPesquisa);
            }
        }

        private void Pesquisar_Click_Editora(object sender, RoutedEventArgs e)
        {
            string termoPesquisa = txtPesquisarEditora.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(termoPesquisa))
            {
                var resultadosPesquisa = livroDAO.PesquisarEditora(termoPesquisa);
                ExibirResultadosPesquisa(resultadosPesquisa);
            }
        }



        private void Pesquisar_Click_Preco(object sender, RoutedEventArgs e)
        {
            // Tentar converter os valores de texto para decimal
            if (decimal.TryParse(txtPrecoMin.Text?.Trim(), out decimal precoMin) && 
                decimal.TryParse(txtPrecoMax.Text?.Trim(), out decimal precoMax))
            {
                // Chamar o método com os dois parâmetros decimais
                var resultadosPesquisa = livroDAO.Faixa_de_preco(precoMin, precoMax);
                ExibirResultadosPesquisa(resultadosPesquisa);
            }
            else
            {
                // Mostrar mensagem de erro caso as conversões falhem
                // Pode ser substituído por uma mensagem na UI
                System.Diagnostics.Debug.WriteLine("Por favor, insira valores numéricos válidos para o intervalo de preço.");
            }
        }
        
        private void MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            // Limpa todos os campos de pesquisa
            txtPesquisarTitulo.Text = string.Empty;
            txtPesquisarCategoria.Text = string.Empty;
            txtPesquisarEditora.Text = string.Empty;
            txtPrecoMin.Text = string.Empty;
            txtPrecoMax.Text = string.Empty;

            // Atualiza a lista com todos os livros
            AtualizarLista();
        }
        
        private void ExibirResultadosPesquisa(List<Livro> resultados)
        {
            lstLivros.Items.Clear();
            
            foreach (var livro in resultados)
            {
                lstLivros.Items.Add(livro);
            }
        }

        // Adicione este método ao arquivo MainWindow.axaml.cs
        private void TipoPesquisa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Esconde todos os painéis de pesquisa
                pnlPesquisaTitulo.IsVisible = false;
                pnlPesquisaCategoria.IsVisible = false;
                pnlPesquisaEditora.IsVisible = false;
                pnlPesquisaPreco.IsVisible = false;
                pnlPesquisaEstoque.IsVisible = false;
                
                // Mostra apenas o painel correspondente à seleção
                switch (cmbTipoPesquisa.SelectedIndex)
                {
                    case 0: // Título
                        pnlPesquisaTitulo.IsVisible = true;
                        break;
                    case 1: // Categoria
                        pnlPesquisaCategoria.IsVisible = true;
                        break;
                    case 2: // Editora
                        pnlPesquisaEditora.IsVisible = true;
                        break;
                    case 3: // Faixa de Preço
                        pnlPesquisaPreco.IsVisible = true;
                        break;
                    case 4: // Estoque
                        pnlPesquisaEstoque.IsVisible = true;
                        break;
                }
            }
            
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao mudar tipo de pesquisa: {ex.Message}");
                // Identificar qual controle está causando o problema
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
        private void Limpar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
            AtualizarLista();
        }
    }
}