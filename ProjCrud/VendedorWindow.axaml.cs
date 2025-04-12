using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace ProjCrud
{
    public partial class VendedorWindow : Window
    {
        private List<Vendedor> vendedores = new List<Vendedor>(); // Inicializar para evitar warnings
        public VendedorWindow()
        {
            try
            {
                InitializeComponent();
                AtualizarLista();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao inicializar: {ex.Message}");
            }
        }

        private void AtualizarLista()
        {
            try
            {
                vendedores = vendedorDAO.Ler();
                lstVendedores.Items.Clear();
                
                foreach (var vendedor in vendedores)
                {
                    lstVendedores.Items.Add(vendedor);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar lista: {ex.Message}");
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ( !string.IsNullOrWhiteSpace(txtNomeVendedor.Text) &&
                    decimal.TryParse(txtSalario.Text, out decimal salario))
                {
                    
                        var novoVendedor = new Vendedor
                        {
                            NomeVendedor = txtNomeVendedor.Text,
                            Salario = salario,
                            
                        };

                        vendedorDAO.Criar(novoVendedor);
                        AtualizarLista();
                        LimparCampos();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Dados inválidos para adicionar vendedor.");
                }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao adicionar vendedor: {ex.Message}");
            }
        }

        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstVendedores.SelectedItem is Vendedor vendedor
                 && decimal.TryParse(txtSalario.Text, out decimal salario))
                {
                    vendedor.NomeVendedor = txtNomeVendedor.Text ?? string.Empty;
                    vendedor.Salario = salario;
                    
                    vendedorDAO.Atualizar(vendedor);
                    AtualizarLista();
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar vendedor: {ex.Message}");
            }
        }

        private void Deletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstVendedores.SelectedItem is Vendedor vendedor)
                {
                    vendedorDAO.Deletar(vendedor.IdVendedor);
                    AtualizarLista();
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao deletar cliente: {ex.Message}");
            }
        }

        private void LstVendedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstVendedores.SelectedItem is Vendedor vendedor)
                {
                    txtNomeVendedor.Text = vendedor.NomeVendedor;
                    txtSalario.Text = vendedor.Salario.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao selecionar vendedor: {ex.Message}");
            }
        }

        private void LimparCampos()
        {
            
            txtNomeVendedor.Text = string.Empty;
            txtSalario.Text = string.Empty;
        }

        private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string termoPesquisa = txtPesquisarNomeVendedor.Text?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(termoPesquisa))
                {
                    // Recebe uma lista, não um único objeto
                    var vendedoresEncontrados = vendedorDAO.Pesquisar(termoPesquisa);
                    
                    lstVendedores.Items.Clear();
                    
                    if (vendedoresEncontrados.Count > 0)
                    {
                        foreach (var vendedor in vendedoresEncontrados)
                        {
                            lstVendedores.Items.Add(vendedor);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Nenhum vendedor encontrado");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao pesquisar vendedor: {ex.Message}");
            }
        }

        private void MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Corrija para o nome correto do controle
                txtPesquisarNomeVendedor.Text = string.Empty;
                AtualizarLista();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao mostrar todos: {ex.Message}");
            }
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
