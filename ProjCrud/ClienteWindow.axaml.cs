using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace ProjCrud
{
    public partial class ClienteWindow : Window
    {
        private List<Cliente> clientes = new List<Cliente>(); // Inicializar para evitar warnings

        public ClienteWindow()
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
                clientes = clientesDAO.Ler();
                lstClientes.Items.Clear();
                
                foreach (var cliente in clientes)
                {
                    lstClientes.Items.Add(cliente);
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
                if (!string.IsNullOrWhiteSpace(txtCpfCliente.Text) && 
                    !string.IsNullOrWhiteSpace(txtNomeCliente.Text) &&
                    !string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    
                        var novoCliente = new Cliente
                        {
                            CpfCliente = txtCpfCliente.Text,
                            NomeCliente = txtNomeCliente.Text,
                            Email = txtEmail.Text ?? string.Empty,
                            IsFlamengo = chkIsFlamengo.IsChecked ?? false,      
                            IsOnePieceFan = chkIsOnePieceFan.IsChecked ?? false, 
                            IsTeixeira = chkIsTeixeira.IsChecked ?? false 
                        };

                        clientesDAO.Criar(novoCliente);
                        AtualizarLista();
                        LimparCampos();
                }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("CPF deve ser um número inteiro válido");
                    }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao adicionar cliente: {ex.Message}");
            }
        }

        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstClientes.SelectedItem is Cliente cliente )
                {
                    cliente.CpfCliente = txtCpfCliente.Text ?? string.Empty;
                    cliente.NomeCliente = txtNomeCliente.Text ?? string.Empty;
                    cliente.Email = txtEmail.Text ?? string.Empty;
                    cliente.IsFlamengo = chkIsFlamengo.IsChecked ?? false;
                    cliente.IsOnePieceFan = chkIsOnePieceFan.IsChecked ?? false;
                    cliente.IsTeixeira = chkIsTeixeira.IsChecked ?? false;
                    
                    clientesDAO.Atualizar(cliente);
                    AtualizarLista();
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
            }
        }

        private void Deletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstClientes.SelectedItem is Cliente cliente)
                {
                    clientesDAO.Deletar(cliente.CpfCliente);
                    AtualizarLista();
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao deletar cliente: {ex.Message}");
            }
        }

        private void LstClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstClientes.SelectedItem is Cliente cliente)
                {
                    txtCpfCliente.Text = cliente.CpfCliente.ToString();
                    txtNomeCliente.Text = cliente.NomeCliente;
                    txtEmail.Text = cliente.Email;
                    chkIsFlamengo.IsChecked = cliente.IsFlamengo;
                    chkIsOnePieceFan.IsChecked = cliente.IsOnePieceFan;
                    chkIsTeixeira.IsChecked = cliente.IsTeixeira;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao selecionar cliente: {ex.Message}");
            }
        }

        private void LimparCampos()
        {
            txtCpfCliente.Text = string.Empty;
            txtNomeCliente.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chkIsFlamengo.IsChecked = false;
            chkIsOnePieceFan.IsChecked = false;
            chkIsTeixeira.IsChecked = false;
        }

        private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCpfCliente.Text))
                {
                    Cliente clienteEncontrado = clientesDAO.Pesquisar(txtCpfCliente.Text);
                    if (clienteEncontrado != null)
                    {
                        lstClientes.Items.Clear();
                        lstClientes.Items.Add(clienteEncontrado);
                    }
                    else if (string.IsNullOrWhiteSpace(txtCpfCliente.Text))
                    {
                        System.Diagnostics.Debug.WriteLine("CPF não pode ser vazio ou nulo");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Cliente não encontrado");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao pesquisar cliente: {ex.Message}");
            }
        }

        private void MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPesquisarCpf.Text = string.Empty;
                lstClientes.Items.Clear();
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
