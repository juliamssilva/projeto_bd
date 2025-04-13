using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProjCrud
{
    public partial class Compras : Window
    {
        private Compra? _compraSelecionada; // Tornar anulável (corrige CS8618)
        
        public Compras()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            CarregarCompras();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // Substituir ContentDialog por MessageBox simples (corrige CS0246)
        private async Task MostrarErro(string mensagem)
        {
            await new MessageDialog
            {
                Title = "Erro",
                Message = mensagem
            }.ShowDialog(this);
        }

        private async Task MostrarSucesso(string mensagem)
        {
            await new MessageDialog
            {
                Title = "Sucesso",
                Message = mensagem
            }.ShowDialog(this);
        }

        // Classe auxiliar de diálogo (já que ContentDialog não está disponível)
        private class MessageDialog : Window
        {
            public string Title { get; set; } = "";
            public string Message { get; set; } = "";

            public MessageDialog()
            {
                Width = 300;
                Height = 150;
                CanResize = false;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
                
                var panel = new StackPanel
                {
                    Margin = new Thickness(10),
                    Spacing = 10
                };
                
                panel.Children.Add(new TextBlock { Text = Message });
                
                var okButton = new Button
                {
                    Content = "OK",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                };
                
                okButton.Click += (s, e) => Close();
                panel.Children.Add(okButton);
                
                Content = panel;
            }
            
            protected override void OnOpened(EventArgs e)
            {
                base.OnOpened(e);
                this.Title = Title;
                ((panel.Children[0] as TextBlock)!).Text = Message;
            }
        }

        private void CarregarCompras()
        {
            try
            {
                dgCompras.ItemsSource = ComprasDAO.Ler();
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
        }

        private void CarregarItensCompra(int idCompra)
        {
            try
            {
                dgItens.ItemsSource = ComprasDAO.ItensPedido(idCompra);
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
        }

        private void dgCompras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCompras.SelectedItem is Compra compra)
            {
                _compraSelecionada = compra;
                CarregarItensCompra(compra.Id);
            }
        }

        private void NovaCompra_Click(object sender, RoutedEventArgs e)
        {
            pnlNovaCompra.IsVisible = true;
            pnlNovoItem.IsVisible = false;
        }

        private void AdicionarItem_Click(object sender, RoutedEventArgs e)
        {
            if (_compraSelecionada == null)
            {
                MostrarErro("Selecione uma compra primeiro");
                return;
            }
            
            pnlNovoItem.IsVisible = true;
            pnlNovaCompra.IsVisible = false;
            
            // Observe mudanças no ID do livro para carregar o preço automaticamente
            txtIdLivro.TextChanged += (s, args) => {
                if (int.TryParse(txtIdLivro.Text, out int idLivro))
                {
                    try
                    {
                        using (var conexao = Conexao.Conectar())
                        {
                            var cmd = new SqlCommand("SELECT Preco FROM Livro WHERE Id = @Id", conexao);
                            cmd.Parameters.AddWithValue("@Id", idLivro);
                            var preco = (decimal)cmd.ExecuteScalar();
                            txtPrecoUnitario.Text = preco.ToString("F2");
                        }
                    }
                    catch
                    {
                        txtPrecoUnitario.Text = "0,00";
                    }
                }
            };
        }

        private void RemoverItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgItens.SelectedItem is not ItemPedido item)
            {
                MostrarErro("Selecione um item para remover");
                return;
            }

            try
            {
                ItemPedidoDAO.Deletar(item.Id);
                CarregarItensCompra(_compraSelecionada.Id);
                ComprasDAO.CalculoTotal(_compraSelecionada.Id);
                CarregarCompras();
                MostrarSucesso("Item removido com sucesso");
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
        }

        private void CalcularTotal_Click(object sender, RoutedEventArgs e)
        {
            if (_compraSelecionada == null)
            {
                MostrarErro("Selecione uma compra primeiro");
                return;
            }

            try
            {
                ComprasDAO.CalculoTotal(_compraSelecionada.Id);
                CarregarCompras();
                MostrarSucesso("Total recalculado com sucesso");
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusca.Text))
            {
                CarregarCompras();
                return;
            }

            try
            {
                if (txtBusca.Text.Length == 11) // Assume que é CPF
                {
                    var cpf = txtBusca.Text;
                    var comprasCliente = ComprasDAO.CompraCliente(cpf);
                    
                    if (comprasCliente.Count > 0)
                    {
                        dgItens.ItemsSource = comprasCliente;
                        MostrarSucesso($"Encontrados {comprasCliente.Count} itens para o cliente");
                    }
                    else
                    {
                        MostrarErro("Nenhuma compra encontrada para este cliente");
                    }
                }
                else if (int.TryParse(txtBusca.Text, out int idVendedor))
                {
                    var vendasVendedor = ComprasDAO.Vendas_Vendedor(idVendedor);
                    
                    if (vendasVendedor.Count > 0)
                    {
                        dgItens.ItemsSource = vendasVendedor;
                        MostrarSucesso($"Encontrados {vendasVendedor.Count} itens para o vendedor");
                    }
                    else
                    {
                        MostrarErro("Nenhuma venda encontrada para este vendedor");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
        }

        private void ConfirmarCompra_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIdCompra.Text, out int id) ||
                string.IsNullOrWhiteSpace(txtCpfCliente.Text) ||
                !int.TryParse(txtIdVendedor.Text, out int idVendedor) ||
                cbFormaPagamento.SelectedItem == null)
            {
                MostrarErro("Preencha todos os campos corretamente");
                return;
            }

            try
            {
                var compra = new Compra
                {
                    Id = id,
                    CpfCliente = txtCpfCliente.Text,
                    DataCompra = dpDataCompra.SelectedDate ?? DateTime.Now,
                    IdVendedor = idVendedor,
                    Total = 0, // Será calculado depois ao adicionar itens
                    FormaPagamento = cbFormaPagamento.SelectedItem.ToString(),
                    StatusPagamento = "Pendente"
                };

                ComprasDAO.Comprar(compra);
                pnlNovaCompra.IsVisible = false;
                CarregarCompras();
                MostrarSucesso("Compra criada com sucesso");
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
        }

        private void CancelarCompra_Click(object sender, RoutedEventArgs e)
        {
            pnlNovaCompra.IsVisible = false;
            LimparCamposCompra();
        }

        private void ConfirmarItem_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtIdItem.Text, out int idItem) ||
                !int.TryParse(txtIdLivro.Text, out int idLivro) ||
                !int.TryParse(txtQuantidade.Text, out int quantidade) ||
                !decimal.TryParse(txtPrecoUnitario.Text, out decimal precoUnitario))
            {
                MostrarErro("Preencha todos os campos corretamente");
                return;
            }

            try
            {
                var item = new ItemPedido
                {
                    Id = idItem,
                    IdCompra = _compraSelecionada.Id,
                    IdLivro = idLivro,
                    Quantidade = quantidade,
                    SubTotal = precoUnitario * quantidade
                };

                ItemPedidoDAO.Adicionar(item);
                pnlNovoItem.IsVisible = false;
                CarregarItensCompra(_compraSelecionada.Id);
                ComprasDAO.CalculoTotal(_compraSelecionada.Id);
                CarregarCompras();
                LimparCamposItem();
                MostrarSucesso("Item adicionado com sucesso");
            }
            catch (Exception ex)
            {
                MostrarErro(ex.Message);
            }
        }

        private void CancelarItem_Click(object sender, RoutedEventArgs e)
        {
            pnlNovoItem.IsVisible = false;
            LimparCamposItem();
        }
        
        private void LimparCamposCompra()
        {
            txtIdCompra.Text = string.Empty;
            txtCpfCliente.Text = string.Empty;
            dpDataCompra.SelectedDate = DateTime.Now;
            txtIdVendedor.Text = string.Empty;
            cbFormaPagamento.SelectedIndex = -1;
        }
        
        private void LimparCamposItem()
        {
            txtIdItem.Text = string.Empty;
            txtIdLivro.Text = string.Empty;
            txtQuantidade.Text = string.Empty;
            txtPrecoUnitario.Text = string.Empty;
        }
    }
}